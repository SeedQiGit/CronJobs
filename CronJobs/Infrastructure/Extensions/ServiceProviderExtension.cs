using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public class ServiceProviderExtension
    {
        /// <summary>
        /// 这里在stratup赋值了app.ApplicationServices
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
