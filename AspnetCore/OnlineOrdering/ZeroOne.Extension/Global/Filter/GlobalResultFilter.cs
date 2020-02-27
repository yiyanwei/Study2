using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZeroOne.Extension.Global
{
    public class GlobalResultAttribute : Attribute
    {
        public bool Ignore { get; set; }

        public GlobalResultAttribute(bool ignore = true)
        {
            this.Ignore = ignore;
        }
    }

    public class GlobalResultFilter : ResultFilterAttribute
    {
        // Token: 0x06000069 RID: 105 RVA: 0x00002B0C File Offset: 0x00000D0C
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                if (!(context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(GlobalResultAttribute)))
                {
                    IActionResult result = context.Result;
                    if (result is EmptyResult || result is ObjectResult)
                    {
                        IActionResult result2;
                        if (!(result is EmptyResult))
                        {
                            result2 = result;
                        }
                        else
                        {
                            IActionResult actionResult = new ObjectResult(null);
                            result2 = actionResult;
                        }
                        context.Result = result2;
                        ObjectResult objectResult = context.Result as ObjectResult;
                        var genenicType = objectResult.Value?.GetType();
                        if (!(genenicType == typeof(BaseResponse<>)))
                        {
                            var objType = typeof(BaseResponse<>).MakeGenericType(genenicType);
                            var obj = Activator.CreateInstance(objType);
                            //数据属性
                            var dataProp = objType.GetProperty(nameof(BaseResponse<object>.data), BindingFlags.Public | BindingFlags.Instance);
                            if (dataProp != null)
                            {
                                dataProp.SetValue(obj, objectResult.Value);
                            }
                            //状态属性
                            var successProp = objType.GetProperty(nameof(BaseResponse<object>.success), BindingFlags.Public | BindingFlags.Instance);
                            if (successProp != null)
                            {
                                successProp.SetValue(obj, true);
                            }
                            objectResult.Value = obj;
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }
    }
}
