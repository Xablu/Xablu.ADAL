using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace Xablu.ADAL.Droid
{
    internal class ActiveDirectoryHelper : BaseActiveDirectoryHelper
    {
        public ActiveDirectoryHelper(ActiveDirectoryConfiguration configuration) : base(configuration) { }

        protected override Task<IPlatformParameters> GetPlatformParams()
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            return Task.FromResult<IPlatformParameters>(new PlatformParameters(activity));
        }
    }

}