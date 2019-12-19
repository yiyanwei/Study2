using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BooksApi.Models;
using BooksApi.Services;

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
            services.AddSingleton<IBookstoreDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            services.AddControllers();
            //使用大驼峰序列化json
            //.AddNewtonsoftJson(options=>options.UseMemberCasing());
            //添加BookService到依赖注入，单例模式,因为mongo client的重用原则
            services.AddSingleton<BookService>();

            //add swagger
            services.AddSwaggerGen((options)=>{
                options.SwaggerDoc("v1",new Swashbuckle.AspNetCore.Swagger.Info{
                    Version = "v1.1.0",
                    Title = "Books Api",
                    Description = "框架集合",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact{Name="yiyanwei",Email="yiyanwei@live.com"}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }
}

//https://www.cnblogs.com/RayWang/p/9216820.html
