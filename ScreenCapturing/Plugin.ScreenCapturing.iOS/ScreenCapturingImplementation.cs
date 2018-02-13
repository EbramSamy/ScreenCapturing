using CoreGraphics;
using Foundation;
using Plugin.ScreenCapturing.Abstractions;
using SafariServices;
using System;
using System.Threading.Tasks;
using UIKit;
using WebKit;
using Xamarin.Forms.Platform.iOS;

namespace Plugin.ScreenCapturing
{
    /// <summary>
    /// Implementation for ScreenCapturing
    /// </summary>
    public class ScreenCapturingImplementation : IScreenCapturing
    {
        public bool TakeScreenshot(string screenshotPath, object screen)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(screenshotPath))
                {
                    Console.WriteLine("Plugin.Screenshot.TakeScreenshot: screenshotPath not set");
                    return false;
                }

                var screenshot = UIScreen.MainScreen.Capture();
                return SaveScreenshot(screenshot, screenshotPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Plugin.Screenshot.TakeScreenshot: {0}", e.Message);
            }

            return false;
        }

        private bool SaveScreenshot(UIImage screenshot, string screenshotPath)
        {
            try
            {
                NSData imgData = screenshot.AsJPEG();
                NSError err = null;

                if (imgData.Save(screenshotPath, false, out err))
                {
                    Console.WriteLine("Screenshot saved as " + screenshotPath);
                    return true;
                }
                else
                {
                    Console.WriteLine("Screenshot NOT saved as " + screenshotPath + " because" + err.LocalizedDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Plugin.Screenshot.SaveScreenshot: {0}", ex.Message);
            }

            return false;
        }
        public async Task<byte[]> CaptureAsync(Xamarin.Forms.View fView)
        {
             var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
            //var view = ConvertFormsToNative(fView, new CGRect(fView.X, fView.Y, fView.Height, fView.Width));
            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsPNG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }
    
        public async Task CaptureAndSaveAsync(Xamarin.Forms.View fView)
        {
            var bytes = await CaptureAsync(fView);
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string date = DateTime.Now.ToString().Replace("/", "-").Replace(":", "-");
            string localPath = System.IO.Path.Combine(documentsDirectory, "Screnshot-" + date + ".png");

            var chartImage = new UIImage(NSData.FromArray(bytes));
            chartImage.SaveToPhotosAlbum((image, error) =>
            {
                //you can retrieve the saved UI Image as well if needed using
                //var i = image as UIImage;
                if (error != null)
                {
                    Console.WriteLine(error.ToString());
                }
            });
        }

      

        private UIView ConvertFormsToNative(Xamarin.Forms.View view, CGRect size)
        {

            var renderer = RendererFactory.GetRenderer(view);

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }

    }
}