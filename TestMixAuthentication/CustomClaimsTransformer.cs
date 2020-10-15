using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestIdentityServerAuthentication
{
    public class CustomClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //if (!principal.HasClaim(c => c.Type == ClaimTypes.Country))
            //{
            //    ClaimsIdentity id = new ClaimsIdentity();
            //    id.AddClaim(new Claim(ClaimTypes.Country, "Canada"));
            //    principal.AddIdentity(id);
            //}
            //return Task.FromResult(principal);

            // This will run every time Authenticate is called so its better to create a new Principal
            var transformed = new ClaimsPrincipal();
            transformed.AddIdentities(principal.Identities);
            if(transformed.HasClaim(c=> c.Type == ClaimTypes.Role))
            {
                transformed.AddIdentity(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "admin")
                }));
            }
            return Task.FromResult(transformed);
        }
    }
}
