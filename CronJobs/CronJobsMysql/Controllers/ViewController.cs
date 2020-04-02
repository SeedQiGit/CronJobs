using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CronJobsMysql.Controllers
{
    public class ViewController: Controller
    {
        private readonly ILogger<ViewController> _logger;

        public ViewController(ILogger<ViewController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
           
            return View();
        }

       

    }
}
