using System.Configuration;

namespace RemoteSimulator.Core
{
    public static class Config
    {
        public static class MacHost
        {
            public static string Host     { get; } = ConfigurationManager.AppSettings["MacHost"];
            public static int    Port     { get; } = ConfigurationManager.AppSettings["MacHostPort"].ToInt(22);
            public static string User     { get; } = ConfigurationManager.AppSettings["MacHostUser"];
            public static string Password { get; } = ConfigurationManager.AppSettings["MacHostPassword"];
        }

        public static string RemoteSimulatorPath { get; } = ConfigurationManager.AppSettings["RemoteSimulatorPath"];
    }
}
