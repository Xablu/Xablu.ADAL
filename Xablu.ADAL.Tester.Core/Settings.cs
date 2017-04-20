using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xablu.ADAL.Tester.Core
{
    public class Settings
    {
        //authentication context to connect to for retrieving a token: $"{aadInstance}/{tenant}"

        private static readonly string aadInstance = "https://login.microsoftonline.com";
        private static readonly string tenant = "mytenant.onmicrosoft.com";
        public static readonly string ActiveDirectoryAuthority = $"{aadInstance}/{tenant}";


        // requested resource to gain access to:
        public static readonly string ActiveDirectoryResourceId = "https://myresource.myorg.com/";

        // client that is asking for access:
        public static readonly string ActiveDirectoryClientId = "7226D655-D555-44C9-A22F-07F7C6C31F9C";

        // uri where the user is redirected after successful login
        public static readonly Uri ActiveDirectoryRedirectUri = new Uri("https://myapp.myorg.com");
    }
}
