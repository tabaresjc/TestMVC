using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MySql;
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
using System.Linq;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(JCTest.Startup))]

namespace JCTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var ioc = UnityConfig.Container;

            // Setup Hangfire
            GlobalConfiguration
                .Configuration
                .UseStorage(new MySqlStorage("DefaultHangfireConnection"));

            GlobalConfiguration
                .Configuration
                .UseActivator(new ContainerJobActivator(UnityConfig.ContainerHangfire));

            var options = new DashboardOptions
            {
                AuthorizationFilters = new List<IAuthorizationFilter>
                {
                    new AuthorizationFilter { Roles = "Admin" },
                    new ClaimsBasedAuthorizationFilter("name", "value")
                }
            };

            app.UseHangfireDashboard("/hangfire", options);
            app.UseHangfireServer();

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

            // Use hangfire to fetch data periodically
            RecurringJob.AddOrUpdate(
                "MovieListUpdateService", 
                () => RunMovieListUpdateService(new Hangfire.JobCancellationToken(false)),
                Cron.Daily
            );
        }

        public void RunMovieListUpdateService(Hangfire.IJobCancellationToken cancellationToken)
        {
            var ioc = UnityConfig.ContainerHangfire;
            var ms = ioc.Resolve<IMovieListUpdateService>();

            ms.FetchAndSave(DateTime.UtcNow.Year, cancellationToken);
        }
    }
}