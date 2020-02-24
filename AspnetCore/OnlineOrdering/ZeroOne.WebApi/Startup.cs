using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityModel;
using System.Text;
using NLog.Web;
using SqlSugar;
using ZeroOne.Application;
using ZeroOne.Entity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ZeroOne.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ZeroOne.WebApi
{

    public class GlobalResultAttribute : Attribute
    {
        public bool Ignore { get; set; }

        public GlobalResultAttribute(bool ignore = true)
        {
            this.Ignore = ignore;
        }
    }

    public class GlobalResultFilter : ResultFilterAttribute
    {
        // Token: 0x06000069 RID: 105 RVA: 0x00002B0C File Offset: 0x00000D0C
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                if (!(context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(GlobalResultAttribute)))
                {
                    IActionResult result = context.Result;
                    if (result is EmptyResult || result is ObjectResult)
                    {
                        IActionResult result2;
                        if (!(result is EmptyResult))
                        {
                            result2 = result;
                        }
                        else
                        {
                            IActionResult actionResult = new ObjectResult(null);
                            result2 = actionResult;
                        }
                        context.Result = result2;
                        ObjectResult objectResult = context.Result as ObjectResult;
                        var genenicType = objectResult.Value?.GetType();
                        if (!(genenicType == typeof(BaseResponse<>)))
                        {
                            var objType = typeof(BaseResponse<>).MakeGenericType(genenicType);
                            var obj = Activator.CreateInstance(objType);
                            //数据属性
                            var dataProp = objType.GetProperty(nameof(BaseResponse<object>.data), BindingFlags.Public | BindingFlags.Instance);
                            if (dataProp != null)
                            {
                                dataProp.SetValue(obj, objectResult.Value);
                            }
                            //状态属性
                            var successProp = objType.GetProperty(nameof(BaseResponse<object>.success), BindingFlags.Public | BindingFlags.Instance);
                            if (successProp != null)
                            {
                                successProp.SetValue(obj, true);
                            }
                            objectResult.Value = obj;
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }
    }

    /// <summary>
    /// api 路由拦截器（第二步）
    /// 扩展了MVCoptions
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routeAttribute">自定的前缀内容</param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }

    }

    /// <summary>
    /// api 路由拦截器（第一步）
    /// </summary>
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        //接口的Apply方法
        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                // 已经标记了 RouteAttribute 的 Controller
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        // 在 当前路由上 再 添加一个 路由前缀
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);

                        // 在 当前路由上 不再 添加任何路由前缀
                        //selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }

                // 没有标记 RouteAttribute 的 Controller
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        // 添加一个 路由前缀
                        //selectorModel.AttributeRouteModel = _centralPrefix;

                        // 不添加前缀(说明：不使用全局路由，重构action，实现自定义、特殊的action路由地址)
                        selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }
            }
        }
    }

    public class Startup
    {
        public IWebHostEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalResultFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //添加服务到服务容器
            services.AddRepository();
            services.AddTransient<IProInfoService, ProInfoService>();
            services.AddTransient<IProCategoryService, ProCategoryService>();
            services.AddTransient<IUserInfoService, UserInfoService>();

            //数据库连接配置
            services.Configure<ConnectionConfig>(Configuration.GetSection("ConnectionConfig"));
            services.AddSingleton<ISqlSugarClient>(t =>
            {
                var connConfig = t.GetRequiredService<IOptions<ConnectionConfig>>().Value;
                var extMethodList = new List<SqlFuncExternal>();
                extMethodList.Add(new SqlFuncExternal()
                {
                    MethodValue = (a, b, c) =>
                    {
                        return string.Empty;
                    },
                    UniqueMethodName = "IFNULL"
                });
                connConfig.ConfigureExternalServices.SqlFuncServices = extMethodList;
                return new SqlSugarClient(connConfig);
            });

            //配置Jwt信息
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            JwtSettings jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);
            //添加身份验证

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //这里的key要进行加密
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience
                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    // ValidateLifetime = true

                };
                o.Events = new JwtBearerOverrideEvents();
            });

            //add swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API1", Version = "v1", Contact = new OpenApiContact { Name = "yiyanwei", Url = new Uri("http://www.baidu.com"), Email = "yiyanwei@live.com" } });
                //为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                //获取entity的xml文件
                var entityXmlFile = $"{typeof(BaseEntity<>).Assembly.GetName().Name}.xml";
                var entityXmlPath = Path.Combine(AppContext.BaseDirectory, entityXmlFile);
                c.IncludeXmlComments(entityXmlPath);
            });

            //添加日志容器服务
            services.AddLogging(logger =>
            {
                logger.AddNLog($"nlog.{Environment.EnvironmentName}.config");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            //启用身份验证中间件
            app.UseAuthentication();
            //启用路由中间件
            app.UseRouting();
            //如果没有匹配上的路由

            app.UseAuthorization();
            //启用swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });

            //var routes = new RouteBuilder(app)
            //{
            //    DefaultHandler = app.ApplicationServices.GetRequiredService<MvcRouteHandler>()
            //};

            
            //app.UseRewriter();
            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404)
            //    {
            //        context.Request.Path = "/Home";
            //        await next();
            //    }
            //});

            //app.UseMvc(options => {
                
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
