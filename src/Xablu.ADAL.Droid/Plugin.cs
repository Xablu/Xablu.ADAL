using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Xablu.ADAL.Droid
{
    public class Plugin : IMvxConfigurablePlugin
    {
        private ActiveDirectoryConfiguration _configuration;

        public void Configure(IMvxPluginConfiguration configuration)
        {
            _configuration = configuration as ActiveDirectoryConfiguration;
        }

        public void Load()
        {
            Mvx.RegisterSingleton<IActiveDirectoryHelper>(new ActiveDirectoryHelper(_configuration));
        }
    }

}