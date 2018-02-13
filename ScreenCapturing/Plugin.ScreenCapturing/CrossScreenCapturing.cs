using Plugin.ScreenCapturing.Abstractions;
using System;

namespace Plugin.ScreenCapturing
{
  /// <summary>
  /// Cross platform ScreenCapturing implemenations
  /// </summary>
  public class CrossScreenCapturing
  {
    static Lazy<IScreenCapturing> Implementation = new Lazy<IScreenCapturing>(() => CreateScreenCapturing(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IScreenCapturing Current
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

    static IScreenCapturing CreateScreenCapturing()
    {
#if PORTABLE
        return null;
#else
        return new ScreenCapturingImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
