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
        /// Crée une nouvelle sprite et y ajoute un bitmap
        /// </summary>
        /// <param name="bitmap">Image de base</param>
        /// <param name="x">Découpe de 32x32 de l'image, pos X</param>
        /// <param name="y">Découpe de 32x32 de l'image, pos Y</param>
        public Sprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32; //Une sprite mesure forcément 32 x 32
            y *= 32;
            this.bitmap = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(this.bitmap);
            g.DrawImage(bitmap, -x, -y);
            bitmap.Dispose();
        }

        /// <summary>
        /// Ajoute un bitmap au sprite
        /// </summary>
        /// <param name="bitmap">Image à ajouter</param>
        /// <param name="x">Découpe de 32x32 de l'image, pos X</param>
        /// <param name="y">Découpe de 32x32 de l'image, pos Y</param>
        public void AddSprite(Bitmap bitmap, int x = 0, int y = 0)
        {
            x *= 32;
            y *= 32;
            Graphics g = Graphics.FromImage(this.bitmap);
            g.TranslateTransform((float)this.bitmap.Width / 2, (float)this.bitmap.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)this.bitmap.Width / 2, -(float)this.bitmap.Height / 2);

            if (bitmap.Tag == null) //Si le Tag n'a pas été défini
            {
                g.DrawImage(bitmap, -x, -y);
            }
            else
            {
                //Tracer un morceau d'ovale
                g.FillEllipse(System.Drawing.Brushes.DarkGray, -x, -y, bitmap.Width * 32, bitmap.Height * 32);
            }
            bitmap.Dispose();
        }

        /// <summary>
        /// Change la rotation du sprite
        /// Les prochains bitmap auront cette valeur
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
        /// Finalisation de l'image
        /// </summary>
        /// <returns>Objet de type ImageBrush directement applicable</returns>
        public ImageBrush ToBrush()
        {
            ImageBrush brush = new ImageBrush(ToImageSource());
            brush.Stretch = Stretch.Fill;
            return brush;
        }
    }
}
