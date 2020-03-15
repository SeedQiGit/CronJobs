using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AutoMapper;
using CronJobs.Data.Dto;
using Infrastructure.Extensions;
using Infrastructure.Model.Enums;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace CronJobsMysql
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            #region �ִ���service������ע��

            services.RegisterAssembly("CronJobs", Lifecycle.Scoped);
 
            MongoClient client = new MongoClient(SettingManager.GetValue("MongoDB:ConnectionString"));
            services.AddSingleton(client);
            //services.AddScoped<IRepository<User>, UserRepository>();

            #endregion

            //services.AddAutoMapper();  ����ʹ����һ��automapper��ע�뷽ʽ
            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoProfile>();
            });
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            services.AddHostedService<QuartzService>();

            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            //scheduler.ListenerManager.AddTriggerListener(new MyTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
            scheduler.ListenerManager.AddJobListener(new JobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            scheduler.Start();


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
                })
                ;
         
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
                endpoints.MapControllers();
            });

            ServiceProviderExtension.ServiceProvider = app.ApplicationServices;

        }
    }
}