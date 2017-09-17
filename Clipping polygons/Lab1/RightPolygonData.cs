using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class RightPolygonData
    {
        private int n;          //number of coners
        private int x0;         //P(x0, y0)
        private int y0;
        private int r;          //radius
        private double phi;     //angle of inclination in radians (360/(2*n))*0,0174533

        public RightPolygonData(int n, int x0, int y0, int r, double phi)
        {
            N = n;
            X0 = x0;
            Y0 = y0;
            R = r;
            Phi = phi;
        }

        public int N
        {
            get
            {
                return n;
            }

            set
            {
                n = value;
            }
        }

        public int X0
        {
            get
            {
                return x0;
            }

            set
            {
                x0 = value;
            }
        }

        public int Y0
        {
            get
            {
                return y0;
            }

            set
            {
                y0 = value;
            }
        }

        public int R
        {
            get
            {
                return r;
            }

            set
            {
                r = value;
            }
        }

        public double Phi
        {
            get
            {
                return phi;
            }

            set
            {
                phi = value;
            }
        }
    }
}
