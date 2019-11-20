using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EPSIC_Bataille_Navale.Models
{
    public class Sprite
    {
        public Bitmap bitmap;
        public int angle = 0;

        public Sprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32;
            y *= 32;
            this.bitmap = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(this.bitmap);
            g.DrawImage(bitmap, -x, -y);
            bitmap.Dispose();
        }

        public void AddSprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32;
            y *= 32;
            Graphics g = Graphics.FromImage(this.bitmap);
            g.TranslateTransform((float)this.bitmap.Width / 2, (float)this.bitmap.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)this.bitmap.Width / 2, -(float)this.bitmap.Height / 2);

            g.DrawImage(bitmap, -x, -y);
            bitmap.Dispose();
        }

        public void RotateSprite(Directions direction)
        {
            angle = (int)direction;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public ImageSource ToImageSource()
        {
            var handle = bitmap.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public ImageBrush ToBrush()
        {
            return new ImageBrush(ToImageSource());
        }
    }
}
