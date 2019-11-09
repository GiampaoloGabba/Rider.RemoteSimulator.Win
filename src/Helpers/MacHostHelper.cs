using System;
using System.Linq;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Common;
using Rider.RemoteSimulator.Win.Core;
using Rider.RemoteSimulator.Win.Models;

namespace Rider.RemoteSimulator.Win.Helpers
{
    public static class MacHostHelper
    {
        public static void MacHostJob(string simulator)
        {
            Program.MainLogger.Info($"Starting connection to host {Config.MacHost.Host}, port {Config.MacHost.Port}, user {Config.MacHost.User}");
            var deviceId = "";

            using (var sshClient = new SshClient(Config.MacHost.Host, Config.MacHost.Port, Config.MacHost.User, Config.MacHost.Password))
            {
                sshClient.ErrorOccurred += SshClientOnErrorOccurred;
                sshClient.Connect();

                //if the selected emulator contains the iOS version in his name,
                //we need a bit of string "magic" to find the right UDID
                if (simulator.Contains(" iOS "))
                {
                    Program.MainLogger.Info("Input contains iOS version, activating advanced seatch for simulators");

                    var versionPosition = simulator.IndexOf(" iOS ", StringComparison.Ordinal);
                    var iosVersion      = simulator.Substring(versionPosition + 5).Replace(".", "-");
                    var simulatorKey    = "com.apple.CoreSimulator.SimRuntime.iOS-" + iosVersion;

                    Program.MainLogger.Info("iOS version extracted: " + iosVersion);
                    Program.MainLogger.Info("Dicitonary key generated: " + simulatorKey);

                    Program.MainLogger.Info("Reading emulator list");

                    var comando = "xcrun simctl list --json";
                    SimCtlResponseModel simCtlResponse;
                    using (var cmd = sshClient.CreateCommand(comando))
                    {
                        cmd.Execute();
                        simCtlResponse = JsonConvert.DeserializeObject<SimCtlResponseModel>(cmd.Result);
                    }

                    if (simCtlResponse?.Devices != null && simCtlResponse.Devices.ContainsKey(simulatorKey))
                    {
                        var deviceName = simulator.Substring(0, versionPosition);
                        var device = simCtlResponse.Devices[simulatorKey].FirstOrDefault(x => x.IsAvailable && x.Name == deviceName);

                        if (device != null)
                            deviceId = device.Udid;
                        else
                            Program.MainLogger.Warn($"Key found but no corresponding simulator found");
                    }
                    else
                    {
                        Program.MainLogger.Warn("Unable to find simultors in the specified dictionary key");
                    }

                }
                else
                {
                    //No iOS version = only 1 simulator = bash magic!!!
                    Program.MainLogger.Info("Input device does not contain iOS version, recovering the UUID with a single bash command");
                    var comando = "echo $( xcrun simctl list devices | grep -w '" + simulator +
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
                Program.MainLogger.Info($"Device UDID found: {deviceId}");
                Program.MainLogger.Info("Starting the Remote Simulator");
                RemoteSimulatorHelper.OpenSimulator(deviceId);
            }
            else
            {
                Program.MainLogger.Warn("Device UDID not found, unable to start the Remote Simulator");
            }
        }

        private static void SshClientOnErrorOccurred(object sender, ExceptionEventArgs e)
        {
            Program.MainLogger.Error("Unable to connect to the Mac Host", e);
        }
    }
}
