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
        public static bool BulkAddOrUpdate<TModel>(this IBulkAddOrUpdate bulk, IList<TModel> models, string tableName, int bulkAddRecords = 1000, bool isSameUpdate = false, string uniqueFieldName = null) where TModel : new()
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

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("未提供数据库表名");
            }

            if (isSameUpdate)
            {
                if (string.IsNullOrWhiteSpace(uniqueFieldName))
                {
                    throw new Exception("在isSameUpdate为true的情况下，未提供唯一键的字段名");
                }
            }

            var client = bulk.Client;
            //实体类名
            Type modelType = typeof(TModel);
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                try
                {
                    //开启事务
                    client.Ado.BeginTran();
                    string tempTableName = $"{tableName}_{DateTime.Now.ToString("yyyyMMdd")}";
                    StringBuilder sbTempTable = new StringBuilder();
                    //创建数据库表sql字符串
                    sbTempTable.Append($" create temporary table if not exists {tempTableName}(");
                    IList<string> columnNames = new List<string>();
                    //List<DbColumnInfo> columns = new List<DbColumnInfo>();
                    foreach (var prop in properties)
                    {
                        //DbColumnInfo column  = new DbColumnInfo()
                        sbTempTable.Append($" {prop.Name} {GetMySqlDbType(prop.PropertyType)},");
                        columnNames.Add(prop.Name);
                    }
                    sbTempTable = sbTempTable.Remove(sbTempTable.Length - 1, 1);
                    sbTempTable.Append(" ) ");
                    //创建临时表
                    client.Ado.ExecuteCommand(sbTempTable.ToString());

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
                            }
                            //转换成以,分割的值的字符串
                            string strVal = string.Join(",", vals);
                            insertSql.Append($"({strVal}),");
                        }
                        insertSql = insertSql.Remove(insertSql.Length - 1, 1);
                        insertSql.Append(";");
                        affecedRows +=  client.Ado.ExecuteCommand(insertSql.ToString());

                    }
                    //如果插入成功的数据与传入的数据量一致
                    if (affecedRows == length)
                    {
                        //判断是否直接添加数据到实际数据表中
                        StringBuilder synchronousData = new StringBuilder();
                        if (!isSameUpdate)
                        {
                            synchronousData.Append($" insert into {tableName} ({string.Join(",", columnNames)}) select {string.Join(",", columnNames)} from {tempTableName}");
                        }
                        else
                        {
                            synchronousData.Append($" insert into {tableName} ({string.Join(",", columnNames)}) select {string.Join(",", columnNames)} from {tempTableName}");
                        }
                        //结束事务
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
                throw new Exception($"该{nameof(TModel)}类没有可实例化的公共属性");
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