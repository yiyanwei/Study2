using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ZeroOne.Extension
{

    public static class Mapper
    {
        /// <summary>
        /// 将源对象转换成目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>返回目标对象</returns>
        public static TTarget Map<TSource, TTarget>(this TSource source) where TSource : class where TTarget : class, new()
        {
            if (source == null)
                return default;
            var sourceType = typeof(TSource);
            if (sourceType.IsClass)
            {
                var paramExp = Expression.Parameter(sourceType, "t");
                var memberInitExpression = GetExpression(paramExp, sourceType, typeof(TTarget));
                var lambda = Expression.Lambda<Func<TSource, TTarget>>(memberInitExpression, paramExp);
                var func = lambda.Compile();
                return func(source);
            }
            else
            {
                throw new Exception("不是类对象");
            }
        }

        /// <summary>
        /// 将对象source转换成目标类型对象
        /// </summary>
        /// <typeparam name="TTarget">目标类型参数</typeparam>
        /// <param name="source">source对象</param>
        /// <returns></returns>
        public static TTarget Map<TTarget>(this object source)
            where TTarget : class, new()
        {
            if (source == null)
            { return default; }
            var sourceType = source.GetType();
            if (sourceType.IsClass)
            {
                var paramExp = Expression.Parameter(sourceType, "t");
                var targetType = typeof(TTarget);
                var memberInitExpression = GetExpression(paramExp, sourceType, targetType);
                var delegateType = typeof(Func<,>).MakeGenericType(new Type[] { sourceType, targetType });
                var lambda = Expression.Lambda(delegateType, memberInitExpression, paramExp);
                var func = lambda.Compile();
                return (TTarget)func.DynamicInvoke(source);
            }
            else
            {
                throw new Exception("不是类对象");
            }
        }

        private static MemberInitExpression GetExpression(Expression paramExp, Type sourceType, Type targetType)
        {
            var memberBindings = new List<MemberBinding>();
            var targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite);
            foreach (var targetProp in targetProperties)
            {
                var sourceProp = sourceType.GetProperty(targetProp.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //判断是否有可读权限并且是公共类型
                if (sourceProp == null || !sourceProp.CanRead)
                { continue; }

                //标注NotMapped特性的属性忽略转换
                if (sourceProp.GetCustomAttribute<NotMappedAttribute>() != null)
                { continue; }

                //源对象属性表达式
                var sourcePropExp = Expression.Property(paramExp, sourceProp);

                //判断都是class 且类型不相同时
                if (targetProp.PropertyType.IsClass && sourceProp.PropertyType.IsClass && targetProp.PropertyType != sourceProp.PropertyType)
                {
                    //防止出现自己引用自己无限递归
                    if (targetProp.PropertyType != targetType && sourceProp.PropertyType != sourceType)
                    {
                        var memberInit = GetExpression(sourcePropExp, sourceProp.PropertyType, targetProp.PropertyType);
                        memberBindings.Add(Expression.Bind(targetProp, memberInit));
                        continue;
                    }
                }
                if (targetProp.PropertyType != sourceProp.PropertyType)
                    continue;
                memberBindings.Add(Expression.Bind(targetProp, sourcePropExp));
            }
            var memerInitExp = Expression.MemberInit(Expression.New(targetType), memberBindings);
            return memerInitExp;
        }
    }

    public static class Mapper<TSource, TTarget> where TSource : class where TTarget : class, new()
    {
        public readonly static Func<TSource, TTarget> Map;

        static Mapper()
        {
            if (Map == null)
                Map = GetMap();
        }

        private static Func<TSource, TTarget> GetMap()
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            var parameterExpression = Expression.Parameter(sourceType, "p");
            var memberInitExpression = GetExpression(parameterExpression, sourceType, targetType);

            var lambda = Expression.Lambda<Func<TSource, TTarget>>(memberInitExpression, parameterExpression);
            return lambda.Compile();
        }


        /// <summary>
        /// 根据转换源和目标获取表达式树
        /// </summary>
        /// <param name="parameterExpression">表达式参数p</param>
        /// <param name="sourceType">转换源类型</param>
        /// <param name="targetType">转换目标类型</param>
        /// <returns></returns>
        private static MemberInitExpression GetExpression(Expression parameterExpression, Type sourceType, Type targetType)
        {
            var memberBindings = new List<MemberBinding>();
            foreach (var targetItem in targetType.GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite))
            {
                var sourceItem = sourceType.GetProperty(targetItem.Name);

                //判断实体的读写权限
                if (sourceItem == null || !sourceItem.CanRead || sourceItem.PropertyType.IsNotPublic)
                    continue;

                //标注NotMapped特性的属性忽略转换
                if (sourceItem.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;

                var propertyExpression = Expression.Property(parameterExpression, sourceItem);

                //判断都是class 且类型不相同时
                if (targetItem.PropertyType.IsClass && sourceItem.PropertyType.IsClass && targetItem.PropertyType == sourceItem.PropertyType)
                {
                    //防止出现自己引用自己无限递归
                    if (targetItem.PropertyType != targetType && sourceItem.PropertyType != sourceType)
                    {
                        var memberInit = GetExpression(propertyExpression, sourceItem.PropertyType, targetItem.PropertyType);
                        memberBindings.Add(Expression.Bind(targetItem, memberInit));
                        continue;
                    }
                }

                if (targetItem.PropertyType != sourceItem.PropertyType)
                    continue;

                memberBindings.Add(Expression.Bind(targetItem, propertyExpression));
            }
            return Expression.MemberInit(Expression.New(targetType), memberBindings);
        }
    }



}
