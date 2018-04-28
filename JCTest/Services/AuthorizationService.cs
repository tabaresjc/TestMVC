using JCTest.Interfaces;
using JCTest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCTest.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsService settings;

        public AuthorizationService(ISettingsService settings)
        {
            this.settings = settings;
        }

        public bool IsMicrosoftAuthEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.settings.Get("MicrosoftClientId")) &&
                    !string.IsNullOrWhiteSpace(this.settings.Get("MicrosoftClientSecret"));
            }
        }

        public bool IsTwitterAuthEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.settings.Get("TwitterClientId")) &&
                    !string.IsNullOrWhiteSpace(this.settings.Get("TwitterClientSecret"));
            }
        }

        public bool IsFacebookAuthEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.settings.Get("FacebookClientId")) &&
                    !string.IsNullOrWhiteSpace(this.settings.Get("FacebookClientSecret"));
            }
        }

        public bool IsGoogleAuthEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.settings.Get("GoogleClientId")) &&
                    !string.IsNullOrWhiteSpace(this.settings.Get("GoogleClientSecret"));
            }
        }

        public void Setup(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManagerService, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            if (this.IsMicrosoftAuthEnabled)
            {
                app.UseMicrosoftAccountAuthentication(
                    clientId: settings.Get("MicrosoftClientId"),
                    clientSecret: settings.Get("MicrosoftClientSecret")
                );
            }

            if (this.IsTwitterAuthEnabled)
            {
                app.UseTwitterAuthentication(
                    consumerKey: this.settings.Get("TwitterClientId"),
                    consumerSecret: this.settings.Get("TwitterClientSecret")
                );
            }

            if (this.IsFacebookAuthEnabled)
            {
                app.UseFacebookAuthentication(
                   appId: this.settings.Get("FacebookClientId"),
                   appSecret: this.settings.Get("FacebookClientSecret")
                );
            }

            if (this.IsGoogleAuthEnabled)
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = this.settings.Get("GoogleClientId"),
                    ClientSecret = this.settings.Get("GoogleClientSecret")
                });
            }
        }
    }
}