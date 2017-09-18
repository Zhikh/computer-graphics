using System;
using SDL2;

namespace Lab1
{
    static class BuildPolygon
    {
        public static void Build_RightPolygon(RightPolygonData cicle, double heightScale)
        {
            var points = new SDL.SDL_Point[cicle.N];
            double omega = Math.PI * 2 / cicle.N;
            int i;
            for (i = 0; i < cicle.N; i++)
            {
                points[i].x = (int)Math.Floor((cicle.X0 + cicle.R * Math.Cos(omega * i + cicle.Phi)) * heightScale);
                points[i].y = (int)Math.Floor((cicle.Y0 - cicle.R * Math.Sin(omega * i + cicle.Phi)) * heightScale);
            }
            cicle.Points = points;
        }

        public static void Build_Rectangle(RectangleData rectangle, double widthScale, double heightScale)
        {
            var points = new SDL.SDL_Point[4,2];

            //[p0p1]
            points[0, 0].x = rectangle.X0;
            points[0, 0].y = rectangle.Y0;
            points[0, 1].x = points[0, 0].x + rectangle.Width;
            points[0, 1].y = points[0, 0].y;

            //[p1p2]
            points[1, 0] = points[0, 1];
            points[1, 1].x = points[1, 0].x;
            points[1, 1].y = Math.Abs(points[1, 0].y - rectangle.Height);

            //[p3p2]
            points[2, 0].x =Math.Abs(points[1, 1].x - rectangle.Width);
            points[2, 0].y = points[1, 1].y;
            points[2, 1] = points[1, 1];

            //[p0p3]
            points[3, 0] = points[0, 0];
            points[3, 1] = points[2, 0];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    points[i, j].x = (int)Math.Floor(points[i, j].x * widthScale);
                    points[i, j].y = (int)Math.Floor(points[i, j].y * heightScale);
                }

            }

            rectangle.Points = points;
        }
    }
}
