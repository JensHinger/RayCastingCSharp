using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.src
{
    struct Player
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double DirX { get; set; }
        public double DirY { get; set; }
        public Player(double x, double y, double dirX, double dirY)
        {
            X = x;
            Y = y;
            DirX = dirX;
            DirY = dirY;
        }

        public (double, int) CheckCollision(ref Map map, double dirX, double dirY)
        {
            int iteratorX = 1;
            int iteratorY = 1;

            bool borderFoundY = false;
            bool borderFoundX = false;
            double borderDistY = double.MaxValue;
            double borderDistX = double.MaxValue;


            // Differentiate between positive dir vectors -> use ceiling / floor 
            while (!(borderFoundX & borderFoundY))
            {
                // Length of vectors when delta_x = 1 or delta_y = 1
                double s_x = iteratorX * Math.Sqrt(1 + (dirY / dirX) * (dirY / dirX));
                double s_y = iteratorY * Math.Sqrt(1 + (dirX / dirY) * (dirX / dirY));

                if (borderFoundY && s_x > s_y) { break; }
                else if (borderFoundX && s_y > s_x) { break; }
                else if (s_y < s_x)
                {
                    iteratorY++;

                    int intersect_y_x = (int)Math.Round(this.X + dirY * s_y);
                    int intersect_y_y;

                    if (dirX < 0) {  intersect_y_y = (int)Math.Ceiling(this.Y + dirX * s_y); }
                    else { intersect_y_y = (int)Math.Floor(this.Y + dirX * s_y); }
                    

                    bool fieldState = map.GetFieldState(intersect_y_x, intersect_y_y);

                    if (fieldState)
                    {
                        borderFoundY = true;
                        borderDistY = s_y;
                    }
                }
                else if (s_x <= s_y)
                {
                    iteratorX++;

                    int intersect_x_x;
                    int intersect_x_y = (int)Math.Round(this.Y + dirX * s_x); 

                    if (dirY < 0) { intersect_x_x = (int)Math.Ceiling(this.X + dirY * s_x); }
                    else { intersect_x_x = (int)Math.Floor(this.X + dirY * s_x); }

                    bool fieldState = map.GetFieldState(intersect_x_x, intersect_x_y);

                    if (fieldState)
                    {
                        borderFoundX = true;
                        borderDistX = s_x;
                    }
                }              
            }
            double s_xx = iteratorY * Math.Sqrt(1 + (dirY / dirX) * (dirY / dirX));
            double s_yy = iteratorX * Math.Sqrt(1 + (dirX / dirY) * (dirX / dirY));
            //Console.WriteLine("Interect Y " + Math.Round(this.X + dirY * s_yy) + " " + (this.Y + dirX * s_yy));
            //Console.WriteLine("Interect X "  + (this.X + dirY * s_xx) + " " + Math.Round(this.Y + dirX * s_xx));

            
            if (borderDistX < borderDistY) { return  (borderDistX, 1); }
            else { return (borderDistY, 0); }  
        }

        public (double[], int[]) CalculateRays(ref Map map, double fov)
        {
            double[] distArr = new double[720];
            int[] colArr = new int[720];

            // 720 is current screen width
            double deltaAngle = fov / 720;
            for (int i = 0; i < 720; i++) 
            {
                double rayAngle = (i - 360) * deltaAngle;
                // convert grad to rad
                rayAngle = (rayAngle * Math.PI) / 180;

                double rayDirX = this.DirX * Math.Cos(rayAngle) - this.DirY * Math.Sin(rayAngle);
                double rayDirY = this.DirX * Math.Sin(rayAngle) + this.DirY * Math.Cos(rayAngle);
                double rayLen = Math.Sqrt(rayDirX * rayDirX + rayDirY * rayDirY);

                // Normalize dir vectors
                rayDirX /= rayLen;
                rayDirY /= rayLen;

                int color;
                double collDist;

                (collDist, color) = CheckCollision(ref map, rayDirX, rayDirY);
                distArr[i] = collDist;
                colArr[i] = color;
            }

            return (distArr, colArr);
        }

        public void MoveX(double dist)
        {
            X += dist;
        }
    }
}
