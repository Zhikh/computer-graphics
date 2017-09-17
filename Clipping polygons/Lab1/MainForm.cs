using System;
using System.Threading;
using System.Windows.Forms;
using SDL2;
using System.Runtime.InteropServices;

namespace Lab1
{
    public partial class MainForm : Form
    {
        private const int SCREEN_WIDTH = 640;
        private const int SCREEN_HEIGHT = 480;

        private IntPtr window;
        private IntPtr renderer;

        private RightPolygonData cicle;
        private RectangleData rectangle;
        private RectangleData clippingWindow;

        public MainForm()
        {
            InitializeComponent();

            Thread sdlWorkerThread = new Thread(SdlWorker);

            sdlWorkerThread.Start();
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            // close the window in main thread

            Hide();
            Close();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        private void SdlWorker()
        {
            OnInit();
            int clippingWindowWidth = SCREEN_WIDTH - 100;
            int clippingWindowHeight = SCREEN_HEIGHT - 25;
            clippingWindow = new RectangleData(50, 50, clippingWindowWidth, clippingWindowHeight);
            cicle = new RightPolygonData(clippingWindow.X0 + 100, clippingWindow.Y0 + 100, 100, 100, 0.4);
            rectangle = new RectangleData(clippingWindow.X0 + 400, clippingWindow.Y0 + 100, 200, 100);

            // some flags
            bool quit = false;
            bool draw = true;

            while (!quit)
            {
                SDL.SDL_Event sdlEvent;
                SDL.SDL_PollEvent(out sdlEvent);

                switch (sdlEvent.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        {
                            quit = true;
                        }
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        {
                            var key = sdlEvent.key;
                            string test = SDL.SDL_GetScancodeName(SDL.SDL_GetScancodeFromKey(key.keysym.sym));
                            switch (key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_DOWN:
                                    {
                                        if (AllocConsole())
                                        {
                                            int newWidth;
                                            Console.WriteLine("Введите ширину окна: ");
                                            while(!Int32.TryParse(Console.ReadLine(), out newWidth))
                                            {
                                                Console.WriteLine("Введите ширину окна: ");
                                            }
                                            clippingWindow.Width = newWidth;
                                            
                                            int newHeight;
                                            Console.WriteLine("Введите ширину окна: ");
                                            while (!Int32.TryParse(Console.ReadLine(), out newHeight))
                                            {
                                                Console.WriteLine("Введите высоту окна: ");
                                            }
                                            clippingWindow.Height = newHeight;
                                            FreeConsole();
                                        }

                                        //IntPtr console = SDL.SDL_CreateWindow("test for input", 100, 100, 100, 100, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                                    }
                                    break;
                                case SDL.SDL_Keycode.SDLK_UP:
                                    {
                                        // do smth
                                    }
                                    break;
                            }
                        }
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        {
                            // start or stop rendering on left mouse button click
                            if (sdlEvent.button.button == SDL.SDL_BUTTON_LEFT)
                            {
                                draw = !draw;   // toggle value
                            }
                            else
                            if (sdlEvent.button.button == SDL.SDL_BUTTON_RIGHT)
                            {
                                // do smth
                            }
                        }
                        break;
                }

                if (draw)
                {
                    OnRender();
                    Thread.Sleep(10); // somehow calibrate render loop
                }
            }

            OnCleanup();
        }

        void OnCleanup()
        {
            // clean up
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        bool OnInit()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            // init window and renderer
            window = SDL.SDL_CreateWindow("Lab 3", 100, 100, 100 + SCREEN_WIDTH, 100 + SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            return true;
        }

        

        static public SDL.SDL_Point Intersection(SDL.SDL_Point A, SDL.SDL_Point B, SDL.SDL_Point C, SDL.SDL_Point D)
        {
            SDL.SDL_Point point;
            double xo = A.x, yo = A.y;
            double p = B.x - A.x, q = B.y - A.y;

            double x1 = C.x, y1 = C.y;
            double p1 = D.x - C.x, q1 = D.y - C.y;

            point.x = (int)Math.Floor((xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p));
            point.y = (int)Math.Floor((yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q));

            return point;
        }

        private void OnRender()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);           // set window background color
            SDL.SDL_RenderClear(renderer);
            SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);   // set shape line color
            
            int width, height;
            int n = 100;
            var points = new SDL.SDL_Point[n];

            SDL.SDL_GetWindowSize(window, out width, out height);

            double widthScale = (double)width / SCREEN_WIDTH;
            double heightScale = (double)height / SCREEN_HEIGHT;

            if (cicle.N > 2)
            {
                BuildPolygon.Build_Rectangle(clippingWindow, widthScale, heightScale);
                BuildPolygon.Build_RightPolygon(cicle, heightScale);
                BuildPolygon.Build_Rectangle(rectangle, widthScale, heightScale);

                SDL.SDL_Point point;

                int i = 0;
                while(i < 4)
                {
                    int j = 0;
                    while (j < cicle.N)
                    {
                        point = Intersection(clippingWindow.Points[i], clippingWindow.Points[i+1], cicle.Points[j], cicle.Points[j+1]);
                    }
                    i += 2;
                }


                Draw_Polygon(clippingWindow.Points, 4);
                Draw_Polygon(cicle.Points, cicle.N);
                Draw_Polygon(rectangle.Points, 4);
                
            }
            SDL.SDL_RenderPresent(renderer);
        }

        void Draw_Polygon(SDL.SDL_Point[] points, int n)
        {
            int i;
            for (i = 1; i < n; i++)
                SDL.SDL_RenderDrawLine(renderer, points[i - 1].x, points[i - 1].y, points[i].x, points[i].y);
            SDL.SDL_RenderDrawLine(renderer, points[n - 1].x, points[n - 1].y, points[0].x, points[0].y);
        }
    }
}