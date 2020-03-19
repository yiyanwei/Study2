using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZeroOne.Extension
{
    public interface IAPIService { }

    public static class ServiceExtension
    {
        /// <summary>
        /// API权限验证
        /// </summary>
        //public static void ValidAuthentication(this IAPIService apiService, APIInvokeRequest request)
        //{
        //    if (xuGongApi == null || string.IsNullOrEmpty(xuGongApi.SecretKey))
        //    {
        //        //throw new SinoException();
        //    }
        //    //判断时间戳是否有效
        //    long ltimestamp = 0;
        //    if (string.IsNullOrEmpty(request.Timestamp) || request.Timestamp.Trim().Length != 13 || !long.TryParse(request.Timestamp.Trim(), out ltimestamp))
        //    {
        //        //throw new SinoException(ErrorCode.E50001, nameof(ErrorCode.E50001).GetCode());
        //    }
        //    //判断sign是否为空
        //    if (string.IsNullOrEmpty(request.Sign))
        //    {
        //        //throw new SinoException(ErrorCode.E50002, nameof(ErrorCode.E50002).GetCode());
        //    }
        //    DateTime now = DateTime.Now.ToUniversalTime();
        //    DateTime dTimeStamp = new DateTime(ltimestamp * 10000 + 621355968000000000);
        //    TimeSpan span = now.Subtract(dTimeStamp);
        //    //判断是否超过2分钟
        //    if (span.TotalSeconds > 120)
        //    {
        //        //throw new SinoException(ErrorCode.E50004, nameof(ErrorCode.E50004).GetCode());
        //    }
        //    string strTimestamp = request.Timestamp.Trim();
        //    using (MD5 mi = MD5.Create())
        //    {
        //        byte[] buffer = Encoding.ASCII.GetBytes(strTimestamp + xuGongApi.SecretKey);
        //        //开始加密
        //        byte[] newBuffer = mi.ComputeHash(buffer);
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < newBuffer.Length; i++)
        //        {
        //            sb.Append(newBuffer[i].ToString("x2"));
        //        }
        //        //判断sign是否匹配
        //        if (request.Sign.Trim().ToLower() != sb.ToString().ToLower())
        //        {
        //            //throw new SinoException(ErrorCode.E50003, nameof(ErrorCode.E50003).GetCode());
        //        }
        //    }
        //}
    }
}
