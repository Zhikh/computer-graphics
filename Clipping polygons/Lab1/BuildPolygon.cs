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
            var points = new SDL.SDL_Point[4];

            points[0].x = rectangle.X0;
            points[0].y = rectangle.Y0;

            points[1].x = points[0].x + rectangle.Width;
            points[1].y = points[0].y;

            points[2].x = points[1].x;
            points[2].y = Math.Abs(points[1].y - rectangle.Height);

            points[3].x =Math.Abs(points[2].x - rectangle.Width);
            points[3].y = points[2].y;

            for (int i = 0; i < 4; i++)
            {
                points[i].x = (int)Math.Floor(points[i].x * widthScale);
                points[i].y = (int)Math.Floor(points[i].y * heightScale);
            }

            rectangle.Points = points;
        }
    }
}
