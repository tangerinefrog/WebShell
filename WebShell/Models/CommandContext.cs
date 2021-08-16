using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShell.Models
{
    public class CommandContext : DbContext, ICommandContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Command> Commands { get; set; }
        
        public void SaveCommand(string command)
        {
            Add(new Command() { Text = command });
            SaveChanges();
        }

        public void ClearHistory()
        {
            Commands.RemoveRange(Commands);
            SaveChanges();
        }
    }
}
