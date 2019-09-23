using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureDynamicDNS.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureDynamicDNS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private AppSettings _appsetting { get; set; }
        private DynamicDNS dyndns;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "AZUREDYNDNS_")
                .Build();
            var appsetting = new AppSettings();
            configuration.Bind(appsetting);

            _appsetting = appsetting;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            dyndns = new DynamicDNS(_logger);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            dyndns.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                dyndns.RunNoLoop();
                int waittime = _appsetting.updateinterval * 60000;
                await Task.Delay(waittime, stoppingToken);
            }
        }
    }
}
