using Plugin.Xablu.Adal.Abstractions;
using System;
using System.Diagnostics;

namespace Plugin.Xablu.Adal
{
    /// <summary>
    /// Cross platform Walkthrough implemenations
    /// </summary>
    public class CrossAdal
    {
        static Lazy<IAdal> implementation = new Lazy<IAdal>(() => CreateAdal(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IAdal Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IAdal CreateAdal()
        {
#if NETSTANDARD_20
            return null;
#else
            return new AdalImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
			new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        
    }
}
