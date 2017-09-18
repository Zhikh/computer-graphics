using System;
using SDL2;

namespace Lab1
{
    class IntersectionPoints
    {
        private int A;
        private int B;
        private int C;

        public SDL.SDL_Point p;

        //public SDL.SDL_Point crossingPoint
        //{
        //    get
        //    {
        //        return p;
        //    }
        //}

        private int vector_mult(int ax, int ay, int bx, int by) //векторное произведение
        {
            return ax * by - bx * ay;
        }

        private bool areCrossing(SDL.SDL_Point p1, SDL.SDL_Point p2, SDL.SDL_Point p3, SDL.SDL_Point p4)//проверка пересечения
        {
            int v1 = vector_mult(p4.x - p3.x, p4.y - p3.y, p1.x - p3.x, p1.y - p3.y);
            int v2 = vector_mult(p4.x - p3.x, p4.y - p3.y, p2.x - p3.x, p2.y - p3.y);
            int v3 = vector_mult(p2.x - p1.x, p2.y - p1.y, p3.x - p1.x, p3.y - p1.y);
            int v4 = vector_mult(p2.x - p1.x, p2.y - p1.y, p4.x - p1.x, p4.y - p1.y);
            if ((v1 * v2) < 0 && (v3 * v4) < 0)
                return true;
            return false;
        }
        
        private void LineEquation(SDL.SDL_Point p1, SDL.SDL_Point p2)
        {
            A = p2.y - p1.y;
            B = p1.x - p2.x;
            C = -p1.x * (p2.y - p1.y) + p1.y * (p2.x - p1.x);
        }

        private SDL.SDL_Point CrossingPoint(int a1, int b1, int c1, int a2, int b2, int c2)
        {
            SDL.SDL_Point pt = new SDL.SDL_Point();
            double d = (double)(a1 * b2 - b1 * a2);
            double dx = (double)(-c1 * b2 + b1 * c2);
            double dy = (double)(-a1 * c2 + c1 * a2);
            pt.x = (int)(dx / d);
            pt.y = (int)(dy / d);
            return pt;
        }

        public bool IsPointExist(SDL.SDL_Point p1, SDL.SDL_Point p2, SDL.SDL_Point p3, SDL.SDL_Point p4)
        {
            if (areCrossing(p1, p2, p3, p4))
            {
                int a1, b1, c1, a2, b2, c2;

                LineEquation(p1, p2);
                a1 = A; b1 = B; c1 = C;

                LineEquation(p3, p4);
                a2 = A; b2 = B; c2 = C;

                p = CrossingPoint(a1, b1, c1, a2, b2, c2);
                return true;
            }
            else
                return false;
        }

        //static double FindDelta(SDL.SDL_Point A, SDL.SDL_Point B, SDL.SDL_Point C, SDL.SDL_Point D)
        //{
        //    return (B.x - A.x)*(C.y - D.y) - (C.x - D.y)*(B.y - A.y); 
        //}

        //static SDL.SDL_Point Intersection(SDL.SDL_Point A, SDL.SDL_Point B, SDL.SDL_Point C, SDL.SDL_Point D)
        //{
        //    SDL.SDL_Point point;
        //    double xo = A.x, yo = A.y;
        //    double p = B.x - A.x, q = B.y - A.y;

        //    double x1 = C.x, y1 = C.y;
        //    double p1 = D.x - C.x, q1 = D.y - C.y;

        //    point.x = (int)Math.Floor((xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
        //        (q * p1 - q1 * p));
        //    point.y = (int)Math.Floor((yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
        //        (p * q1 - p1 * q));

        //    return point;
        //}
    }
}
