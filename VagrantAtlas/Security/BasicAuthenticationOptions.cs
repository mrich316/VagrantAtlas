using Microsoft.AspNetCore.Builder;

namespace VagrantAtlas.Security
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationOptions()
        {
            AuthenticationScheme = "Basic";
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
