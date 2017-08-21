using System;
using Plugin.Xablu.Adal;
using Plugin.Xablu.Adal.Abstractions;

namespace Sample
{
    public static class MyClass
    {
        public static void SetLogin()
        {
            CrossAdal.Current.Persistence = new LoginPersistence();
            CrossAdal.Current.Configure(new AdalConfiguration()
            {
                ResourceId = "https://portal-dev.bdo.global/7f3d35f1-d478-4cc6-a4ed-b6b398516f7b",
                Authority = "https://login.microsoftonline.com/81f78a73-800c-41f1-b368-a024711b2836",
                ClientId = "f96f18ea-47e8-4f08-b1c5-f9cab8bac1ae",
                RedirectUri = new Uri("https://gpa-app-dev.bdo.global")
            });
        }
    }
}
