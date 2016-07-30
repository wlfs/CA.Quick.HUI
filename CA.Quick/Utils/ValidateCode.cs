using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public  MemoryStream Make() {
            var str=Helper.RndNum(4,2);
            return GrunImg(str);
        }
        int fontSize = 22;
        int width = 0;
        int height = 40;
        public  MemoryStream GrunImg(string str)
        {
            
            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            width = str.Length * (fontSize+7);
            Img = new Bitmap(width, height);
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
            for (int i = 0; i < str.Length; i++)
            {
                Font f = new Font(pfs.Families[random.Next(pfs.Families.Length)], fontSize);
                SolidBrush s = new SolidBrush(Color.FromArgb(random.Next(1, 150), random.Next(1, 150), random.Next(1, 150)));
                g.DrawString(str[i].ToString(), f, s, 3+(i*fontSize+7), 3);
                
            }
            _writeCurve(g);
             ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            Img.Dispose();
            return ms;
        }
 
        private  void _writeCurve(Graphics g)
        {
           
            double px =0, py = 0;
            // 曲线前部分
            Random random = new Random();
            SolidBrush bursh = new SolidBrush(Color.FromArgb(random.Next(1, 150), random.Next(1, 150), random.Next(1, 150)));
            var A = random.Next(1, height / 2);                  // 振幅
        double b = random.Next(-height / 4, height / 4);   // Y轴方向偏移量
        var f = random.Next(-height / 4, height / 4);   // X轴方向偏移量
        var T = random.Next(height, width * 2);  // 周期
        var w = (2 * Math.PI) /T;
                        
        var px1 = 0;  // 曲线横坐标起始位置
        var px2 = random.Next(width / 2, (int)(width * 0.8));  // 曲线横坐标结束位置

            for (px =px1; px <=px2; px = px + 1) {
                if (w != 0) {
                py = A* Math.Sin(w *px + f)+ b + height / 2;  // y = Asin(ωx+φ) + b
                var i = (int)(fontSize / 5);
                    g.DrawLine(new Pen(bursh), (float)px, (float)py, (float)px +i, (float)py + i);
                }
            }
        
        // 曲线后部分
        A = random.Next(1, height / 2);                  // 振幅		
        f = random.Next(-height / 4, height / 4);   // X轴方向偏移量
        T = random.Next(height, width * 2);  // 周期
        w = (2 * Math.PI) /T;		
        b = py - A* Math.Sin(w *px + f) - height / 2;
        px1 = px2;
        px2 = width;

            for (px =px1; px <=px2; px =px + 1) {
                if (w != 0) {
                py = A* Math.Sin(w *px + f)+ b + height / 2;  // y = Asin(ωx+φ) + b
                var i = (int)(fontSize / 5);
                    g.DrawLine(new Pen(bursh), (float)px, (float)py, (float)px + i, (float)py + i);
                }
            }
        }
    
        private static PrivateFontCollection _fonts()
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            var path=Helper.GetBinPath();
            Debug.WriteLine(path + "/ttfs/1.ttf");
            pfc.AddFontFile(path+"/ttfs/1.ttf");
            pfc.AddFontFile(path+"/ttfs/2.ttf");
            pfc.AddFontFile(path + "/ttfs/3.ttf");
            pfc.AddFontFile(path + "/ttfs/4.ttf");
            pfc.AddFontFile(path + "/ttfs/5.ttf");
            pfc.AddFontFile(path + "/ttfs/6.ttf");
            return pfc;
        }
    }
}
