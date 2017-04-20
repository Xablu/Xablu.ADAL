using Xablu.ADAL.Tester.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace Xablu.ADAL.Tester.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<FirstViewModel>();
        }
    }

}
