﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Entity;
using ZeroOne.Extension.Global;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        protected IWebHostEnvironment Enviroment { get; set; }
        public TestController(IWebHostEnvironment env)
        {
            this.Enviroment = env;
        }
        // GET: api/Test
        //[HttpGet("Get")]
        //public string Get(BoolEnum @bool)
        //{
        //    return "bool: " + @bool;
        //}

        //[HttpGet("GetSomething")]
        //public string GetSomething(ProInfo model)
        //{
        //    string name = "zhangsan";
        //    return name;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAuthInfo")]
        public Tuple<string, string> GetAuthInfo()
        {
            DateTime now = DateTime.Now;
            long ticks = (now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            string strTimestamp = ticks.ToString();

            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.ASCII.GetBytes(strTimestamp + "5E9B1D3F36B152415BAC6FF4CAFF2A42D9");
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }

                return new Tuple<string, string>(strTimestamp, sb.ToString());
                //判断sign是否匹配
                //if (request.Sign.Trim().ToLower() != sb.ToString().ToLower())
                //{
                //    //throw new SinoException(ErrorCode.E50003, nameof(ErrorCode.E50003).GetCode());
                //}
            }

            //5E9B1D3F36B152415BAC6FF4CAFF2A42D9
        }

        [HttpGet("GetEmoji")]
        public Tuple<string, string> GetEmoji()
        {
            IList<string> emojis = new List<string>();
            IList<string> strBytes = new List<string>();
            string name = string.Empty;
            byte[] tempArray = new byte[] { 0x3d, 0xd8 };
            for (byte i = 0x80; i < 0xff; i++)
            {
                List<byte> tempList = new List<byte>();
                tempList.AddRange(tempArray);
                tempList.Add(i);
                tempList.Add(0xde);
                name = System.Text.Encoding.Unicode.GetString(tempList.ToArray());
                emojis.Add(name);
                strBytes.Add(string.Join(",", tempList));
                //Console.WriteLine(System.Text.Encoding.Unicode.GetString(tempList.ToArray()));
            }
            return new Tuple<string, string>(string.Join(",", emojis), string.Join("@", strBytes));
        }

        [HttpGet("GetSystemPath")]
        public Tuple<string, string> GetSystemPath()
        {
            return new Tuple<string, string>(Enviroment.ContentRootPath, Enviroment.WebRootPath);
        }
    }
}