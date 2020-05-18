using AspNet.Security.OpenIdConnect.Primitives;
using IdentityService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.TestApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                NameClaimType = OpenIdConnectConstants.Claims.Name,
                RoleClaimType = OpenIdConnectConstants.Claims.Role,
                ValidateAudience = false,
                ValidateIssuer = false,
                TokenDecryptionKey = new X509SecurityKey(AuthenticationExtensionMethods.TokenEncryptionCertificate()),
                IssuerSigningKey = new X509SecurityKey(AuthenticationExtensionMethods.TokenSigningCertificate())
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //same as "Bearer"
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });


            services.AddOpenIddict().AddValidation(
                options =>
                {
                    options.SetTokenValidationParameters(config => config = tokenValidationParameters);
                    options.UseAspNetCore();
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(options => options.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
