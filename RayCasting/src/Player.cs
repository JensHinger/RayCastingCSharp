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
        public double RayLen { get; set; }

        public Player(double x, double y, double dirX, double dirY)
        {
            X = x;
            Y = y;
            DirX = dirX;
            DirY = dirY;
            RayLen = Math.Sqrt(DirX * DirX + DirY * DirY);
        }

        public (double, int) CheckCollision(ref Map map, double dirX, double dirY)
        {
            int iteratorX = 0;
            int iteratorY = 0;

            bool borderFoundY = false;
            bool borderFoundX = false;
            double borderDistY = double.MaxValue;
            double borderDistX = double.MaxValue;

            // Depending if dir vector negativ calculate position "in" cell
            double xBorderDist = dirY switch
            {
                < 0 => Y - Math.Truncate(Y),
                >= 0 => 1 - (Y - Math.Truncate(Y)),
                _ => throw new Exception("Something went wrong!")
            };

            double yBorderDist = dirX switch
            {
                < 0 => X - Math.Truncate(X),
                >= 0 => 1 - (X - Math.Truncate(X)),
                _ => throw new Exception("Something went wrong!")
            };

            // Differentiate between positive dir vectors -> use ceiling / floor 
            while (!(borderFoundX & borderFoundY))
            {
                // Length of vectors when delta_x = 1 or delta_y = 1
                double deltaDistX = Math.Sqrt(1 + (dirY / dirX) * (dirY / dirX));
                double deltaDistY = Math.Sqrt(1 + (dirX / dirY) * (dirX / dirY));

                double s_x = iteratorX * deltaDistX + xBorderDist * deltaDistX;
                double s_y = iteratorY * deltaDistY + yBorderDist * deltaDistY;

                if (double.IsNaN(s_x)) { s_x = double.MaxValue; }
                if (double.IsNaN(s_y)) { s_y = double.MaxValue; }

                if (borderFoundY && s_x > s_y) { break; }
                else if (borderFoundX && s_y > s_x) { break; }
                // If dirX = 0, then deltaDistX = double.Infinity
                else if (s_y < s_x || double.IsInfinity(deltaDistX))
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
                // If dirY = 0, then deltaDistY = double.Infinity
                else if (s_x <= s_y || double.IsInfinity(deltaDistY))
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

                // Normalize dir vectors
                rayDirX /= RayLen;
                rayDirY /= RayLen;

                int color;
                double collDist;

                (collDist, color) = CheckCollision(ref map, rayDirX, rayDirY);
                distArr[i] = collDist;
                colArr[i] = color;
            }

            return (distArr, colArr);
        }

        public void Move()
        {
            Console.WriteLine(X + " " + Y);

            X += DirX / RayLen;
            Y += DirY / RayLen;

            Console.WriteLine(X + " " + Y);
        }

        public void RotateLeft() 
        {
            double rotation = - 5 * (Math.PI / 180);

            // TODO change to "smooth" rotation
            DirX = this.DirX * Math.Cos(rotation) - this.DirY * Math.Sin(rotation);
            DirY = this.DirX * Math.Sin(rotation) + this.DirY * Math.Cos(rotation);
        }

        public void RotateRight() 
        {
            double rotation = 5 * (Math.PI / 180);

            // TODO change to "smooth" rotation
            DirX = this.DirX * Math.Cos(rotation) - this.DirY * Math.Sin(rotation);
            DirY= this.DirX * Math.Sin(rotation) + this.DirY * Math.Cos(rotation);
        }
    }
}
