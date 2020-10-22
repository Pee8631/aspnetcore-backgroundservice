using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using aspnetcore_backgroundservice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetcore_backgroundservice
{
    public class UploadBackgroundService : BackgroundService
    {
        private readonly ChannelReader<UploadModel> _channel;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UploadBackgroundService> _logger;
        public UploadBackgroundService(IWebHostEnvironment environment,
                                       ILogger<UploadBackgroundService> logger,
                                       ChannelReader<UploadModel> channel)
        {
            _environment = environment;
            _logger = logger;
            _channel = channel;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start upload background service");
            await foreach (var item in _channel.ReadAllAsync(stoppingToken))
            {
                await Task.Delay(1000);
                try
                {
                    _logger.LogInformation("Uploading item");
                    var randomName = Guid.NewGuid().ToString() + ".pdf";
                    var savePath = Path.Combine(_environment.WebRootPath, "documents");
                    var target = Path.Combine(savePath, randomName);
                    await File.WriteAllBytesAsync(target, item.Data);
                    _logger.LogInformation("Uploaded item " + target);
                    _logger.LogInformation("Processing PDF files...");
                    await Task.Delay(20000);
                    _logger.LogInformation("Processed PDF file...");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An unhandled exception occurred.");
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping upload background service");
            return base.StopAsync(cancellationToken);
        }

    }
}