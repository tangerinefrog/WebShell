using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebShell.Helpers;
using WebShell.Models;

namespace WebShell.Controllers
{
    public class HomeController : Controller
    {
        private readonly CommandPrompt _cmd;

        public HomeController()
        {
            _cmd = new CommandPrompt();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<string> Output(string command)
        {
            if (command == null)
                return null;

            var commSplitted = command.Split(' ');
            
            if (commSplitted.Length > 1 && commSplitted[0] == "cd")
            {
                HttpContext.Session.SetString("path", commSplitted[1]);
            }

            var path = HttpContext.Session.GetString("path") ?? Environment.CurrentDirectory;
            var output = ResponseBeautifier(_cmd.Run(command, path));
            
            return new[]{ output, Path.GetFullPath(path) };
        }

        private string ResponseBeautifier(string response)
        {
            return response.Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\n", "<br>");
        }
    }
}
