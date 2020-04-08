using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using ZeroOne.Extension.Model;
using ZeroOne.Entity;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using System.Text.RegularExpressions;
using Hangfire;
using Hangfire.MySql;

namespace ZeroOne.WebApi
{
    public class Program
    {
        //private static int Getx(int x)
        //{
        //    Console.WriteLine(x);
        //    return x;
        //}
        public static void Main(string[] args)
        {            
            
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            //DateTime now = DateTime.Now;
            //long ticks = (now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            //Console.WriteLine(ticks);
            //Console.WriteLine(now);
            //Console.WriteLine(now.Ticks);
            //Console.WriteLine(now.ToUniversalTime());
            //Console.WriteLine(now.ToUniversalTime().Ticks);

            //Console.WriteLine(new DateTime(621355968000000000));
            //Console.WriteLine(new DateTime(ticks * 10000 + 621355968000000000).AddHours(8));

            //            1584361276650
            //2020 / 3 / 16 20:21:16
            //637199868766505249
            //2020 / 3 / 16 12:21:16
            //637199580766505249
            //1970 / 1 / 1 0:00:00


            //var firstType = typeof(ProInfo);
            //var secondType = typeof(ProInfo);
            //if (firstType == secondType)
            //{
            //    Console.WriteLine("==");
            //}
            //if (firstType.Equals(secondType))
            //{
            //    Console.WriteLine("Equals");
            //}
            //LocatingPoint start = new LocatingPoint(32.1934050000, 119.4428390000);
            //LocatingPoint end = new LocatingPoint(32.1899910000, 119.4680730000);
            //Console.WriteLine(new LocatingLine().GetDistance(start, end));
            //Console.WriteLine(Math.PI);

            //LocatingPoint start2 = new LocatingPoint(32.20440944, 119.45583541);
            //Console.WriteLine(start2.GetDistance(48.49918, 124.8857));

            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<PageSearchResult<ProCategorySearchResult>, PageSearchResult<ProCategoryResponse>>();
            //    cfg.CreateMap<ProCategorySearchResult, ProCategoryResponse>()
            //    .ForMember(x => x.CreationTime, x => x.MapFrom(y => y.CreationTime.HasValue ? y.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty));
            //});
            //char[] ss = "🥰".ToCharArray();
            //string name = "❤❤🥰";
            //|(\ud83e\udd70)
            //var match = System.Text.RegularExpressions.Regex.Matches(name, "(\ud83c[\udf00-\udfff])|(\ud83d[\udc00-\ude4f])|(\ud83d[\ude80-\udeff])");

            //byte[] unicodeBytes = System.Text.Encoding.Unicode.GetBytes("🥰");
            ////byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes("🥰");

            //byte[] temp = new byte[] { 0x3e, 0xd8, 0x70, 0xdd };
            //string name = System.Text.Encoding.Unicode.GetString(unicodeBytes);
            ////Console.WriteLine();
            //byte[] tempArray = new byte[] { 0x3c, 0xd8 };
            //for (byte i = 0x00; i < 0xff; i++)
            //{
            //    List<byte> tempList = new List<byte>();
            //    tempList.AddRange(tempArray);
            //    tempList.Add(i);
            //    tempList.Add(0xdf);
            //    name = System.Text.Encoding.Unicode.GetString(tempList.ToArray());
            //    //Console.WriteLine(System.Text.Encoding.Unicode.GetString(tempList.ToArray()));
            //}

            //var bytes = System.Text.Encoding.UTF8.GetBytes(name);
            //string formatName = System.Text.Encoding.UTF8.GetString(bytes);
            //foreach (var s in name)
            //{

            //}
            ////🥰
            //int len = name.Length;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.Configure(app=>app.UseMiddleware())
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://*:5002/");
                });
    }
}
