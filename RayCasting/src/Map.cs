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
        public bool[,] GameField = new bool[50, 50];

        public Map() 
        {
            // Create map with one solid wall
            for (int i = 0; i < GameField.GetLength(0); i++) 
            {
                GameField[0, i] = true;
                GameField[i, 0] = true;
                GameField[49, i] = true;
                GameField[i, 49] = true;
            }

            GameField[3, 4] = true;
            GameField[3, 5] = true;
            GameField[3, 6] = true;

            for (int i = 0;i < GameField.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j=0; j < GameField.GetLength(1); j++)
                {
                    Console.Write("{0}", GameField[i, j]);
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
                // Index out of bounds
                return true; 
            }
        }

    }
}
