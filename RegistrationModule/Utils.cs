using System.Diagnostics;

namespace RegistrationModul
{
    public static class Utils
    {
        public static string GetUUID()
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c " + "wmic csproduct get UUID")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var proc = new Process() { StartInfo = procStartInfo };
            proc.Start();

            return proc.StandardOutput.ReadToEnd().Replace("UUID", string.Empty).Trim().ToUpper();
        }
    }
}
