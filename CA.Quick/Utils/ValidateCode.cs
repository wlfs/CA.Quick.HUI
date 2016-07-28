using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick.Utils
{
    public class ValidateCode
    {
        public static MemoryStream Make() {
            var str=Helper.RndNum(4,2);
            return GrunImg(str);
        }
        public static MemoryStream GrunImg(string str)
        {
            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            int gheight = str.Length * 20;
            Img = new Bitmap(gheight, 35);
            g = Graphics.FromImage(Img);
            Random random = new Random();
            g.Clear(Color.FromArgb(243, 251, 254));
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(Img.Width);
                int y = random.Next(Img.Height);
                Img.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            var pfs = _fonts();
            Font f = new Font(pfs.Families[random.Next(pfs.Families.Length)], 20);
            SolidBrush s = new SolidBrush(Color.FromArgb(random.Next(1, 150), random.Next(1, 150), random.Next(1, 150)));
            g.DrawString(str, f, s, 3, 3);
            ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            Img.Dispose();
            return ms;
        }
        private static PrivateFontCollection _fonts()
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("ttfs/1.ttf");
            pfc.AddFontFile("ttfs/2.ttf");
            pfc.AddFontFile("ttfs/3.ttf");
            pfc.AddFontFile("ttfs/4.ttf");
            pfc.AddFontFile("ttfs/5.ttf");
            pfc.AddFontFile("ttfs/6.ttf");
            return pfc;
        }
    }
}
