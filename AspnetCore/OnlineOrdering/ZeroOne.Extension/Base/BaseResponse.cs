using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }

        public string errMsg { get; set; }
        public string errCode { get; set; }
        public bool success { get; set; }
        public T data { get; set; }
    }
}
