using System;
using Plugin.Xablu.Adal;
using Plugin.Xablu.Adal.Abstractions;

namespace Sample.Core
{
    public static class AdalTest
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