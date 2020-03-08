using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using ZeroOne.Extension.Model;
using ZeroOne.Entity;
using NLog.Web;

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

            var firstType = typeof(ProInfo);
            var secondType = typeof(ProInfo);
            if (firstType == secondType)
            {
                Console.WriteLine("==");
            }
            if (firstType.Equals(secondType))
            {
                Console.WriteLine("Equals");
            }
            //LocatingPoint start = new LocatingPoint(32.1934050000, 119.4428390000);
            //LocatingPoint end = new LocatingPoint(32.1899910000, 119.4680730000);
            //Console.WriteLine(new LocatingLine().GetDistance(start, end));
            //Console.WriteLine(Math.PI);

            //LocatingPoint start2 = new LocatingPoint(32.20440944, 119.45583541);
            //Console.WriteLine(start2.GetDistance(48.49918, 124.8857));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://*:5002/");
                });
    }
}
