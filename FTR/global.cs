using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;


namespace FTR
{
    public class global
    {
        public static Form1 form_menu; //форма
        public static Vector ScreenScale; //Размер экрана
        public static int MapOffset;
        public static Image ScaleImage(Image Img) //Меняет размер изображения относительно размера экрана
        {
            return (Image)(new Bitmap(Img, new Size((int)Math.Floor(Img.Width * ScreenScale.X), (int)Math.Floor(Img.Height * ScreenScale.Y))));
        }
    }
}
