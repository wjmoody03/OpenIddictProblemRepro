using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Server.Data;
using IdentityService.Server.Models;
using IdentityService.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityService.Server
{
    public class Startup
    {
        string connectionString = "Data Source=localhost,1433;Initial Catalog=IdentityServer;uid=sa;pwd=P@ssword1";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token");
                options.AllowPasswordFlow();
                options.AcceptAnonymousClients();
                options.AddEncryptionCertificate(AuthenticationExtensionMethods.TokenEncryptionCertificate());
                options.AddSigningCertificate(AuthenticationExtensionMethods.TokenSigningCertificate());
                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseOpenIddict();
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(config => config.AllowAnyOrigin());
            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(options => options.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"));

        }
    }
}
