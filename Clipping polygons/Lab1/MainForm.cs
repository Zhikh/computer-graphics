using System;
using System.Threading;
using System.Windows.Forms;
using SDL2;
using System.Drawing;

namespace Lab1
{
    public partial class MainForm : Form
    {
        private const int SCREEN_WIDTH = 640;
        private const int SCREEN_HEIGHT = 480;

        private IntPtr window;
        private IntPtr renderer;


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



        private void SdlWorker()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            // init window and renderer
            window = SDL.SDL_CreateWindow("Lab 1", 100, 100, 100 + SCREEN_WIDTH, 100 + SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

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

                            switch (key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_DOWN:
                                    {
                                        // do smth
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
                    DrawShape();

                    Thread.Sleep(10); // somehow calibrate render loop
                }
            }

            // clean up
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        private void DrawShape()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);           // set window background color
            SDL.SDL_RenderClear(renderer);
            SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);   // set shape line color
            
            int width, height;
            int n = 100;
            var points = new SDL.SDL_Point[n];

            SDL.SDL_GetWindowSize(window, out width, out height);
            int x0 = 100, y0 = 200;
            int r = 100;
            double phi = 0.4;

            double widthScale = (double)width / SCREEN_WIDTH;
            double heightScale = (double)height / SCREEN_HEIGHT;
            

            Draw_RightPolygon(x0, y0, r, n, phi, heightScale);
            SDL.SDL_RenderPresent(renderer);
        }

        void Draw_Polygon(SDL.SDL_Point[] points, int n)
        {
            int i;
            for (i = 1; i < n; i++)
                SDL.SDL_RenderDrawLine(renderer, points[i - 1].x, points[i - 1].y,
                  points[i].x, points[i].y);
            SDL.SDL_RenderDrawLine(renderer, points[n - 1].x, points[n - 1].y,
              points[0].x, points[0].y);

        }

        void Draw_RightPolygon(int x0, int y0, int r, int n, double phi, double heightScale)
        {
            var points = new SDL.SDL_Point[n];
            if (n > 2)
            {
                double omega = Math.PI * 2 / n;
                int i;
                for (i = 0; i < n; i++)
                {
                    points[i].x = (int)Math.Floor((x0 + r * Math.Cos(omega * i + phi)) * heightScale);
                    points[i].y = (int)Math.Floor((y0 - r * Math.Sin(omega * i + phi)) * heightScale);
                }
                Draw_Polygon(points, n);
            }
        }
    }
}