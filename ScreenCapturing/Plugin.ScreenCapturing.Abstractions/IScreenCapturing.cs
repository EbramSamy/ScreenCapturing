using System;
using System.Threading.Tasks;

namespace Plugin.ScreenCapturing.Abstractions
{
  /// <summary>
  /// Interface for ScreenCapturing
  /// </summary>
  public interface IScreenCapturing
  {
        Task CaptureAndSaveAsync(Xamarin.Forms.View fView);
        Task<byte[]> CaptureAsync(Xamarin.Forms.View fview);
  }
}
