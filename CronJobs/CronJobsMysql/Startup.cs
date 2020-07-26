using AutoMapper;
using CronJobsMysql.Data.Dto;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Services.Quartz;
using Infrastructure.Extensions;
using Infrastructure.Middlewares;
using Infrastructure.Model.Enums;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CronJobsMysql.Services.Quartz.Trigger;
using Infrastructure.Helper;

namespace CronJobsMysql
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region 模型、仓储和service注入及其他细节

            services.RegisterAssembly("CronJobsMysql", Lifecycle.Scoped);

            //services.AddAutoMapper();  这里使用另一种automapper的注入方式
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoProfile>();
            });
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            #endregion

            #region mysql

            services.AddScoped<DbContext, CronJobsMysqlContext>();

            #endregion

            #region 定时任务注册

            services.AddHostedService<QuartzService>();

            services.AddScoped<JobCronTrigger>();

            #endregion

            #region MVC

            //AddControllersWithViews 是mvc项目使用  api可以直接AddControllers
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    //options.SuppressModelStateInvalidFilter = true;
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var validationErrors = context.ModelState
                            .Keys
                            .SelectMany(k => context.ModelState[k].Errors)
                            .Select(e => e.ErrorMessage)
                            .ToArray();
                        var json = BaseResponse.GetBaseResponse(BusinessStatusType.ParameterError, string.Join(",", validationErrors));

                        return new BadRequestObjectResult(json)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });


            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        //如果把下面的也允许就会报错
                        //.AllowCredentials()
                        );
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();
            
            //日志记录中间件  先注册这个，其他的中间件后注册
            app.UseHttpLogMiddleware();
            app.UseExceptionHandling();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=View}/{action=Index}");
            });

            ServiceProviderExtension.ServiceProvider = app.ApplicationServices;

        }

    }
}
