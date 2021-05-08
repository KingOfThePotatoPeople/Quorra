using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Quorra.Utilities;

namespace Quorra
{
    public class Program
    {
        
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            // Logger Config
            LogManager.Configuration = NLogConfig.GetConfig();
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                logger.Info("Quorra server initialising.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Stopped Nexus server because of a fatal exception.");
                throw;
            }
            finally
            {
                // Flush and stop internal timers/threads before application-exit
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();
    }
}