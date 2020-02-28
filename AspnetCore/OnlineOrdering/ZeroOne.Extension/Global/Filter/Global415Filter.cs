using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZeroOne.Extension.Global
{
    public class Global415Filter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //判断是否返回 415 http状态码
            if (context.Result != null && context.Result is ObjectResult)
            {
                var result = context.Result as ObjectResult;
                //判断状态码是否为415
                if (result.StatusCode.HasValue && result.StatusCode.Value == 415)
                {
                    var action = context.ActionDescriptor as ControllerActionDescriptor;
                    var fromParams = action.Parameters;
                    Dictionary<object, IList<PropertyInfo>> keyValuePairs = null;
                    if (fromParams?.Count > 0)
                    {
                        keyValuePairs = new Dictionary<object, IList<PropertyInfo>>();
                        foreach (var parameter in fromParams)
                        {
                            object temp = Activator.CreateInstance(parameter.ParameterType);
                            if (temp != null)
                            {
                                keyValuePairs.Add(temp, temp.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList());
                            }
                        }

                        //遍历querystring
                        if (context.HttpContext.Request.Query != null)
                        {
                            foreach (var queryItem in context.HttpContext.Request.Query)
                            {
                                var matchs = keyValuePairs.Select(t => new Tuple<object, PropertyInfo>(t.Key, t.Value.Where(x => x.Name.ToLower() == queryItem.Key.ToLower().Trim()).FirstOrDefault())).Where(t => t.Item2 != null);
                                if (matchs != null && matchs.Count() > 0)
                                {
                                    foreach (var match in matchs)
                                    {
                                        if (queryItem.Value.Count == 1)
                                        {
                                            match.Item2.SetValue(match.Item1, queryItem.Value[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //执行结果
                    var actionResult = action.MethodInfo.Invoke(context.Controller, keyValuePairs!= null && keyValuePairs.Keys != null ? keyValuePairs.Keys.ToArray() : null);
                    //执行成功
                    result.StatusCode = 200;
                    result.Value = actionResult;
                }
            }
            base.OnResultExecuting(context);
        }
    }
}
