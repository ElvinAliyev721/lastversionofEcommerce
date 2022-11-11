using Code.DAL;
using Code.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code
{
    public class Startup
    {
        public IConfiguration _configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(cfg => {
                cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<AppDbContext>(optios =>
            {
                optios.UseSqlServer(_configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<AppUser, IdentityRole>(IdentityOptions=> {
                IdentityOptions.Password.RequireDigit = true;
                IdentityOptions.Password.RequiredLength = 8;
                IdentityOptions.Password.RequireLowercase = true;
                IdentityOptions.Password.RequireUppercase = true;
                IdentityOptions.Password.RequireNonAlphanumeric = true;

                IdentityOptions.User.RequireUniqueEmail = true;

                IdentityOptions.Lockout.AllowedForNewUsers = true;
                IdentityOptions.Lockout.MaxFailedAccessAttempts = 4;
                IdentityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
