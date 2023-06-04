using ComponentFactory.Krypton.Toolkit;
using Isocline.Controller;
using Isocline.Model;
using Isocline.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Isocline.View
{
    internal class IsoclineView
    {
        private IsoclineControler isoclineControler;
        private FunctionController functionController;
        private PictureBox picture;
        private KryptonPanel panel;
        private KryptonTextBox box_fxy;
        private KryptonTextBox b_min;
        private KryptonTextBox b_max;
        private KryptonTextBox b_x;
        private KryptonTextBox b_y;
        private KryptonTextBox b_step;
        private KryptonTextBox b_length;
        private KryptonTextBox constanta;
        private GraphMaster master;

        public IsoclineView(PictureBox picture, KryptonPanel panel, KryptonTextBox box_fxy, KryptonTextBox b_min, KryptonTextBox b_max, KryptonTextBox b_x, KryptonTextBox b_y, KryptonTextBox b_step, KryptonTextBox b_length, IsoclineControler isoclineControler, FunctionController functionController, KryptonTextBox constanta, GraphMaster master)
        {
            this.picture = picture;
            this.panel = panel;
            this.box_fxy = box_fxy;
            this.b_min = b_min;
            this.b_max = b_max;
            this.b_x = b_x;
            this.b_y = b_y;
            this.b_step = b_step;
            this.b_length = b_length;
            this.isoclineControler = isoclineControler;
            this.functionController = functionController;
            this.constanta = constanta;
            this.master = master;
        }


        public void ShowDirectionField(object sender, EventArgs e)
        {
            picture.Width = panel.Width;
            picture.Height = panel.Height;
            var f = functionController.FuncParser(box_fxy.Text);
            var min = double.Parse(b_min.Text);
            var max = double.Parse(b_max.Text);
            var steps = double.Parse(b_step.Text);
            var lenght = double.Parse(b_length.Text);
            double Min = min;
            double Max = max;
            double step = steps;
            double arrowLength = lenght;
            var gm = new GraphMaster(min, max, picture);
            gm.CreateGraph(panel.Width, panel.Height);
            Bitmap bmp = new Bitmap(picture.Image);

            Graphics g = Graphics.FromImage(bmp);

            for (double x = Min; x <= Max; x += step)
            {
                for (double y = Min; y <= Max; y += step)
                {
                    double slope = f(x, y);
                    double deltaX = isoclineControler.CalculateX(slope, steps, lenght);
                    double deltaY = isoclineControler.CalculateY(slope, steps, lenght);
                    g.DrawLine(Pens.Black, Helpers.Transform((float)(x - deltaX), (float)(y - deltaY), min, max, picture.Width, picture.Height), Helpers.Transform((float)(x + deltaX), (float)(y + deltaY), min, max, picture.Width, picture.Height));

                }
            }
            picture.Image = bmp;
        }
        public void ShowIsoClines(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(picture.Image);
            Graphics g = Graphics.FromImage(bmp);
            picture.Width = panel.Width;
            picture.Height = panel.Height;
            var f = functionController.FuncParser(box_fxy.Text);
            var min = double.Parse(b_min.Text);
            var max = double.Parse(b_max.Text);
            var steps = double.Parse(b_step.Text);
            var length = double.Parse(b_length.Text);
            var interval = double.Parse(constanta.Text);
            DrawIsoclines(g, f, min, max, panel.Width, panel.Height, interval);
            picture.Image = bmp;
        }
        public void DrawIsoclines(Graphics g, Func<double, double, double> f, double min, double max, int width, int height, double value)
        {
            double stepX = (max - min) / width;
            double stepY = (max - min) / height;

            List<PointF> points = new List<PointF>();

            for (double x = min; x <= 0; x += stepX)
            {
                for (double y = min; y <= max; y += stepY)
                {
                    double equationValue = f(x, y);

                    if (Math.Abs(equationValue - value) < 1e-2) // Перевіряємо, чи відповідає значення диференціального рівняння значенню value
                    {
                        PointF point = Helpers.Transform((float)x, (float)y, (float)min, (float)max, width, height);
                        points.Add(point);
                    }
                }
            }

            if (points.Count > 1)
            {
                PointF[] pointsArray = points.ToArray();
                g.DrawCurve(Pens.Black, pointsArray);

            }
            points.Clear();

            for (double x = 0; x <= max; x += stepX)
            {
                for (double y = min; y <= max; y += stepY)
                {
                    double equationValue = f(x, y);

                    if (Math.Abs(equationValue - value) < 1e-2) // Перевіряємо, чи відповідає значення диференціального рівняння значенню value
                    {
                        PointF point = Helpers.Transform((float)x, (float)y, (float)min, (float)max, width, height);
                        points.Add(point);
                    }
                }
            }

            if (points.Count > 1)
            {
                PointF[] pointsArray = points.ToArray();
                g.DrawCurve(Pens.Black, pointsArray);
            }

        }

    }
}


