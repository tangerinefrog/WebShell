using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShell.Helpers;
using WebShell.Models;

namespace WebShell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommandPrompt _cmd;
        private readonly ICommandContext _db;

        public HomeController(ICommandContext ctx, ICommandPrompt cmd)
        {
            _db = ctx;
            _cmd = cmd;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("path", Environment.CurrentDirectory);

            return View();
        }

        public IEnumerable<string> Output(string command)
        {
            if (command == null)
                return null;

            _db.SaveCommand(command);

            var commSplitted = command
                .Split(' ')
                .Select(item => item.ToLower())
                .ToArray();

            var comm = commSplitted[0];
            var args = commSplitted[1..];


            if (args.Length > 0 && (comm == "cd" || comm == "chdir"))
            {
                var currentPath = HttpContext.Session.GetString("path");
                
                HttpContext.Session.SetString("path", 
                    PathHelper.ProcessPath(args[0], currentPath));
            }

            var path = HttpContext.Session.GetString("path") ?? Environment.CurrentDirectory;

            if (comm == "histclr")
            {
                _db.ClearHistory();
                return new[] { "Cleared!", path };
            }

            var output = ResponseBeautifier(_cmd.Run(command, path));
            
            return new[]{ output, path };
        }

        public async Task<IEnumerable<Command>> History()
        {
            return await _db.Commands.ToArrayAsync();
        }
        
        private string ResponseBeautifier(string response)
        {
            return response.Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\n", "<br>")
                .Replace("\r", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }
    }
}
