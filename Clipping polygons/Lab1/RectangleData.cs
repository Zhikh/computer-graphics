using SDL2;
using System;

namespace Lab1
{
    class RectangleData
    {
        private int x0;         //P(x0, y0)
        private int y0;
        private int width;
        private int height;
        private SDL.SDL_Point[] points;
        private SDL.SDL_Point[] oldPoints;

        public RectangleData(int x0, int y0, int width, int height)
        {
            X0 = x0;
            Y0 = y0;
            Width = width;
            Height = height;

            oldPoints = new SDL.SDL_Point[4];
            oldPoints[0].x = X0;
            oldPoints[0].y = Y0;

            oldPoints[1].x = oldPoints[0].x + Width;
            oldPoints[1].y = oldPoints[0].y;

            oldPoints[2].x = oldPoints[1].x;
            oldPoints[2].y = Math.Abs(oldPoints[1].y - Height);

            oldPoints[3].x = Math.Abs(oldPoints[2].x - Width);
            oldPoints[3].y = oldPoints[2].y;
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

        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public SDL.SDL_Point[] Points
        {
            get
            {
                return points;
            }

            set
            {
                points = new SDL.SDL_Point[4];
                points = value;
            }
        }

        public SDL.SDL_Point[] OldPoints
        {
            get
            {
                return oldPoints;
            }

            set
            {
                oldPoints = value;
            }
        }
    }
}
