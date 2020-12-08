using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AkulaDisk.SignalR;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
            string connection;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                connection = Configuration.GetConnectionString("AzureConnection");
                services.AddSignalR()
                .AddAzureSignalR(Configuration.GetValue<string>("SignalRSercet"));
            }
            else
            {
                services.AddSignalR();
                connection = Configuration.GetConnectionString("DefaultConnection");
                services.AddSwaggerGen(options => {
                    options.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "WebApp", Version = "v1" });
                });
            }
           // services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("AkulaDisk")));
            //   services.AddDbContext<FileContext>(options =>
            //     options.UseSqlServer(connection, b => b.MigrationsAssembly("AkulaDisk")));
            services.AddDefaultIdentity<ApplicationUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            // options.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddMvc();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFileProcessor, FileProcessor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ISharedFolderRepository, SharedRepository>();
            //services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetValue<string>("ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("ClientSecret");
                });

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
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
               // app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
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
                endpoints.MapHub<RequestHub>("/chat");
            });
          



        }
    }
}
