using Rider.RemoteSimulator.Win.Core;

namespace Rider.RemoteSimulator.Win.Helpers
{
    public static class RemoteSimulatorHelper
    {
        public static void OpenSimulator(string uid)
        {
            string strCmdText = $"--device={uid} -h={Config.MacHost.Host} -ssh={Config.MacHost.User}";

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    UseShellExecute        = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow         = true,

                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName    = Config.RemoteSimulatorPath,
                    Arguments   = strCmdText
                }
            };
            process.Start();

#if DEBUG
            //When debugging in IDE we need this to keep the simulator open
            process.WaitForExit();
#endif

        }
    }
}
