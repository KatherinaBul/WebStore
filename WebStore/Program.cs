using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host => host
                    .UseStartup<Startup>()
                    .ConfigureLogging((host, log) =>
                    {
                        log.AddFilter("Microsoft", level => level > LogLevel.Warning);
                    }))
                .UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                    .WriteTo.RollingFile($@".\Log\WebStore[{DateTime.Now:yyyy-mm-ddTHH-mm-ss}].log")
                    .WriteTo.File(new JsonFormatter(",", true),
                        $@".\Log\WebStore[{DateTime.Now:yyyy-mm-ddTHH-mm-ss}].log.json")
                );
    }
}