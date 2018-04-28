using JCTest.Interfaces;
using JCTest.Models;
using JCTest.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using Unity;

[assembly: OwinStartupAttribute(typeof(JCTest.Startup))]

namespace JCTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var ioc = UnityConfig.Container;

            // Configure the db context to use a single instance per request
            app.CreatePerOwinContext(() =>
            {
                return ioc.Resolve<ApplicationDbContext>();
            });

            // Configure the signin manager to use a single instance per request
            app.CreatePerOwinContext<UserManagerService>((o, c) =>
            {
                var u = ioc.Resolve<IUserStore<ApplicationUser>>();

                var m = new UserManagerService(
                    u,
                    o,
                    ioc.Resolve<IIdentityMessageService>("IdentityMsgSvcEmail"),
                    ioc.Resolve<IIdentityMessageService>("IdentityMsgSvcSMS"),
                    ioc.Resolve<ISettingsService>());
                return m;
            });

            // Configure the user manager to use a single instance per request
            app.CreatePerOwinContext<SignInManagerService>((o, c) =>
            {
                var m = new SignInManagerService(
                     c.GetUserManager<UserManagerService>(),
                     c.Authentication,
                     ioc.Resolve<ISettingsService>());

                return m;
            });

            var authorizationService = ioc.Resolve<IAuthorizationService>();

            authorizationService.Setup(app);
        }
    }
}