using Akavache;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Xablu.ADAL.Exceptions;

namespace Xablu.ADAL
{
    internal abstract class BaseActiveDirectoryHelper : IActiveDirectoryHelper
    {
        private ActiveDirectoryConfiguration configuration { get; }

        private ActiveDirectoryUser currentUser;

        private const string tokenCacheKey = "tokenCache";
        private const string loggedInUserCacheKey = "loggedInUser";

        private IBlobCache cache = BlobCache.Secure;
        private SemaphoreSlim loginSemaphore = new SemaphoreSlim(1);
        private SemaphoreSlim loginFlowFinishedSemaphore = new SemaphoreSlim(0);
        private AuthenticationResult authResult = null;

        public BaseActiveDirectoryHelper(ActiveDirectoryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private async Task<ActiveDirectoryUser> GetOrSignInUserAsync(bool silent = false)
        {
            if (configuration == null) throw new Exception($"No {nameof(ActiveDirectoryConfiguration)} provided for Xablu.ADAL. Please provide a valid IMvxPluginConfiguration in Setup.cs");

            if (!CrossConnectivity.Current.IsConnected)
            {
                throw new ConnectivityException("No active internet connection");
            }

            byte[] tokenCacheBytes = null;
            string loggedInUserId = null;

            try
            {
                tokenCacheBytes = await cache.Get(tokenCacheKey).ToTask().ConfigureAwait(false);
                loggedInUserId = await cache.GetObject<string>(loggedInUserCacheKey).ToTask().ConfigureAwait(false);
            }
            catch (KeyNotFoundException)
            {
            }

            TokenCache tokenCache = new TokenCache();
            if (tokenCacheBytes != null)
            {
                try
                {
                    tokenCache = new TokenCache(tokenCacheBytes);
                }
                catch (Exception)
                {
                    tokenCache = new TokenCache();
                    await ClearCache();
                }
            }

            var authContext = new AuthenticationContext(configuration.Authority, tokenCache);
            var platformParams = await GetPlatformParams();

            if (loggedInUserId != null)
            {
                try
                {
                    authResult = await authContext.AcquireTokenSilentAsync(configuration.ResourceId, configuration.ClientId, new UserIdentifier(loggedInUserId, UserIdentifierType.UniqueId));
                }
                catch (Exception)
                {
                    await ClearCache();
                    loggedInUserId = null;
                    authResult = null;
                }
            }

            try
            {
                await loginSemaphore.WaitAsync();

                if (authResult == null)
                {
                    if (silent) return null;

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
                        // temporary fix: included following line in droid csproj for release build only:
                        // <AndroidLinkSkip>System.Runtime.Serialization</AndroidLinkSkip>
                        //
                        // final fix will be available in next major Xamarin release (in alpha now)
                        await cache.Insert(tokenCacheKey, authContext.TokenCache.Serialize()).ToTask().ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                loginFlowFinishedSemaphore.Release();
                loginSemaphore.Release();
            }

            if (authResult == null) return null;

            await cache.InsertObject(loggedInUserCacheKey, authResult.UserInfo.UniqueId).ToTask().ConfigureAwait(false);

            currentUser = new ActiveDirectoryUser
            {
                UserId = new Guid(authResult.UserInfo.UniqueId),
                AccessTokenType = authResult.AccessTokenType,
                AccessToken = authResult.AccessToken,
                UserInfo = authResult.UserInfo
            };

            return currentUser;
        }

        private async Task ClearCache()
        {
            await cache.Invalidate(tokenCacheKey).ToTask().ConfigureAwait(false);
            await cache.InvalidateObject<string>(loggedInUserCacheKey).ToTask().ConfigureAwait(false);
        }

        public async Task<ActiveDirectoryUser> GetLoggedInUserAsync()
        {
            if (currentUser == null) await GetOrSignInUserAsync(silent: true);
            return currentUser;
        }

        public async Task<ActiveDirectoryUser> EnsureUserLoggedIn()
        {
            return currentUser ?? await GetOrSignInUserAsync(silent: false);
        }

        protected abstract Task<IPlatformParameters> GetPlatformParams();

        public async Task WaitForLogin()
        {
            if (currentUser != null) return;
            await loginFlowFinishedSemaphore.WaitAsync();
        }
    }
}

