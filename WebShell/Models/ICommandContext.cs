using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShell.Models
{
    public interface ICommandContext
    {
        DbSet<Command> Commands { get; set; }
        void SaveCommand(string command);
        void ClearHistory();
    }
}
