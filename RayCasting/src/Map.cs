using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.src
{
    internal class Map
    {
        public bool[,] GameField = new bool[10, 10];

        public Map() 
        {
            // Create map with one solid wall
            for (int i = 0; i < GameField.GetLength(0); i++) 
            {
                GameField[0, i] = true;
                GameField[i, 0] = true;
                GameField[9, i] = true;
                GameField[i, 9] = true;
            }

            //GameField[4, 4] = true;
            //GameField[4, 5] = true;
            //GameField[4, 6] = true;

            for (int i = 0;i < GameField.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j=0; j < GameField.GetLength(1); j++)
                {
                    Console.Write("{0}, {1}, {2}", GameField[i, j], i, j);
                }
            }
        }

        public bool GetFieldState(int x, int y)
        {
            try
            {
                return GameField[x, y];
            }
            catch {
                // Index not found
                return false; 
            }
        }

    }
}
