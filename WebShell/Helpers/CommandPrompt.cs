﻿using System.Diagnostics;
using System.IO;

namespace WebShell.Helpers
{
    public class CommandPrompt : ICommandPrompt
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
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
        }

        public string Run(string command, string path)
        {
            _cmd.StartInfo.Arguments = $"/c {command}";
            _cmd.StartInfo.WorkingDirectory = Path.GetFullPath(path);

            _cmd.Start();

            return _cmd.StandardOutput.ReadToEnd() + _cmd.StandardError.ReadToEnd();
        }
    }
}
