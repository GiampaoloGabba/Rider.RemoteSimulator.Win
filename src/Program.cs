using System.IO;
using NLog;
using RemoteSimulator.Helpers;

namespace RemoteSimulator
{
    static class Program
    {
        public static readonly Logger MainLogger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                MainLogger.Warn("No configuration path specified, Unable to proceed");
                MainLogger.Warn("Make sure to pass the $ModuleFileDir$ argument when configuring the external tool in Rider!");
                return;
            }

            var baseFolder = args[0];
            var workspace  = Path.Combine(baseFolder, ".idea", "workspace.xml");

            MainLogger.Info("Getting the workspace file: " + workspace);
            if (!File.Exists(workspace))
            {
                MainLogger.Error("File not found, unable to proceed");
                return;
            }

            MainLogger.Info("Reading simulator name from workspace file");
            var simulatore = WorkSpaceHelper.GetSimulatorName(workspace);
            if (string.IsNullOrEmpty(simulatore))
            {
                MainLogger.Error("Unable to find a suitable simulator");
                return;
            }

            MainLogger.Info("Simulator selected: " + simulatore);
            MacHostHelper.MacHostJob(simulatore);
        }
    }
}
