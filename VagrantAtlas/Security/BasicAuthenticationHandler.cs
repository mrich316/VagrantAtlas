using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;

namespace VagrantAtlas.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string header = Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(header))
            {
                return Task.FromResult(AuthenticateResult.Skip());
            }

            if (header.StartsWith(Options.AuthenticationScheme + " "))
            {
                var actualToken = header.Substring(Options.AuthenticationScheme.Length).Trim();
                var expectedToken = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(Options.Username + ":" + Options.Password));

                if (actualToken == expectedToken)
                {
                    var identity = new ClaimsIdentity(Options.AuthenticationScheme, "sub", "role");
                    identity.AddClaim(new Claim("sub", Options.Username));

                    var ticket = new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties(),
                        Options.AuthenticationScheme);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Invalid Credentials"));
            }

            return Task.FromResult(AuthenticateResult.Skip());
        }
    }
}
