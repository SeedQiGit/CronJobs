using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace Infrastructure.Extensions
{
    public class SettingManager
    {
        private static readonly IConfiguration Configuration;
        private static readonly string EnvironmentName;

        static SettingManager()
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            provider.Load();
            provider.TryGet("ASPNETCORE_ENVIRONMENT", out string environmentName);
            EnvironmentName=environmentName;
            var aa=Directory.GetCurrentDirectory();
            //使用reloadOnChange为true会造成有线程监听配置文件   可以考虑使用依赖注入配置文件或者改为false
            //https://blog.csdn.net/hiliqi/article/details/80953502
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
            .AddJsonFile("host.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"host.{environmentName}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(Configuration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

        /// <summary>
        /// 获取配置节点儿的值，目前没有固定在：AppSettings节点
        /// key的使用规则详情看这里：https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
        /// </summary>
        /// <param name="key">不区分大小写</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Configuration[key];
        }

        public static T GetValue<T>(string key)
        {
            return Configuration.GetValue<T>(key);
        }
        
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration.GetSection(key);
        }
    }
}
