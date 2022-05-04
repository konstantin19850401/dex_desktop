using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class ScanForm : Form
    {
        public ScanForm(string path, string mime)
        {
            InitializeComponent();

            /*
            if (!mime.Equals(".pdf"))
            {
                string sss = "sdcv";
                
            }
            else 
            {
                string ddd = "asdc";

                try
                {

                    using (FileStream fs = new FileStream(@"C:\Users\Public\Pictures\Sample Pictures\ДМС_Добавлять роли.pdf", FileMode.Open))
                    {
                        // передали поток документу
                        Apitron.PDF.Rasterizer.Document document = new Apitron.PDF.Rasterizer.Document(fs);

                        // постранично сохраняем в картинки
                        //for (int i = 0; i < document.Pages.Count; i++)
                        //{
                        Page currentPage = document.Pages[0];

                        // можно поменять настройки DPI, размер, формат сохраняемого изображения

                        using (Bitmap bitmap = currentPage.Render(500, 500, new RenderingSettings()))
                        {
                            bitmap.Save(string.Format("{0}.jpeg", 0), System.Drawing.Imaging.ImageFormat.Jpeg);
                            pictureBox1.Image = bitmap;
                            pictureBox1.Invalidate();
                            //pictureBox1.Load(bitmap);
                        }
                        //}
                    }
                }
                catch (Exception) {
                    string ff = "sdfv";
                }

                
            }
            */

            pictureBox1.Load(path);
            //pictureBox1.Size = new System.Drawing.Size(140, 140);



            try
            {
                
                
                
                Image myBitmap = pictureBox1.Image;


                int width = this.Width;
                var ww = (double) myBitmap.Width / width;
                var height = (double) myBitmap.Height / ww;

                //this.pictureBox1.Size = new Size(myBitmap.Width, myBitmap.Height);
                this.Height = (int)height;
                this.pictureBox1.Size = new Size(width, (int)height);
                //Size nSize = new Size(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2);
                Size nSize = new Size(width, (int)height);
                Image gdi = new Bitmap(nSize.Width, nSize.Height);
                Graphics ZoomInGraphics = Graphics.FromImage(gdi);
                ZoomInGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ZoomInGraphics.DrawImage(pictureBox1.Image, new System.Drawing.Rectangle(new Point(0, 0), nSize), new System.Drawing.Rectangle(new Point(0, 0), pictureBox1.Image.Size), GraphicsUnit.Pixel);
                ZoomInGraphics.Dispose();
                pictureBox1.Image = gdi;
                pictureBox1.Size = gdi.Size;
                
            }
            catch (Exception) { }  
        }
    }
}
