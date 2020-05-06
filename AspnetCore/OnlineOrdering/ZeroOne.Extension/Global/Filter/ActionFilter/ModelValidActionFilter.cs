using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension
{
    /// <summary>
    /// 表单对象验证过滤器
    /// </summary>
    public class ModelValidActionFilter : IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> totalErrList = new List<string>();
                foreach (var key in context.ModelState.Keys)
                {
                    var item = context.ModelState[key];
                    if (item.ValidationState == ModelValidationState.Invalid)
                    {
                        List<string> errMsgList = new List<string>();
                        foreach (var error in item.Errors)
                        {
                            if (string.IsNullOrEmpty(error?.ErrorMessage))
                            {
                                if (!string.IsNullOrEmpty(error.Exception?.Message))
                                {
                                    errMsgList.Add(error.Exception.Message);
                                }
                            }
                            else
                            {
                                errMsgList.Add(error.ErrorMessage);
                            }
                        }
                        totalErrList.Add($"{key}:{string.Join('|', errMsgList)}");
                    }
                }
                if (totalErrList.Count > 0)
                {
                    throw new Exception(string.Join('\n', totalErrList));
                }
            }
        }
    }
}
