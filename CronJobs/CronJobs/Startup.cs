using AutoMapper;
using CronJobs.Data.Dto;
using CronJobs.Infrastructure.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Model.Enums;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace CronJobs
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

            //// Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{swagger ��Ӧ3.1core�Ļ��ڲ��Խ׶Ρ�
            //    c.SwaggerDoc("v1", new Info { Title = "CronJobs", Version = "v1" });
            //    c.CustomSchemaIds(type => type.FullName); // �����ͬ�����ᱨ�������
            //    c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CronJobs.xml"));
            //});

            //services.AddAutoMapper();  ����ʹ����һ��automapper��ע�뷽ʽ
            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoProfile>();
            });
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();



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

            app.UseStaticFiles();

            app.UseRouting();
          
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
