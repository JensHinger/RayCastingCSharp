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
        public double Rotation { get; set; }

        public Player(double x, double y, double dirX, double dirY)
        {
            X = x;
            Y = y; 
            RayLen = Math.Sqrt(dirX * dirX + dirY * dirY);

            // Normalize dir Vectors
            DirX = dirX / RayLen;
            DirY = dirY / RayLen;

            Rotation = 0;
        }

        public (double, int) CheckCollision(ref Map map, double rayDirX, double rayDirY)
        {
            int iteratorX = 0;
            int iteratorY = 0;

            bool borderFoundY = false;
            bool borderFoundX = false;
            double borderDistY = double.MaxValue;
            double borderDistX = double.MaxValue;

            // Depending if dir vector negativ calculate position "in" cell
            // Think about this
            double yBorderDist = rayDirX switch
            {
                <= 0 => Y - Math.Truncate(Y),
                > 0 => 1 - (Y - Math.Truncate(Y)),
                _ => throw new Exception("Something went wrong!")
            };
            
            double xBorderDist = rayDirY switch
            {
                <= 0 => X - Math.Truncate(X),
                > 0 => 1 - (X - Math.Truncate(X)),
                _ => throw new Exception("Something went wrong!")
            };

            // TODO seems to stop working when one of the rays is negative?
            // Length of vectors when delta_x = 1 or delta_y = 1
            double deltaDistX = Math.Sqrt(1 + (rayDirY / rayDirX) * (rayDirY / rayDirX));
            double deltaDistY = Math.Sqrt(1 + (rayDirX / rayDirY) * (rayDirX / rayDirY));

            // Differentiate between positive dir vectors -> use ceiling / floor 
            while (!(borderFoundX & borderFoundY))
            {
                double s_x = iteratorX * deltaDistX + yBorderDist * deltaDistX;
                double s_y = iteratorY * deltaDistY + xBorderDist * deltaDistY;

                if (double.IsNaN(s_x)) { s_x = double.MaxValue; }
                if (double.IsNaN(s_y)) { s_y = double.MaxValue; }

                if (borderFoundY && s_x > s_y) { break; }
                else if (borderFoundX && s_y > s_x) { break; }
                // TODO when vectors are exact same how to know which wall color to choose?
                else if ((borderFoundX || borderFoundY) && double.Equals(rayDirX, rayDirY)) { Console.WriteLine(rayDirX + " EQUALS " + rayDirY); break; }
                // If dirX = 0, then deltaDistX = double.Infinity
                else if (s_y < s_x || double.IsInfinity(deltaDistX))
                {
                    iteratorY++;
                    //Console.WriteLine(rayDirY + " " + xBorderDist);
                    //Console.WriteLine("y_x " + (this.X + (rayDirY * s_y)));

                    int intersect_y_x = (int)Math.Round(this.X + rayDirY * s_y);
                    int intersect_y_y;

                    if (rayDirX < 0) {  intersect_y_y = (int)Math.Ceiling(this.Y + rayDirX * s_y); }
                    else { intersect_y_y = (int)Math.Floor(this.Y + rayDirX * s_y); }
                    
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
                    //Console.WriteLine("x_y " + (this.Y + rayDirX * s_x));

                    int intersect_x_x;
                    int intersect_x_y = (int)Math.Round(this.Y + rayDirX * s_x); 

                    if (rayDirY < 0) { intersect_x_x = (int)Math.Ceiling(this.X + rayDirY * s_x); }
                    else { intersect_x_x = (int)Math.Floor(this.X + rayDirY * s_x); }

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

                int color;
                double collDist;

                (collDist, color) = CheckCollision(ref map, rayDirX, rayDirY);
                distArr[i] = collDist;
                colArr[i] = color;
            }

            return (distArr, colArr);
        }

        public void Move(bool forward = true)
        {
            if (forward)
            {
                X += DirX;
                Y += DirY;
            } else
            {
                X -= DirX;
                Y -= DirY;
            }
            
        }

        public void Rotate(bool right = true) 
        {
            double rotation;

            if (right) { rotation = 5 * (Math.PI / 180); }
            else { rotation =  -5 * (Math.PI / 180); }

            double csn = Math.Cos(rotation);
            double sn = Math.Sin(rotation);

            // TODO change to "smooth" rotation
            double tempDirX = this.DirX * csn - this.DirY * sn;
            double tempDirY = this.DirX * sn + this.DirY * csn;

            DirX = tempDirX;
            DirY = tempDirY;
        }
    }
}
