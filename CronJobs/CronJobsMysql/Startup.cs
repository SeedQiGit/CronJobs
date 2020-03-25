using AutoMapper;
using CronJobsMysql.Data.Dto;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Services.Quartz;
using CronJobsMysql.Services.Quartz.Listeners;
using Infrastructure.Extensions;
using Infrastructure.Model.Enums;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

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
            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
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

            //var schedulerFactory = new StdSchedulerFactory();
            //var scheduler = schedulerFactory.GetScheduler().Result;
            ////scheduler.ListenerManager.AddTriggerListener(new MyTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
            //scheduler.ListenerManager.AddJobListener(new JobListener(), GroupMatcher<JobKey>.AnyGroup());
            //scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            //scheduler.Start();

            #endregion

            #region MVC

            services.AddControllers()
                .AddJsonOptions(options =>
                {
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

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=CronJob}/{action=CronJobList}");
            });

            ServiceProviderExtension.ServiceProvider = app.ApplicationServices;

        }
    }
}
