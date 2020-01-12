using CronJobs.Infrastructure.Extensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace CronJobs
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ≤÷¥¢∫Õserviceµ»“¿¿µ◊¢»Î

            services.RegisterAssembly("CronJobs", Lifecycle.Scoped);
            //services.AddScoped(typeof(IRepositoryBase<>), typeof(EfRepositoryBase<>));

            //services.AddScoped<DbContext, bihu_apicoreContext>();

            MongoClient client = new MongoClient(SettingManager.GetValue("MongoDB:ConnectionString"));
            services.AddSingleton(client);
         
            //services.AddScoped<IRepository<User>, UserRepository>();


            #endregion

            services.AddControllers();
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
