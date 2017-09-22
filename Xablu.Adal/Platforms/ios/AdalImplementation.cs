using Plugin.Xablu.Adal.Abstractions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.Xablu.Adal
{
    public class AdalImplementation : BaseAdal
    {
        protected override async Task<IPlatformParameters> GetPlatformParams()
        {
            var controller = await Task.Run(() =>
            {
                UIViewController rootController = null;
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                });
                return rootController;
            });

            return new PlatformParameters(controller);
        }
    }
}