using Owin;

namespace JCTest.Interfaces
{
    public interface IAuthorizationService
    {
        bool IsFacebookAuthEnabled { get; }
        bool IsGoogleAuthEnabled { get; }
        bool IsMicrosoftAuthEnabled { get; }
        bool IsTwitterAuthEnabled { get; }

        void Setup(IAppBuilder app);
    }
}