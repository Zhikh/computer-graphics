using SDL2;

namespace Lab1
{
    class RectangleData
    {
        private int x0;         //P(x0, y0)
        private int y0;
        private int width;
        private int height;
        private SDL.SDL_Point[,] points;

        public RectangleData(int x0, int y0, int width, int height)
        {
            X0 = x0;
            Y0 = y0;
            Width = width;
            Height = height;
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

        public SDL.SDL_Point[,] Points
        {
            get
            {
                return points;
            }

            set
            {
                points = new SDL.SDL_Point[4, 2];
                points = value;
            }
        }
    }
}
