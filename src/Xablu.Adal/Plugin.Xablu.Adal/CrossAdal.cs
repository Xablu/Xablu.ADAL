using Plugin.Xablu.Adal.Abstractions;
using System;

namespace Plugin.Xablu.Adal
{
  /// <summary>
  /// Cross platform Adal implemenations
  /// </summary>
  public class CrossAdal
  {
    static Lazy<IAdal> Implementation = new Lazy<IAdal>(() => CreateAdal(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IAdal Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IAdal CreateAdal()
    {
#if PORTABLE
        return null;
#else
        return new AdalImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
