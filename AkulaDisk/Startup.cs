using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Implementations;
using Services.Interfaces;

namespace AkulaDisk
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddControllersWithViews();
            services.AddRazorPages();
            string connection = Configuration.GetConnectionString("AzureConnection");
           services.AddDbContext<ApplicatopnDbContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("AkulaDisk")));
         //   services.AddDbContext<FileContext>(options =>
           //     options.UseSqlServer(connection, b => b.MigrationsAssembly("AkulaDisk")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicatopnDbContext>();
            services.AddMvc();
            services.BuildServiceProvider().GetService<ApplicatopnDbContext>().Database.Migrate();
            services.AddTransient<IFileProcessor, FileProcessor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            var builder = new ContainerBuilder();
            builder.Populate(services);
           
           // builder.RegisterType<AnotherRepository>().As<IAnotherRepository>();
            this.ApplicationContainer = builder.Build();
           // return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
            app.UseStatusCodePagesWithRedirects("/Home/Error");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            //app.UseMvc();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            

        }
    }
}
