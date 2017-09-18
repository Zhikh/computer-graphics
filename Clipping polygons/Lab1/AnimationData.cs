using System;

namespace Lab1
{
    class AnimationData
    {
        private double[,] affine_transformation;
        private int rectOffsetX, rectOffSetY,
            ciclOffsetX, ciclOffSetY;
        private int rDelta;
        private int cDelta;
        private int timerFake;
        private int fi;

        public AnimationData()
        {
            ciclOffsetX = 0;
            ciclOffSetY = 0;
            rectOffsetX = 0;
            rectOffSetY = 0;
            rDelta = 0;
            cDelta = 0;
            timerFake = 0;
            fi = 0;

            affine_transformation = new double[3, 3];

            affine_transformation[0, 2] = 0;
            affine_transformation[1, 2] = 0;
            affine_transformation[2, 2] = 0;
            affine_transformation[2, 2] = 0;
            affine_transformation[2, 2] = 1;
        }

        public int Fi
        {
            get
            {
                return fi;
            }

            set
            {
                fi = value;

                affine_transformation[0, 0] = Math.Cos(fi);
                affine_transformation[0, 1] = Math.Sin(fi);
                affine_transformation[1, 0] = -Math.Sin(fi);
                affine_transformation[1, 1] = Math.Cos(fi);
            }
        }

        public int RectOffsetX
        {
            get
            {
                return rectOffsetX;
            }

            set
            {
                rectOffsetX = value;
            }
        }

        public int RectOffSetY
        {
            get
            {
                return rectOffSetY;
            }

            set
            {
                rectOffSetY = value;
            }
        }

        public int CiclOffsetX
        {
            get
            {
                return ciclOffsetX;
            }

            set
            {
                ciclOffsetX = value;
            }
        }

        public int CiclOffSetY
        {
            get
            {
                return ciclOffSetY;
            }

            set
            {
                ciclOffSetY = value;
            }
        }

        public int RDelta
        {
            get
            {
                return rDelta;
            }

            set
            {
                rDelta = value;
            }
        }

        public int CDelta
        {
            get
            {
                return cDelta;
            }

            set
            {
                cDelta = value;
            }
        }

        public int TimerFake
        {
            get
            {
                return timerFake;
            }

            set
            {
                timerFake = value;
            }
        }
    }
}
