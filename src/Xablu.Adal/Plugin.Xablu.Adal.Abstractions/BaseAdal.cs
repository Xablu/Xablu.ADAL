using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Plugin.Xablu.Adal.Abstractions.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Xablu.Adal.Abstractions
{
    public abstract class BaseAdal : IAdal
    {
        public IAdalPersistence Persistence { get; set; }

        private AdalConfiguration configuration;
        private ActiveDirectoryUser loggedInUser;

        private SemaphoreSlim loginSemaphore = new SemaphoreSlim(1);
        private SemaphoreSlim loginFlowFinishedSemaphore = new SemaphoreSlim(0);

        public BaseAdal()
        {
            Persistence = new InMemoryAdalPersistence();
        }

        public void Configure(AdalConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ActiveDirectoryUser> EnsureUserLoggedIn()
        {
            return loggedInUser ?? await GetOrSignInUserAsync(allowUserInteraction: true);
        }

        public async Task<ActiveDirectoryUser> GetLoggedInUserAsync(bool forceRefresh = false)
        {
            if (loggedInUser == null || forceRefresh)
                loggedInUser = await GetOrSignInUserAsync(allowUserInteraction: false);

            return loggedInUser;
        }

        public async Task WaitForLogin()
        {
            if (loggedInUser != null) return;
            await loginFlowFinishedSemaphore.WaitAsync();
        }

        public async Task Logout()
        {
            await Persistence.RemoveUserId();
            await Persistence.RemoveTokenCache();
            loggedInUser = null;
            loginFlowFinishedSemaphore = new SemaphoreSlim(0);
        }

        private async Task<ActiveDirectoryUser> GetOrSignInUserAsync(bool allowUserInteraction = true)
        {
            if (configuration == null) throw new Exception($"No {nameof(AdalConfiguration)} provided.");

            var userId = await Persistence.RetrieveUserId();
            var tokenCacheBytes = await Persistence.RetrieveTokenCache();
            var tokenCache = new TokenCache();
            if (tokenCacheBytes != null && tokenCacheBytes?.Length > 0)
            {
                try
                {
                    tokenCache = new TokenCache(tokenCacheBytes);
                }
                catch (Exception)
                {
                    throw new Exception("Invalid token cache");
                }
            }

            AuthenticationResult authResult = null;

            var authContext = new AuthenticationContext(configuration.Authority, tokenCache);
            var platformParams = await GetPlatformParams();

            if (userId != null)
            {
                try
                {
                    var userIdentifier = new UserIdentifier(userId, UserIdentifierType.UniqueId);
                    authResult = await authContext.AcquireTokenSilentAsync(configuration.ResourceId, configuration.ClientId, userIdentifier);
                }
                catch (Exception)
                {
                    authResult = null;
                }
            }

            try
            {
                await loginSemaphore.WaitAsync();

                if (authResult == null)
                {
                    if (!allowUserInteraction) return null;

                    try
                    {
                        authResult = await authContext.AcquireTokenAsync(configuration.ResourceId, configuration.ClientId, configuration.RedirectUri, platformParams).ConfigureAwait(false);
                    }
                    catch (AdalServiceException e)
                    {
                        switch (e.ErrorCode)
                        {
                            case AdalError.AuthenticationCanceled:
                                throw new LoginCanceledException(e);
                            default:
                                throw new LoginFailedException(e);
                        }
                    }
                    if (authResult != null)
                    {
                        //TODO: authContext.TokenCache.Serialize() throws exception in release build
                        //see: http://stackoverflow.com/questions/34641841/crash-in-release-build-when-using-datacontractjsonserializer-in-xamarin-android
                        //and: https://bugzilla.xamarin.com/show_bug.cgi?id=37491
                        // temporary fix: include following line in droid csproj for release build only:
                        // <AndroidLinkSkip>System.Runtime.Serialization</AndroidLinkSkip>
                        //
                        // final fix will be available in next major Xamarin release (in alpha now)
                        await Persistence.PersistTokenCache(authContext.TokenCache.Serialize());
                    }
                }
            }
            finally
            {
                loginFlowFinishedSemaphore.Release();
                loginSemaphore.Release();
            }

            if (authResult == null) return null;

            await Persistence.PersistUserId(authResult.UserInfo.UniqueId);

            loggedInUser = new ActiveDirectoryUser
            {
                UserId = new Guid(authResult.UserInfo.UniqueId),
                AccessTokenType = authResult.AccessTokenType,
                AccessToken = authResult.AccessToken,
                UserInfo = authResult.UserInfo
            };

            return loggedInUser;
        }

        protected abstract Task<IPlatformParameters> GetPlatformParams();
    }
}
