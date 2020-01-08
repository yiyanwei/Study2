using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net.NetworkInformation;

namespace BooksApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BookstoreDatabaseSettings>(Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            services.AddSingleton<IBookstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            services.AddControllers()
            //使用大驼峰序列化json
            .AddNewtonsoftJson(options => options.UseMemberCasing());
            //添加BookService到依赖注入，单例模式,因为mongo client的重用原则
            services.AddSingleton<BookService>();

            //add swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API2", Version = "v2", Contact = new OpenApiContact { Name = "yiyanwei", Url = new Uri("http://www.baidu.com"), Email = "yiyanwei@live.com" } });
                //为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1.0", new Info { Title = "My Demo API", Version = "1.0" });
            //     c.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, "ZhiKeCore.API.xml"));
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applLifetime)
        {
            //ConfigurationBinder.Bind()

            //RabbitMQ.Client.ConnectionFactory
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //获取ip地址
            string ip = string.Empty;
            string adapterName="VMnet8";
            var matchAdapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                     .Where(p=>p.Name.Contains(adapterName));
            if(matchAdapters!=null&&matchAdapters.Count()>0)
            {
                var tpAdapter = matchAdapters.FirstOrDefault();
                var ips = tpAdapter.GetIPProperties()
                         .UnicastAddresses
                         //ipv4地址并且不是环回地址（127开头，可以绕过tcp/ip协议的底层）
                         .Where(t=>t.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork&&!System.Net.IPAddress.IsLoopback(t.Address));
                if(ips!=null&&ips.Count()>0)
                {
                   ip = ips.FirstOrDefault().Address.ToString();
                }
            }


            // .Select(p => p.GetIPProperties())
            // .SelectMany(p => p.UnicastAddresses)
            // .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address));
            


            // NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            // foreach (NetworkInterface adapter in adapters)
            // {
            //     Console.WriteLine("描述：" + adapter.Description);
            //     Console.WriteLine("标识符：" + adapter.Id);
            //     Console.WriteLine("名称：" + adapter.Name);
            //     Console.WriteLine("类型：" + adapter.NetworkInterfaceType);
            //     Console.WriteLine("速度：" + adapter.Speed * 0.001 * 0.001 + "M");
            //     Console.WriteLine("操作状态：" + adapter.OperationalStatus);
            //     Console.WriteLine("MAC 地址：" + adapter.GetPhysicalAddress());

            //     if(adapter.Name.Contains(adapterName))
            //     {
            //         Console.WriteLine("match success");
            //     }
 
            //     // 格式化显示MAC地址                
            //     PhysicalAddress pa = adapter.GetPhysicalAddress();//获取适配器的媒体访问（MAC）地址
            //     byte[] bytes = pa.GetAddressBytes();//返回当前实例的地址
            //     StringBuilder sb = new StringBuilder();
            //     for (int i = 0; i < bytes.Length; i++)
            //     {                    
            //         sb.Append(bytes[i].ToString("X2"));//以十六进制格式化
            //         if (i != bytes.Length - 1)
            //         {
            //             sb.Append("-");
            //         }
            //     }
            //     Console.WriteLine("MAC 地址：" + sb);
            //     Console.WriteLine();
            // }

            // ServiceEntity serviceEntity = new ServiceEntity
            // {
            //     IP = ip,
            //     Port = Convert.ToInt32(Configuration["Service:Port"]),
            //     ServiceName = Configuration["Service:Name"],
            //     ConsulIP = Configuration["Consul:IP"],
            //     ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])
            // };

            // app.RegisterConsul(applLifetime, serviceEntity);

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            });
        }
    }
}