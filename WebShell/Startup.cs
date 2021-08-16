using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebShell.Helpers;
using WebShell.Models;

namespace WebShell
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CommandContext>(opt =>
                opt.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddTransient<ICommandContext, CommandContext>();
            services.AddTransient<ICommandPrompt, CommandPrompt>();

            services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(builder => 
                builder.MapRoute("default","{Controller=Home}/{Action=Index}"));
        }
    }
}
