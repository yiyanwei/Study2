using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ZeroOne.Extension.Global
{
    public enum BoolEnum
    {
        True,
        False
    }

    public class EnumConstraint : IRouteConstraint
    {
        private Type _enumType;

        public EnumConstraint(string enumTypeName)
        {
            _enumType = Type.GetType(enumTypeName);
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            //var value = values[routeKey];
            //if (value == null)
            //{
            //    return false;
            //}

            //if (Enum.TryParse(_enumType, value.ToString(), out object result))
            //{
            //    if (Enum.IsDefined(_enumType, result))
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
