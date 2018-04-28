using JCTest.Interfaces;
using JCTest.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JCTest.Services
{
    public class SignInManagerService : SignInManager<ApplicationUser, string>
    {
        private readonly ISettingsService settings;

        public SignInManagerService(
            UserManagerService userManager,
            IAuthenticationManager authenticationManager,
            ISettingsService settings)
            : base(userManager, authenticationManager)
        {
            this.settings = settings;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((UserManagerService)UserManager);
        }
    }
}