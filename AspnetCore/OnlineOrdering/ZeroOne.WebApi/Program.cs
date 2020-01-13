using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

using ZeroOne.Entity;
namespace ZeroOne.WebApi
{
    public class Program
    {
        private static int Getx(int x)
        {
            Console.WriteLine(x);
            return x;
        }
        public static void Main(string[] args)
        {
            if (Getx(1) > 1 && Getx(2) == 2 || 1 == 1)
            {
                Console.WriteLine("1=1");
            }

            // A parameter for the lambda expression.
            // ParameterExpression paramExpr = Expression.Parameter(typeof(int), "arg");

            //// Expression<Func<int, bool>> expression = 

            // // This expression represents a lambda expression
            // // that adds 1 to the parameter value.
            // LambdaExpression lambdaExpr = Expression.Lambda(
            //     Expression.Add(
            //         paramExpr,
            //         Expression.Constant(1)
            //     ),
            //     new List<ParameterExpression>() { paramExpr }
            // );


            // // Print out the expression.
            // Console.WriteLine(lambdaExpr);

            // // Compile and run the lamda expression.
            // // The value of the parameter is 1.
            // Console.WriteLine(lambdaExpr.Compile().DynamicInvoke(1));


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
