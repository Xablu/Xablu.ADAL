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
                ResourceId = "",
                Authority = "",
                ClientId = "",
                RedirectUri = new Uri("")
            });
        }
    }
}
