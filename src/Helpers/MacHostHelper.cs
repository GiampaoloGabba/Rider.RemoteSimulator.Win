using System;
using System.Linq;
using Newtonsoft.Json;
using RemoteSimulator.Core;
using RemoteSimulator.Models;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace RemoteSimulator.Helpers
{
    public static class MacHostHelper
    {
        public static void MacHostJob(string simulatore)
        {
            Program.MainLogger.Info(
                $"Inizio connessione a host {Config.MacHost.Host}, porta {Config.MacHost.Port}, utente {Config.MacHost.User}");
            var deviceId = "";
            using (var sshClient = new SshClient(Config.MacHost.Host, Config.MacHost.Port, Config.MacHost.User,
                Config.MacHost.Password))
            {
                sshClient.ErrorOccurred += SshClientOnErrorOccurred;
                sshClient.Connect();

                //recupero il blocco che contiene il mio simulatore
                if (simulatore.Contains(" iOS "))
                {
                    Program.MainLogger.Info(
                        "Passata versione iOS, attivo modalità di ricerca avanzata");

                    var versionPosition = simulatore.IndexOf(" iOS ", StringComparison.Ordinal);
                    var iosVersion      = simulatore.Substring(versionPosition + 5).Replace(".", "-");
                    var simulatorKey    = "com.apple.CoreSimulator.SimRuntime.iOS-" + iosVersion;

                    Program.MainLogger.Info("Versione iOS estratta: " + iosVersion);
                    Program.MainLogger.Info("Chiave dizionario generata: " + simulatorKey);

                    Program.MainLogger.Info("Recupero la lista dei simulatori");

                    var comando = "xcrun simctl list --json";
                    SimCtlResponseModel simCtlResponse;
                    using (var cmd = sshClient.CreateCommand(comando))
                    {
                        cmd.Execute();
                        simCtlResponse = JsonConvert.DeserializeObject<SimCtlResponseModel>(cmd.Result);
                    }

                    if (simCtlResponse?.Devices != null && simCtlResponse.Devices.ContainsKey(simulatorKey))
                    {
                        var deviceName = simulatore.Substring(0, versionPosition);
                        var device = simCtlResponse.Devices[simulatorKey]
                            .FirstOrDefault(x => x.IsAvailable && x.Name == deviceName);

                        if (device != null)
                            deviceId = device.Udid;
                        else
                            Program.MainLogger.Warn($"La chiave è stata trovata ma non esistono simulatori corrispondenti");
                    }
                    else
                    {
                        Program.MainLogger.Warn("Impossibile trovare simulatori per la chiave specificata");
                    }

                }
                else
                {
                    Program.MainLogger.Info("Il device passato NON contiene la versione di iOS, recupero UUDID con un singolo comando bash");
                    var comando = "echo $( xcrun simctl list devices | grep -w '" + simulatore +
                                  " ' | grep -v -e 'unavailable' | awk 'match($0, /\\(([-0-9A-F]+)\\)/) { print substr( $0, RSTART + 1, RLENGTH - 2 )}' )";
                    using (var cmd = sshClient.CreateCommand(comando))
                    {
                        cmd.Execute();
                        deviceId = cmd.Result;
                    }
                }

                sshClient.Disconnect();
                sshClient.ErrorOccurred -= SshClientOnErrorOccurred;
            }

            if (!string.IsNullOrEmpty(deviceId))
            {
                Program.MainLogger.Info($"Trovato Device ID: {deviceId}");
                Program.MainLogger.Info("Avvio Remote Simulator");
                RemoteSimumlatorHelper.OpenSimulator(deviceId);
            }
            else
            {
                Program.MainLogger.Warn("Device ID non trovato, impossibile avviare il Remote Simulator");
            }
        }

        private static void SshClientOnErrorOccurred(object sender, ExceptionEventArgs e)
        {
            Program.MainLogger.Error("Errore di connessione all'host mac", e);
        }
    }
}
