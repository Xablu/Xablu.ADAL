using Android.App;
using Android.Content;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Plugin.Xablu.Adal.Abstractions;

namespace Plugin.Xablu.Adal
{
    public static class AndroidAdalExtensions
    {
        public static void OnActivityResult(this IAdal adal, int requestCode, Result resultCode, Intent data)
        {
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }
    }
}