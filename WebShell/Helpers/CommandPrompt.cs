using System.Diagnostics;
using System.IO;

namespace WebShell.Helpers
{
    public class CommandPrompt
    {
        private readonly Process _cmd;

        public CommandPrompt()
        {
            _cmd = new Process()
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };
        }

        public string Run(string command, string path)
        {
            _cmd.StartInfo.Arguments = $"/c {command}";
            _cmd.StartInfo.WorkingDirectory = Path.GetFullPath(path);
            _cmd.Start();

            return _cmd.StandardOutput.ReadToEnd();
        }
    }
}
