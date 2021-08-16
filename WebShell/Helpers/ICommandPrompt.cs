using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShell.Helpers
{
    public interface ICommandPrompt
    {
        public string Run(string command, string path);
    }
}