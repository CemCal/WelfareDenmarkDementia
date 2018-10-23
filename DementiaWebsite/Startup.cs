using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaWebsite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DementiaWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;

namespace DementiaWebsite
{
    public class Startup
    {
        readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
                 configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/appsettings.json")
                .AddJsonFile(env.ContentRootPath + "/appsettings.Development.json", true)
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")
            });

            services.AddDbContext<SignUpDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PersonDataContext"));
            });

            services.AddDbContext<IdentityDbContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("DementiaWebsiteContextConnection"),
                optionsBuilder =>
                optionsBuilder.MigrationsAssembly("DementiaWebsite"));

            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders() ;

            //this method should always be implemented with app.UseMvc()
            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, FeatureToggles features)
        {  
            app.UseFileServer();

            StaticFileOptions option = new StaticFileOptions();
            FileExtensionContentTypeProvider contentTypeProvider =
                (FileExtensionContentTypeProvider)option.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".unityweb", "application/octet-stream");
            option.ContentTypeProvider = contentTypeProvider;

            app.UseStaticFiles(option);

            if (features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            //routing method which will look for "Controllers","/public methods in those classes and what parameters they might have
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
