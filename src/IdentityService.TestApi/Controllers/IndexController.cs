using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using IdentityService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IdentityService.TestApi.Controllers
{
    public class IndexController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("ParseJWE")]
        [HttpPost]
        public string ParseJWE(string jwe)
        {
            var key = new X509SecurityKey(AuthenticationExtensionMethods.TokenEncryptionCertificate());
            var sigkey = new X509SecurityKey(AuthenticationExtensionMethods.TokenSigningCertificate());

            var handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(
                jwe,
                new TokenValidationParameters
                {
                    //ValidAudience = "abc123",
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    ValidIssuer = "https://localhost:44365/",
                    RequireSignedTokens = true,
                    TokenDecryptionKey = key,
                    IssuerSigningKey = sigkey,
                    ValidateAudience = false
                },
                out SecurityToken securityToken);
            var result = new
            {
                Principal= new
                {
                    Name = claimsPrincipal.Identity.Name,
                    AuthType = claimsPrincipal.Identity.AuthenticationType
                },
                Token=securityToken
            };
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}