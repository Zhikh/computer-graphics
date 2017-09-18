using System;
using SDL2;

namespace Lab1
{
    static class BuildPolygon
    {
        public static void Build_RightPolygon(RightPolygonData cicle, double heightScale, AnimationData param)
        {
            var points = new SDL.SDL_Point[cicle.N];
            double omega = Math.PI * 2 / cicle.N;
            int i;
            for (i = 0; i < cicle.N; i++)
            {
                points[i].x = (int)Math.Floor((cicle.X0 + cicle.R * Math.Cos(omega * i + cicle.Phi)) * heightScale + param.CiclOffsetX);
                points[i].y = (int)Math.Floor((cicle.Y0 - cicle.R * Math.Sin(omega * i + cicle.Phi)) * heightScale + param.CiclOffSetY);
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

            points[3].x = Math.Abs(points[2].x - rectangle.Width);
            points[3].y = points[2].y;

            for (int i = 0; i < 4; i++)
            {
                points[i].x = (int)Math.Floor(points[i].x * widthScale);
                points[i].y = (int)Math.Floor(points[i].y * heightScale);
            }
            rectangle.Points = points;
        }

        public static void Build_Rectangle(RectangleData rectangle, double widthScale, double heightScale, AnimationData param)
        {
            var points = new SDL.SDL_Point[4];
            var tempPoints = new SDL.SDL_Point[4];

            points[0].x = rectangle.X0 ;
            points[0].y = rectangle.Y0 ;

            points[1].x = points[0].x + rectangle.Width;
            points[1].y = points[0].y;

            points[2].x = points[1].x;
            points[2].y = Math.Abs(points[1].y - rectangle.Height);

            points[3].x = Math.Abs(points[2].x - rectangle.Width);
            points[3].y = points[2].y;

            for (int i = 0; i < 4; i++)
            {
                tempPoints[i].x = (int)Math.Floor((points[i].x * param.Affine_transformation[0, 0] + 
                    points[i].y * param.Affine_transformation[0,1] +  param.RectOffsetX + 70) * widthScale);
                tempPoints[i].y = (int)Math.Floor((points[i].x * param.Affine_transformation[0, 1] +
                     points[i].y * param.Affine_transformation[1, 1] + param.RectOffSetY + 100) * heightScale);
            }

            rectangle.Points = tempPoints;

            ////[p0p1]
            //points[0, 0].x = rectangle.X0;
            //points[0, 0].y = rectangle.Y0;
            //points[0, 1].x = points[0, 0].x + rectangle.Width;
            //points[0, 1].y = points[0, 0].y;

            ////[p1p2]
            //points[1, 0] = points[0, 1];
            //points[1, 1].x = points[1, 0].x;
            //points[1, 1].y = Math.Abs(points[1, 0].y - rectangle.Height);

            ////[p3p2]
            //points[2, 0].x =Math.Abs(points[1, 1].x - rectangle.Width);
            //points[2, 0].y = points[1, 1].y;
            //points[2, 1] = points[1, 1];

            ////[p0p3]
            //points[3, 0] = points[0, 0];
            //points[3, 1] = points[2, 0];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        points[i, j].x = (int)Math.Floor(points[i, j].x * widthScale);
            //        points[i, j].y = (int)Math.Floor(points[i, j].y * heightScale);
            //    }

            //}

    
        }

        public static bool isPointInsidePolygon(SDL.SDL_Point[] p, int arrLen, int x, int y)
        {
            int i1, i2, n, N, S, S1, S2, S3;
            bool flag = false;
            N = arrLen;
            for (n = 0; n < N; n++)
            {
                flag = false;
                i1 = n < N - 1 ? n + 1 : 0;
                while (!flag)
                {
                    i2 = i1 + 1;
                    if (i2 >= N)
                        i2 = 0;
                    if (i2 == (n < N - 1 ? n + 1 : 0))
                        break;
                    S = Math.Abs(p[i1].x * (p[i2].y - p[n].y) +
                        p[i2].x * (p[n].y - p[i1].y) +
                        p[n].x * (p[i1].y - p[i2].y));
                    S1 = Math.Abs(p[i1].x * (p[i2].y - y) +
                        p[i2].x * (y - p[i1].y) +
                        x * (p[i1].y - p[i2].y));
                    S2 = Math.Abs(p[n].x * (p[i2].y - y) +
                        p[i2].x * (y - p[n].y) +
                        x * (p[n].y - p[i2].y));
                    S3 = Math.Abs(p[i1].x * (p[n].y - y) +
                        p[n].x * (y - p[i1].y) +
                        x * (p[i1].y - p[n].y));
                    if (S == S1 + S2 + S3)
                    {
                        flag = true;
                        break;
                    }
                    i1 = i1 + 1;
                    if (i1 >= N)
                        i1 = 0;
                }
                if (!flag)
                    break;
            }
            return flag;
        }
    }
}
