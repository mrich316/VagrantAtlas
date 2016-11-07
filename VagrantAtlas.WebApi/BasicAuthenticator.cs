using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VagrantAtlas.WebApi
{
    public class BasicAuthenticator
    {
        private readonly string _id;
        private readonly string _secret;
        private readonly Task<IEnumerable<Claim>> _noClaims = Task.FromResult<IEnumerable<Claim>>(null);

        public BasicAuthenticator(string id, string secret)
        {
            _id = id ?? string.Empty;
            _secret = secret ?? string.Empty;
        }

        public Task<IEnumerable<Claim>> Authenticate(string id, string secret)
        {
            if (id != _id || secret != _secret) return _noClaims;

            return Task.FromResult<IEnumerable<Claim>>(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id)
            });
        }
    }
}