using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory,
            string configurationFile = "log4net.config")
        {
            if (Path.IsPathRooted(configurationFile))
            {
                var assembly = Assembly.GetEntryAssembly() ??
                               throw new InvalidOperationException(
                                   "Не удалось определить сборку с точкой входа в приложение");
                var dir = Path.GetDirectoryName(assembly.Location) ??
                          throw new InvalidOperationException(
                              "Не удалось определить путь размещения сборки с точкой входа в приложение");
                configurationFile = Path.Combine(dir, configurationFile);
            }

            factory.AddProvider(new Log4NetLoggerProvider(configurationFile));

            return factory;
        }
    }
}