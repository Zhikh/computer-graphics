﻿using System;
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
        private AnimationData animParams;

        private bool mouseClicked;
        private int prevX, prevY;
        private double fi;

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
            OnInit();

            int clippingWindowWidth = SCREEN_WIDTH - 100;
            int clippingWindowHeight = SCREEN_HEIGHT - 25;

            mouseClicked = false;
            fi = 0;

            clippingWindow = new RectangleData(50, 50, clippingWindowWidth, clippingWindowHeight);
            cicle = new RightPolygonData(100, clippingWindow.X0 + 100, clippingWindow.Y0 + 100, 50, 0.4);
            rectangle = new RectangleData(clippingWindow.X0 + 400, clippingWindow.Y0 + 200, 200, 70);
            animParams = new AnimationData();

            OnLoop();

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

        private void OnLoop()
        {
            // some flags
            bool quit = false;

            while (!quit)
            {
                SDL.SDL_Event sdlEvent;
                SDL.SDL_PollEvent(out sdlEvent);

                quit = OnEvent(sdlEvent);
            }
        }

        private bool OnEvent(SDL.SDL_Event sdlEvent)
        {
            bool draw = true;

            switch (sdlEvent.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                {
                    return true;
                }
                case SDL.SDL_EventType.SDL_KEYDOWN:
                {
                    var key = sdlEvent.key;
                    string test = SDL.SDL_GetScancodeName(SDL.SDL_GetScancodeFromKey(key.keysym.sym));
                    switch (key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_DOWN:
                        {
                            ShowConsole();
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
                        mouseClicked = true;
                        prevX = sdlEvent.button.x;
                        prevY = sdlEvent.button.y;
                    }
                }
                break;
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                {
                    if (mouseClicked)
                    {
                        int x = sdlEvent.button.x;
                        int y = sdlEvent.button.y;

                        if (BuildPolygon.isPointInsidePolygon(rectangle.Points, rectangle.Points.Length, x, y))
                        {
                            animParams.RectOffsetX += (x - prevX);
                            animParams.RectOffSetY += (y - prevY);
                        }
                        if (BuildPolygon.isPointInsidePolygon(cicle.Points, cicle.Points.Length, x, y))
                        {
                            animParams.CiclOffsetX += (x - prevX);
                            animParams.CiclOffSetY += (y - prevY);
                        }
                    }
                }
                break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    {
                        mouseClicked = false;
                    }
                    break;
            }

            if (draw)
            {
                OnRender();
                Thread.Sleep(10); // somehow calibrate render loop
            }

            return false;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        private void ShowConsole()
        {
            if (AllocConsole())
            {
                int newWidth;
                do
                {
                    Console.WriteLine("Введите ширину окна: ");
                } while (!Int32.TryParse(Console.ReadLine(), out newWidth));
                clippingWindow.Width = newWidth;

                int newHeight;
                do
                {
                    Console.WriteLine("Введите высоту окна: ");
                } while (!Int32.TryParse(Console.ReadLine(), out newHeight));
                clippingWindow.Height = newHeight;
                FreeConsole();
            }
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

            if (cicle.N > 5)
            {
                if (animParams.TimerFake++ >= 5)
                {
                    if (animParams.RectOffsetX > 0 || animParams.RectOffsetX < -350 * widthScale)    //+-
                        animParams.RDelta *= -1;

                    if (animParams.CiclOffsetX > 350 * widthScale || animParams.CiclOffsetX < -80 * widthScale)     //+-
                        animParams.CDelta *= -1;

                    animParams.RectOffsetX += animParams.RDelta;
                    animParams.CiclOffsetX += animParams.CDelta;

                    animParams.TimerFake = 0;

                    fi ++;
                    if (fi > 360)
                        fi = 0;
                }
                animParams.SetFi( fi * Math.PI / 180);
                BuildPolygon.Build_Rectangle(clippingWindow, widthScale, heightScale);
                BuildPolygon.Build_RightPolygon(cicle, heightScale, animParams);
                BuildPolygon.Build_Rectangle(rectangle, widthScale, heightScale, animParams);
                //rectangle.OldPoints = rectangle.Points;
                //rectangle.Points = BuildPolygon.FindNewRotetedPoints(rectangle.Points, fi * Math.PI / 180);

                Draw_ClippingWindow(clippingWindow.Points);
                SDL.SDL_Point[] testPointArray = FindIntersectionPoints(clippingWindow.Points, rectangle.Points, true);
                Draw_Polygon(testPointArray, animParams.RectInsideClipWindow);
                testPointArray = FindIntersectionPointsForCicle(clippingWindow.Points, cicle.Points, false);
                Draw_Polygon(testPointArray, animParams.CiclInsideClipWindow);
            }
            SDL.SDL_RenderPresent(renderer);
        }

        void Draw_ClippingWindow(SDL.SDL_Point[] points)
        {
            int i;
            for (i = 1; i < points.Length; i++)
            {
                SDL.SDL_RenderDrawLine(renderer, points[i - 1].x, points[i - 1].y, points[i].x, points[i].y);
            }
            SDL.SDL_RenderDrawLine(renderer, points[points.Length - 1].x, points[points.Length - 1].y, points[0].x, points[0].y);
        }

        void Draw_Polygon(SDL.SDL_Point[] points, bool[] isInside)
        {
            int i;
            for (i = 1; i < points.Length; i++)
            {
                if(isInside[i - 1] || isInside[i])
                    SDL.SDL_RenderDrawLine(renderer, points[i - 1].x, points[i - 1].y, points[i].x, points[i].y);
            }
            if (isInside[points.Length - 1] || isInside[0])
                SDL.SDL_RenderDrawLine(renderer, points[points.Length - 1].x, points[points.Length - 1].y, points[0].x, points[0].y); 
        }

        SDL.SDL_Point[] FindIntersectionPointsForCicle(SDL.SDL_Point[] clippingPoints, SDL.SDL_Point[] polygonPoints, bool isRectangle)
        {
            int length = polygonPoints.Length;
            SDL.SDL_Point[] points = new SDL.SDL_Point[length];
            bool[] isInside = new bool[length];
            bool isOutside = false;
            IntersectionPoints pointSercher = new IntersectionPoints();

            int k = 0;
            for (int j = 1; j < polygonPoints.Length; j++)
            {
                points[k] = polygonPoints[j - 1];
                isInside[k] = BuildPolygon.isPointInsidePolygon(clippingPoints, clippingPoints.Length, points[k].x, points[k].y);
                if(isInside[k])
                    isInside[k] = !BuildPolygon.isPointInsidePolygon(rectangle.Points, rectangle.Points.Length, points[k].x, points[k].y);
                
                k++;
                for (int i = 1; i < 4; i++)
                {
                    if (pointSercher.IsSegmentsIntersect(clippingPoints[i - 1], clippingPoints[i], polygonPoints[j], polygonPoints[j - 1]))
                    {
                        isOutside = !(isOutside);
                        length++;
                        Array.Resize(ref points, length);
                        Array.Resize(ref isInside, length);
                        points[k] = pointSercher.crossingPoint;
                        isInside[k] = false;
                        k++;
                    }

                    if (pointSercher.IsSegmentsIntersect(rectangle.Points[i - 1], rectangle.Points[i], polygonPoints[j], polygonPoints[j - 1]))
                    {
                        isOutside = !(isOutside);
                        length++;
                        Array.Resize(ref points, length);
                        Array.Resize(ref isInside, length);
                        points[k] = pointSercher.crossingPoint;
                        isInside[k] = false;
                        k++;
                    }
                }

                if (pointSercher.IsSegmentsIntersect(clippingPoints[3], clippingPoints[0], polygonPoints[j], polygonPoints[j - 1]))
                {
                    isOutside = !(isOutside);
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }

                if (pointSercher.IsSegmentsIntersect(rectangle.Points[3], rectangle.Points[0], polygonPoints[j], polygonPoints[j - 1]))
                {
                    isOutside = !(isOutside);
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }
            }

            points[k] = polygonPoints[polygonPoints.Length - 1];
            isInside[k] = BuildPolygon.isPointInsidePolygon(clippingPoints, clippingPoints.Length, points[k].x, points[k].y);
            if (isInside[k])
                isInside[k] = !BuildPolygon.isPointInsidePolygon(rectangle.Points, rectangle.Points.Length, points[k].x, points[k].y);
            k++;
            for (int i = 1; i < 4; i++)
            {
                if (pointSercher.IsSegmentsIntersect(clippingPoints[i - 1], clippingPoints[i], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
                {
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }

                if (pointSercher.IsSegmentsIntersect(rectangle.Points[i - 1], rectangle.Points[i], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
                {
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }
            }

            if (pointSercher.IsSegmentsIntersect(clippingPoints[3], clippingPoints[0], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
            {
                length++;
                Array.Resize(ref points, length);
                Array.Resize(ref isInside, length);
                points[k] = pointSercher.crossingPoint;
                isInside[k] = false;
                k++;
            }
            if (pointSercher.IsSegmentsIntersect(rectangle.Points[3], rectangle.Points[0], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
            {
                length++;
                Array.Resize(ref points, length);
                Array.Resize(ref isInside, length);
                points[k] = pointSercher.crossingPoint;
                isInside[k] = false;
                k++;
            }

            if (isRectangle)
            {
                animParams.RectInsideClipWindow = isInside;
            }
            else
            {
                animParams.CiclInsideClipWindow = isInside;
            }
            return points;
        }

        SDL.SDL_Point[] FindIntersectionPoints(SDL.SDL_Point[] clippingPoints, SDL.SDL_Point[] polygonPoints, bool isRectangle)
        {
            int length = polygonPoints.Length;
            SDL.SDL_Point[] points = new SDL.SDL_Point[length];
            bool[] isInside = new bool[length];
            bool isOutside = false;
            IntersectionPoints pointSercher = new IntersectionPoints();

            int k = 0;
            for (int j = 1; j < polygonPoints.Length; j++)
            {
                points[k] = polygonPoints[j - 1];
                isInside[k] = BuildPolygon.isPointInsidePolygon(clippingPoints, clippingPoints.Length, points[k].x, points[k].y);
                k++;
                for (int i = 1; i < 4; i++)
                {
                    if (pointSercher.IsSegmentsIntersect(clippingPoints[i-1], clippingPoints[i], polygonPoints[j], polygonPoints[j-1]))
                    {
                        isOutside = !(isOutside);
                        length++;
                        Array.Resize(ref points, length);
                        Array.Resize(ref isInside, length);
                        points[k] = pointSercher.crossingPoint;
                        isInside[k] = false;
                        k++;
                    }
                }

                if (pointSercher.IsSegmentsIntersect(clippingPoints[3], clippingPoints[0], polygonPoints[j], polygonPoints[j - 1]))
                {
                    isOutside = !(isOutside);
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }
            }

            points[k] = polygonPoints[polygonPoints.Length - 1];
            isInside[k] = BuildPolygon.isPointInsidePolygon(clippingPoints, clippingPoints.Length, points[k].x, points[k].y);
            k++;
            for (int i = 1; i < 4; i++)
            {
                if (pointSercher.IsSegmentsIntersect(clippingPoints[i - 1], clippingPoints[i], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
                {
                    length++;
                    Array.Resize(ref points, length);
                    Array.Resize(ref isInside, length);
                    points[k] = pointSercher.crossingPoint;
                    isInside[k] = false;
                    k++;
                }
            }

            if (pointSercher.IsSegmentsIntersect(clippingPoints[3], clippingPoints[0], polygonPoints[polygonPoints.Length - 1], polygonPoints[0]))
            {
                length++;
                Array.Resize(ref points, length);
                Array.Resize(ref isInside, length);
                points[k] = pointSercher.crossingPoint;
                isInside[k] = false;
                k++;
            }
            
            if (isRectangle)
            {
                animParams.RectInsideClipWindow = isInside;
            }
            else
            {
                animParams.CiclInsideClipWindow = isInside;
            }
            return points;
        }
    }
}