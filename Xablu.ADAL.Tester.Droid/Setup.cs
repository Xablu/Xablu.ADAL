using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using System;
using Acr.UserDialogs;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace Xablu.ADAL.Tester.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializePlatformServices()
        {
            UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);

            base.InitializePlatformServices();
        }

        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(Xablu.ADAL.Droid.Plugin))
            {
                return new ActiveDirectoryConfiguration()
                {
                    ResourceId = Core.Settings.ActiveDirectoryResourceId,
                    Authority = Core.Settings.ActiveDirectoryAuthority,
                    ClientId = Core.Settings.ActiveDirectoryClientId,
                    RedirectUri = Core.Settings.ActiveDirectoryRedirectUri
                };
            }

            return null;
        }
    }
}
