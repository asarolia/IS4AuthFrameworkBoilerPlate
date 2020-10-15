using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;

//namespace TestIdentityServerAuthentication.Quickstart.Account
namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterViewModel: RegisterInputModel
    {
        

        public bool EnableLocalLogin { get; set; } = true;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}
