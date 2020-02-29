using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static int GetCode(this string strCode)
        {
            if (!string.IsNullOrWhiteSpace(strCode))
            {
                return int.Parse(strCode.Substring(1));
            }
            //没有访问权限
            return 10001;
        }
    }
}
