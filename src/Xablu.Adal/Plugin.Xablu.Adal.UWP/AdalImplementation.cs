using Plugin.Xablu.Adal.Abstractions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace Plugin.Xablu.Adal
{
    public class AdalImplementation : BaseAdal
    {
        protected override Task<IPlatformParameters> GetPlatformParams()
        {
            return Task.FromResult<IPlatformParameters>(new PlatformParameters(PromptBehavior.Auto, false));
        }
    }
}