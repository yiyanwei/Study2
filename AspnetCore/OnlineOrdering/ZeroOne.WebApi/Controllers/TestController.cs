using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZeroOne.Application;
using ZeroOne.Entity;
using ZeroOne.Extension.Global;
using ZeroOne.Extension.Model;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private static int count = 1;
        protected IWebHostEnvironment Enviroment { get; set; }

        protected IMapper Mapper { get; set; }

        protected IDistrictService DistrictService { get; set; }

        public TestController(IWebHostEnvironment env, IDistrictService districtService, IMapper mapper)
        {
            this.Enviroment = env;
            this.Mapper = mapper;
            this.DistrictService = districtService;
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

        [HttpGet("GetDistrict")]
        public async void GetDistrict()
        {
            string filePath = Enviroment.ContentRootPath + "/Data/District.json";
            DistrictResponse districtResponse = null;
            if (System.IO.File.Exists(filePath))
            {
                var fileStream = System.IO.File.OpenRead(filePath);
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream);
                string val = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(val))
                {
                    val = System.Text.RegularExpressions.Regex.Replace(val, "\"citycode\":.?\\[\\]", "\"citycode\": null");
                    districtResponse = JsonConvert.DeserializeObject<DistrictResponse>(val);
                    if (districtResponse?.districts?.Count > 0 && districtResponse.districts[0].districts?.Count > 0)
                    {
                        //目标对象
                        List<District> districts = new List<District>();
                        this.AddList(districts, districtResponse.districts[0].districts, null);
                        if (districts?.Count > 0)
                        {
                            DateTime now = DateTime.Now;
                            districts = districts.Select(t =>
                            {
                                t.CreationTime = now;
                                return t;
                            }).ToList();
                            var rows = await this.DistrictService.AddEntityListAsync(districts);
                        }
                    }
                }
            }

            //return JsonConvert.SerializeObject(districtResponse);
        }

        private void AddList(List<District> targets, List<Districts> sources, Guid? parentId)
        {
            if (sources?.Count > 0)
            {
                foreach (var item in sources)
                {
                    District target = Mapper.Map<District>(item);
                    target.Id = Guid.NewGuid();
                    target.ParentId = parentId;
                    targets.Add(target);
                    this.AddList(targets, item.districts, target.Id);
                }
            }
        }

        [HttpGet("GetData")]
        public string GetData([FromQuery]string tag)
        {
            count++;
            if (count > 100)
            {
                count = 2;
            }
            TagPosition tagPosition = null;
            string filePath = Enviroment.ContentRootPath + "/mydata1.txt";
            if (System.IO.File.Exists(filePath))
            {
                var fileStream = System.IO.File.OpenRead(filePath);
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream);
                string val = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(val))
                {
                    tagPosition = JsonConvert.DeserializeObject<TagPosition>(val);
                    if (tag?.Length > 0)
                    {
                        tag = tag.Trim(',');
                        string[] tags = tag.Split(',');
                        tagPosition.tags = tagPosition.tags.Where(t => tags.Contains(t.id)).Select((t, i) =>
                        {
                            if (t.smoothedPosition?.Length >= 2)
                            {
                                t.smoothedPosition[0] = t.smoothedPosition[0] + i + 1;
                                t.smoothedPosition[1] = t.smoothedPosition[1] + i + 1 + count;
                            }
                            return t;
                        }).ToList();
                    }
                }
            }
            if (tagPosition == null)
            {
                tagPosition = new TagPosition();
            }
            return JsonConvert.SerializeObject(tagPosition);
            //return tagPosition;
        }

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