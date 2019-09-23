using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;

namespace AzureDynamicDNS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var processname = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(processname + ".log")
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the service");
                return;
                
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                    })
                    .UseSerilog();
        }
    }

}
