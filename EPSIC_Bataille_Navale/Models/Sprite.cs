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

        /// <summary>
        /// Create a new sprite and add a bitmap to it
        /// </summary>
        /// <param name="bitmap">Base image</param>
        /// <param name="x">Sprite is the next 32px from X</param>
        /// <param name="y">Sprite is the next 32px from Y</param>
        public Sprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32; // A sprite is always a square of 32x32 px
            y *= 32;
            this.bitmap = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(this.bitmap);
            g.DrawImage(bitmap, -x, -y);
            bitmap.Dispose();
        }

        /// <summary>
        /// Add a bitmap to the sprite
        /// </summary>
        /// <param name="bitmap">Image to add</param>
        /// <param name="x">Sprite is the next 32px from X</param>
        /// <param name="y">Sprite is the next 32px from Y</param>
        public void AddSprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32;
            y *= 32;
            Graphics g = Graphics.FromImage(this.bitmap);
            g.TranslateTransform((float)this.bitmap.Width / 2, (float)this.bitmap.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)this.bitmap.Width / 2, -(float)this.bitmap.Height / 2);

            if (bitmap.Tag == null) // If Tag isn't defined (why?)
            {
                g.DrawImage(bitmap, -x, -y);
            }
            else
            {
                // Draw part of an ellipse
                g.FillEllipse(System.Drawing.Brushes.DarkGray, -x, -y, bitmap.Width * 32, bitmap.Height * 32);
            }
            bitmap.Dispose();
        }

        /// <summary>
        /// Change the sprite rotation
        /// Next bitmaps will have this value
        /// </summary>
        /// <param name="direction"></param>
        public void RotateSprite(Direction direction)
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

        /// <summary>
        /// Finalize image
        /// </summary>
        /// <returns>Objet of type ImageBrush, immediatly usable</returns>
        public ImageBrush ToBrush()
        {
            return new ImageBrush(ToImageSource());
        }
    }
}
