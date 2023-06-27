using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Isocline
{
    class GraphMaster
    {
        private double min = -10;
        private double max = 10;
        private int width;
        private int height;
        private static Font font = new Font("Times New Roman", 14);
        private PictureBox picture;
        public GraphMaster(double _min, double _max,PictureBox picture_)
        {
            min = _min;
            max = _max;
            picture = picture_;
        }

        public void CreateGraph(int w, int h)
        {
            width = w;
            height = h;
            Bitmap bmp = new Bitmap(picture.Image);

            Graphics g = Graphics.FromImage(bmp);
            DrawCoord(g);
            picture.Image = bmp;
        }

        private void DrawCoord(Graphics g)
        {
            g.DrawLine(Pens.Black, Helpers.Transform(0, min, min, max, width, height), Helpers.Transform(0, max, min, max, width, height));
            g.DrawLine(Pens.Black, Helpers.Transform(min, 0, min, max, width, height), Helpers.Transform(max, 0, min, max, width, height));
            for (var i = min; i < max; i += 1)
            {
                var transformPointY = Helpers.Transform(0, i, min, max, width, height);
                g.DrawString(String.Format("{0}", Math.Round(i, 2)), font, Brushes.Black, transformPointY);
                g.DrawLine(Pens.Black, transformPointY.X - 3, transformPointY.Y, transformPointY.X + 3, transformPointY.Y);

                var transformPointX = Helpers.Transform(i, 0, min, max, width, height);
                g.DrawString(String.Format("{0}", Math.Round(i, 2)), font, Brushes.Black, transformPointX);
                g.DrawLine(Pens.Black, transformPointX.X, transformPointX.Y - 3, transformPointX.X, transformPointX.Y + 3);
            }
        }

        public void DrawQuadraticInterpolation( Func<double, double, double> dy, double x0, double y0, double accurac)
        {
            Bitmap bmp = new Bitmap(picture.Image);
            Graphics g = Graphics.FromImage(bmp);
            double x = x0, y = y0, step = accurac;
            List<PointF> points = new List<PointF>();
            points.Add(Helpers.Transform(x, y, min, max, width, height));
            while (x <= max)
            {
                var k1 = dy(x, y);
                var k2 = dy(x + step / 2, y + step / 2 * k1);
                var k3 = dy(x + step, y + step * k2);

                var nx = x + step;
                var ny = y + step / 6 * (k1 + 4 * k2 + k3);

                try
                {
                  
                    Point p = Helpers.Transform(nx, ny, min, max, width, height);
                    if (p.X > 0 && p.X < picture.Width && p.Y > 0 && p.Y < picture.Height)
                        points.Add(p);
                }
                catch { }

                x = nx;
                y = ny;
            }
            g.DrawLines(Pens.Black, points.ToArray());
            x = x0;
            y = y0;
            points = new List<PointF>();
            points.Add(Helpers.Transform(x, y, min, max, width, height));
            while (x >= min)
            {
                var k1 = dy(x, y);
                var k2 = dy(x - step / 2, y - step / 2 * k1);
                var k3 = dy(x - step, y - step * k2);

                var nx = x - step;
                var ny = y - step / 6 * (k1 + 4 * k2 + k3);

                try
                {
                    
                    Point p = Helpers.Transform(nx, ny, min, max, width, height);
                    if (p.X > 0 && p.X < picture.Width && p.Y > 0 && p.Y < picture.Height)
                        points.Add(p);
                }
                catch { }

                x = nx;
                y = ny;
            }
            g.DrawLines(Pens.Black, points.ToArray());
            picture.Image = bmp;
        }
        public void DrawLinearInterpolation( Func<double, double, double> dy, double x0, double y0, double accurac)
        {
            Bitmap bmp = new Bitmap(picture.Image);
            Graphics g = Graphics.FromImage(bmp);
            double x = x0, y = y0, step = accurac;
            List<PointF> points = new List<PointF>();
            points.Add(Helpers.Transform(x, y, min, max, width, height));
            while (x <= max)
            {
                var k = dy(x, y);
                var nx = x + step;
                var ny = y + step * k;

                try
                {
                    Point p = Helpers.Transform(nx, ny, min, max, width, height);
                    if (p.X > 0 && p.X < picture.Width  && p.Y > 0 && p.Y < picture.Height)
                    points.Add(p);
                }
                catch { };

                x = nx;
                y = ny;
            }
            g.DrawLines(Pens.Black, points.ToArray());

            x = x0;
            y = y0;
            points = new List<PointF>();
            points.Add(Helpers.Transform(x, y, min, max, width, height));
            while (x >= min)
            {
                var k = dy(x, y);
                var nx = x - step;
                var ny = y - step * k;

                try
                {
                    Point p = Helpers.Transform(nx, ny, min, max, width, height);
                    if (p.X > 0 && p.X < picture.Width && p.Y > 0 && p.Y < picture.Height)
                        points.Add(p);
                }
                catch { };

                x = nx;
                y = ny;
            }
            g.DrawLines(Pens.Black, points.ToArray());
            picture.Image = bmp;
        }
    }
}
