using Plugin.Xablu.Adal.Abstractions;
using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Plugin.CurrentActivity;

namespace Plugin.Xablu.Adal
{
    public class AdalImplementation : BaseAdal
    {
        protected override Task<IPlatformParameters> GetPlatformParams()
        {
            var activity = CrossCurrentActivity.Current.Activity;
            if (activity == null) throw new Exception("No android activity set, make sure to set CrossCurrentActivity.Current.Activity in your MainApplication");
            return Task.FromResult<IPlatformParameters>(new PlatformParameters(activity));
        }
    }
}