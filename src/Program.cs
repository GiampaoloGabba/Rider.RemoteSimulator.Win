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
                MainLogger.Warn("E' necessario specificare la path della solution per proseguire");
                return;
            }

            var baseFolder = args[0];
            var workspace  = Path.Combine(baseFolder, ".idea", "workspace.xml");

            MainLogger.Info("Recupero file workspace: " + workspace);
            if (!File.Exists(workspace))
            {
                MainLogger.Error("File non trovato, impossibile continuare");
                return;
            }

            MainLogger.Info("Recupero nome simulator dal workspace");
            var simulatore = WorkSpaceHelper.GetSimulatorName(workspace);
            if (string.IsNullOrEmpty(simulatore))
            {
                MainLogger.Error($"Impossibile recuperare il simulatore selezionato");
                return;
            }

            MainLogger.Info("Simulatore selezionato: " + simulatore);
            MacHostHelper.MacHostJob(simulatore);
        }
    }
}
