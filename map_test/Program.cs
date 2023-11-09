using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(20, 40);
            char[,] Map = map.CreateMap();
            for (int i = 0;i < Map.GetLength(0);i++)
            {
                for (int j = 0;j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i,j]);
                }
                Console.WriteLine();
            }
        }
    }
    internal class Map
    {
        public int Map_length { get; protected set; }
        public int Map_width { get; protected set; }
        
        public Random rnd = new Random();
        public Map(int x, int y)         // set length,width of map and count of enemy and health potions (constructor)
        {
            Map_length = x < 9 ? y : (x % 2 == 0 ? x + 1 : x);                                       // dont let player set lower than 9 - length and width
            Map_width = y < 9 ? y : (y % 2 == 0 ? y + 1 : y);                                        // if number is not odd we convert it to odd for pathfinding
        }
        public char[,] CreateMap()                                              //creating map with information which was dated in constructor
        {
            char[,] Map = new char[Map_length, Map_width];
            for (int i = 0; i < Map.GetLength(0); i++)                           //making wall around double array map to dont let player go out of it
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (i % 2 == 0 || j % 2 == 0)                                   //to create maze we divide map into walls and checkpoints
                    {
                        Map[i, j] = '#';
                    }
                    else                                                             
                    {
                        Map[i, j] = '@';
                    }
                }
            }
            int packman_x = Map_length/2 + 1;
            int packman_y = Map_width/2 + 1;
            Map[packman_x, packman_y] = ' ';
            while (CompleteFinder(Map))
            {
                int direction = rnd.Next(1,5);
                PackmanPathfinder(Map,ref packman_x,ref packman_y, direction);
            }
            return Map;
        }
        public bool CompleteFinder(char[,] map)                                     // we need to check every time in pool is there any checkpoint seft or not and as we know all checkpoints is placed in odd
        {                                                                           // numbers so we dont need to check all the numbers in 2d array only odd
            for (int i = 1;i < map.GetLength(0); i += 2)
            {
                for (int j = 1; j < map.GetLength(1);j += 2)
                {
                    if (map[i,j] == '@')
                        return true;
                }
            }
            return false;
        }
        public void PackmanPathfinder(char[,] Map,ref int packman_x,ref int packman_y,int direction)
        {
            switch (direction)
            {
                case 1:                                                                   // its rotation up so we need to check if there is map or not,to dont go out of maze
                    if (packman_x - 2 < 0) break;
                    else
                    {
                        if (Map[packman_x - 2, packman_y] == '@')                         // if we meet checkpint in road we deleting it and creating free space and also deleting wall in way to him
                        {
                            packman_x -= 2;
                            Map[packman_x, packman_y] = ' ';
                            Map[packman_x + 1, packman_y] = ' ';
                            break;
                        }
                        else if (Map[packman_x - 2, packman_y] == ' ')                         // if we met empty space we just torate our packman and continue road cause we know this place is already reachable
                        {
                            packman_x -= 2;
                            break;
                        }
                    }
                    break;
                case 2:                                                                     // its rotation down so we need to check same thing as for up
                    if (packman_x + 2 >= Map_length) break;
                    else
                    {
                        if (Map[packman_x + 2, packman_y] == '@')
                        {
                            packman_x += 2;
                            Map[packman_x, packman_y] = ' ';
                            Map[packman_x - 1, packman_y] = ' ';
                            break;
                        }
                        else if (Map[packman_x + 2, packman_y] == ' ')
                        {
                            packman_x += 2;
                            break;
                        }
                        break;
                    }
                case 3:                                                                     // its rotation left so we checking same things as for up and down below 
                    if (packman_y - 2 < 0) break;
                    else
                    {
                        if (Map[packman_x, packman_y - 2] == '@')
                        {
                            packman_y -= 2;
                            Map[packman_x, packman_y] = ' ';
                            Map[packman_x, packman_y + 1] = ' ';
                            break;
                        }
                        else if (Map[packman_x, packman_y - 2] == ' ')
                        {
                            packman_y -= 2;
                            break;
                        }
                        break;
                    }
                case 4:                                                                       // its rotation right so we checking same things as for up,down and left below
                    if (packman_y + 2 >= Map_width) break;
                    else
                    {
                        if (Map[packman_x, packman_y + 2] == '@')
                        {
                            packman_y += 2;
                            Map[packman_x, packman_y] = ' ';
                            Map[packman_x, packman_y - 1] = ' ';
                            break;
                        }
                        else if (Map[packman_x, packman_y + 2] == ' ')
                        {
                            packman_y += 2;
                            break;
                        }
                        break;
                    }
            }
        }
    }
}
