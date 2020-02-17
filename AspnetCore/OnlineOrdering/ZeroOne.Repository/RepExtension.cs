using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using SqlSugar;
using ZeroOne.Entity;
using ZeroOne.Extension;

namespace ZeroOne.Repository
{

    public interface IBulkAddOrUpdate
    {
        /// <summary>
        /// sqlsugar客户端对象
        /// </summary>
        ISqlSugarClient Client { get; set; }
    }

    /// <summary>
    /// 仓库扩展类
    /// </summary>
    public static class RepExtension
    {
        public static void BeforeAction(this IBulkAddOrUpdate bulk, ISqlSugarClient client, Type type)
        {
            if (type == null)
            {
                throw new Exception("Model类型为空");
            }
            //实体类型
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var tableAttribute = type.GetCustomAttribute<SugarTable>();
            string tempTableName = tableAttribute != null ? tableAttribute.TableName : type.Name;

            StringBuilder sbTempTable = new StringBuilder();
            //创建数据库表sql字符串
            sbTempTable.Append($" create table if not exists {tempTableName}(");
            foreach (var prop in properties)
            {
                sbTempTable.Append($" {prop.Name} {GetMySqlDbType(prop.PropertyType)},");
            }
            sbTempTable = sbTempTable.Remove(sbTempTable.Length - 1, 1);
            sbTempTable.Append(" ) ");
            //创建临时表
            client.Ado.ExecuteCommand(sbTempTable.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="bulk"></param>
        /// <param name="models"></param>
        /// <param name="bulkAddRecords"></param>
        /// <param name="beforeAction"></param>
        /// <param name="afterAction"></param>
        /// <returns></returns>
        public static bool BulkAddOrUpdate<TSource, TTarget>(this IBulkAddOrUpdate bulk, IList<TSource> models, int bulkAddRecords = 1000, Action<ISqlSugarClient, Type> beforeAction = null, Action<ISqlSugarClient, Type, Type, string> afterAction = null) where TSource : IBulkModel, new() where TTarget : new()
        {
            int length = 0;
            if (models == null || models.Count() <= 0)
            {
                throw new Exception("数据集为空");
            }
            else
            {
                length = models.Count();
            }

            var client = bulk.Client;
            //实体类名
            Type modelType = typeof(TSource);
            var tableAttribute = modelType.GetCustomAttribute<SugarTable>();
            string tempTableName = tableAttribute != null ? tableAttribute.TableName : modelType.Name;

            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                try
                {
                    //开启事务
                    client.Ado.BeginTran();

                    //执行批量插入之前是否有操作
                    beforeAction?.Invoke(client, modelType);

                    //获取所有公共属性名
                    var columnNames = properties.Select(t => t.Name);
                    int step = 0;
                    bool isExactDivision = (length % bulkAddRecords == 0);
                    //批量插入数据
                    if (isExactDivision)
                    {
                        step = length / bulkAddRecords;
                    }
                    else
                    {
                        step = (int)(length / bulkAddRecords) + 1;
                    }

                    //批量标识数据
                    string bulkIdentityVal = string.Empty;

                    int start = 0;
                    int affecedRows = 0;
                    for (int i = 0; i < step; i++)
                    {
                        start = bulkAddRecords * i;
                        int stepSize = 0;
                        //判断是否为最后一段
                        if (step - i - 1 == 0)
                        {
                            stepSize = length - start;
                        }
                        else
                        {
                            stepSize = bulkAddRecords;
                        }
                        StringBuilder insertSql = new StringBuilder();
                        insertSql.Append($" insert into {tempTableName} values ");
                        //遍历按步长大小分多次插入数据库
                        for (int j = start; j < start + stepSize; j++)
                        {
                            IList<string> vals = new List<string>();
                            foreach (var columnName in columnNames)
                            {
                                var prop = properties.First(t => t.Name == columnName);
                                var propVal = prop.GetValue(models[j]);
                                vals.Add(GetMySqlDataType(prop.PropertyType, propVal));
                                if (columnName == nameof(IBulkModel.BulkIdentity) && string.IsNullOrWhiteSpace(bulkIdentityVal))
                                {
                                    bulkIdentityVal = vals[vals.Count - 1];
                                }
                            }
                            //转换成以,分割的值的字符串
                            string strVal = string.Join(",", vals);
                            insertSql.Append($"({strVal}),");
                        }
                        insertSql = insertSql.Remove(insertSql.Length - 1, 1);
                        insertSql.Append(";");
                        affecedRows += client.Ado.ExecuteCommand(insertSql.ToString());

                    }
                    //如果插入成功的数据与传入的数据量一致
                    if (affecedRows == length)
                    {

                        //1.数据操作对象，2：操作目标表对象类型，3：临时表对象类型，4：批量操作的批次标识
                        afterAction?.Invoke(client, modelType, typeof(TTarget), bulkIdentityVal);
                        //提交事务
                        client.Ado.CommitTran();
                    }
                    else
                    {
                        //回滚事务
                        client.Ado.RollbackTran();
                    }
                }
                catch (Exception ex)
                {
                    client.Ado.RollbackTran();
                    throw ex;
                }
                finally
                {
                    client.Close();
                }
                return true;
            }
            else
            {
                throw new Exception($"该{nameof(TSource)}类没有可实例化的公共属性");
            }
        }

        private static string GetMySqlDataType(Type propValType, object val)
        {
            if (val == null)
            {
                return "NULL";
            }

            else if (propValType == typeof(int) || propValType == typeof(int?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(long) || propValType == typeof(long?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(decimal) || propValType == typeof(decimal?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(double) || propValType == typeof(double?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(float) || propValType == typeof(float?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(DateTime) || propValType == typeof(DateTime?))
            {
                return $"'{val.ToString()}'";
            }
            else if (propValType == typeof(Guid) || propValType == typeof(Guid?))
            {
                return $"'{val.ToString()}'";
            }
            else if (propValType == typeof(string))
            {
                return $"'{val.ToString()}'";
            }
            else if (propValType == typeof(bool) || propValType == typeof(bool?))
            {
                return val.ToString().ToLower();
            }
            return "NULL";
        }

        private static string GetMySqlDbType(Type propType)
        {
            if (propType == typeof(int) || propType == typeof(int?))
            {
                return "int";
            }
            else if (propType == typeof(long) || propType == typeof(long?))
            {
                return "bigint";
            }
            else if (propType == typeof(decimal) || propType == typeof(decimal?))
            {
                return "decimal";
            }
            else if (propType == typeof(double) || propType == typeof(double?))
            {
                return "double";
            }
            else if (propType == typeof(float) || propType == typeof(float?))
            {
                return "float";
            }
            else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
            {
                return "datetime";
            }
            else if (propType == typeof(Guid) || propType == typeof(Guid?))
            {
                return "char(36)";
            }
            else if (propType == typeof(string))
            {
                return "varchar(255)";
            }
            else if (propType == typeof(bool) || propType == typeof(bool?))
            {
                return "bit";
            }
            return string.Empty;
        }
    }
}