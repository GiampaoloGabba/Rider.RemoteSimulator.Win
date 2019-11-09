using RemoteSimulator.Core;

namespace RemoteSimulator.Helpers
{
    public static class RemoteSimumlatorHelper
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
            process.WaitForExit();
#endif

        }
    }
}
