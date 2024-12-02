using AutoMapper;
using Demo.BLL.Interface;
using Demo.BLL.Repositories;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Demo.PL.MappingProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();


            var Builder = WebApplication.CreateBuilder(args);
            #region Configuration Service that Allow Dependancy Injection 

            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<App3TierArch>(Options => Options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection")));
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Builder.Services.AddAutoMapper(m => m.AddProfiles(new List<Profile> { new EmployeeProfile(), new DepartmentProfile(), new UserProfile(), new RoleProfile() }));
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>
                (options =>
                {
                    options.Password.RequireNonAlphanumeric = true; //@#$
                    options.Password.RequireDigit = true; // 123
                    options.Password.RequireLowercase = true; // asd
                    options.Password.RequireUppercase = true; // ASD

                }).AddEntityFrameworkStores<App3TierArch>()
                      .AddDefaultTokenProviders();

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => {
                        options.LoginPath = "Account/Login";
                        options.AccessDeniedPath = "Home/Error";

                    });


            #endregion
            var app = Builder.Build();

            #region Configure Http request pipeline 

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

            #endregion
            app.Run();

        }



        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
