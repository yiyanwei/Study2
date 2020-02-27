using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ZeroOne.Extension.Global
{
    public class UrlParamToObjectFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// OnActionExecuting方法在Controller的Action执行前执行
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            IUrlHelper urlHelper = new UrlHelper(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor));

            string actionUrl = urlHelper.Action("Display", "User", new { id = 15 });
        }

        /// <summary>
        /// OnActionExecuted方法在Controller的Action执行后执行
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            IUrlHelper urlHelper = new UrlHelper(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor));

            string actionUrl = urlHelper.Action("About", "Home", new { id = 15 });
        }
    }
}
