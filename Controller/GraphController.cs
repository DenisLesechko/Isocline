using Isocline.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Isocline.Controller
{
    internal class GraphController
    {
        private PictureBox picture;
        public GraphController(PictureBox picture_)
        {
            picture = picture_;
        }
        public void CreateGraphImage(double min, double max, int width, int height, Func<double, double, double> f, double x0, double y0,double accurac)
        {
            var gm = new GraphMaster(min, max, picture);
            gm.CreateGraph(width, height);
            gm.DrawLinearInterpolation(f, x0, y0,accurac);
        }
        public void CreateGraphImage2(double min, double max, int width, int height, Func<double, double, double> f, double x0, double y0, double accurac)
        {
            var gm = new GraphMaster(min, max, picture);
            gm.CreateGraph(width, height);
            gm.DrawQuadraticInterpolation(f, x0, y0,accurac);
        }
    }
}
