using System;
using System.Collections.Generic;
using System.IO;

namespace AstarConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialise the test environment
            TileType[][] map = Utility.LoadMap("mapdata.txt");
            Utility.DrawMap(map);
            Console.WriteLine("Path finding: E({0},{1}) -> P({2},{3})", Utility.EnemyX, Utility.EnemyY, Utility.PlayerX, Utility.PlayerY);

            Astar astar = new Astar(map);
            List<Node> path = astar.FindPath(Utility.EnemyX, Utility.EnemyY, Utility.PlayerX, Utility.PlayerY);
            if (path.Count == 0)
            {
                Console.WriteLine("NO PATH FOUND");
            }
            else
            {
                Utility.DrawPath(path);
            }
        }
    }
}