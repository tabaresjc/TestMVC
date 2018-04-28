using JCTest.Interfaces;
using JCTest.Models;
using JCTest.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace JCTest
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // Services
            container.RegisterType<ISettingsService>(
                new PerRequestLifetimeManager(),
                new InjectionFactory((c, t, s) => 
                {
                    return new SettingsService(System.Configuration.ConfigurationManager.AppSettings);
                })
            );

            container.RegisterType<IIdentityMessageService, EmailService>("IdentityMsgSvcEmail");
            container.RegisterType<IIdentityMessageService, SmsService>("IdentityMsgSvcSMS");

            container.RegisterType<ApplicationDbContext>(
                new PerRequestLifetimeManager(),
                new InjectionFactory((c) =>
                {
                    return new ApplicationDbContext();
                })
            );

            container.RegisterType<IUserStore<ApplicationUser>>(
                new PerRequestLifetimeManager(),
                new InjectionFactory((c) =>
                {
                    var ac = c.Resolve<ApplicationDbContext>();
                    return new UserStore<ApplicationUser>(ac);
                })
            );

            container.RegisterType<UserManagerService>(
                new PerRequestLifetimeManager(),
                new InjectionFactory((c) =>
                {
                    return HttpContext
                        .Current
                        .GetOwinContext()
                        .Get<UserManagerService>();
                })
            );

            container.RegisterType<SignInManagerService>(
                new PerRequestLifetimeManager(),
                new InjectionFactory((c) =>
                {
                    return HttpContext
                        .Current
                        .GetOwinContext()
                        .Get<SignInManagerService>();
                })
            );

            container.RegisterType<IAuthorizationService, AuthorizationService>(
                new PerRequestLifetimeManager());
        }
    }
}