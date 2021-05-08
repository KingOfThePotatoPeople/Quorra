using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace Quorra.Utilities
{
    public static class NLogConfig
    {
        
        private static readonly string EnvStr = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static LoggingConfiguration GetConfig()
        {

            // Internal / debugging
            InternalLogger.LogLevel = LogLevel.Trace;
            InternalLogger.LogFile = "C:\\Users\\jason\\Desktop\\log.txt";

            // Get appsettings
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{EnvStr}.json");

            // Get configuration
            IConfiguration envConfig = builder.Build();

            // NLog: Programmatically
            LoggingConfiguration config = new();

            // Targets
            DatabaseTarget dbTarget = new("targetDb")
            {
                Name = "database",
                CommandText =
                    $"INSERT INTO dbo.EventLog (Application, Logged, Level, Message, UserName, ServerName, Port, Url, Https, ServerAddress, RemoteAddress, Logger, Callsite, Exception)" +
                    "Values (@Application, @Logged, @Level, @Message, @UserName, @ServerName, @Port, @Url, @Https, @ServerAddress, @RemoteAddress, @Logger, @Callsite, @Exception)",
                Parameters =
                {
                    new DatabaseParameterInfo {Name = "@Application", Layout = "Quorra"},
                    new DatabaseParameterInfo {Name = "@Logged", Layout = "${date}"},
                    new DatabaseParameterInfo {Name = "@Level", Layout = "${level}"},
                    new DatabaseParameterInfo {Name = "@Message", Layout = "${message}"},
                    new DatabaseParameterInfo {Name = "@Username", Layout = "${identity}"},
                    new DatabaseParameterInfo {Name = "@ServerName", Layout = "${aspnet-request:serverVariable=SERVER_NAME}"},
                    new DatabaseParameterInfo {Name = "@Port", Layout = "${aspnet-request:serverVariable=SERVER_PORT}"},
                    new DatabaseParameterInfo {Name = "@Url", Layout = "${aspnet-request:serverVariable=HTTP_URL}"},
                    new DatabaseParameterInfo {Name = "@Https", Layout = "${when:inner=1:when='${aspnet-request:serverVariable=HTTPS}' == 'on'}${when:inner=0:when='${aspnet-request:serverVariable=HTTPS}' != 'on'}"},
                    new DatabaseParameterInfo {Name = "@ServerAddress", Layout = "${aspnet-request:serverVariable=LOCAL_ADDR}"},
                    new DatabaseParameterInfo {Name = "@RemoteAddress", Layout = "${aspnet-request:serverVariable=REMOTE_ADDR}:${aspnet-request:serverVariable=REMOTE_PORT}"},
                    new DatabaseParameterInfo {Name = "@Logger", Layout = "${logger}"},
                    new DatabaseParameterInfo {Name = "@Callsite", Layout = "${callsite}"},
                    new DatabaseParameterInfo {Name = "@Exception", Layout = "${exception:tostring}"}
                }
            };
            config.AddTarget(dbTarget);

            // Logging intensity / connection string based on environment.
            switch (EnvStr)
            {
                case "Development":
                    dbTarget.ConnectionString = envConfig.GetConnectionString("DefaultConnection");
                    config.AddRule(LogLevel.Info, LogLevel.Fatal, dbTarget);
                    break;
                case "Staging":
                    dbTarget.ConnectionString = envConfig.GetConnectionString("DefaultConnection");
                    config.AddRule(LogLevel.Warn, LogLevel.Fatal, dbTarget);
                    break;
                case "Production":
                    dbTarget.ConnectionString = envConfig.GetConnectionString("DefaultConnection");
                    config.AddRule(LogLevel.Warn, LogLevel.Fatal, dbTarget);
                    break;
                default:
                    dbTarget.ConnectionString = envConfig.GetConnectionString("DefaultConnection");
                    config.AddRuleForAllLevels(dbTarget);
                    break;
            }

            return config;

        }
        
    }
}