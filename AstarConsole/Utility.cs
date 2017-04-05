using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AstarConsole
{
    public class Utility
    {
        private static int playerX = 0;
        private static int playerY = 0;
        private static int enemyX = 0;
        private static int enemyY = 0;

        public static void DrawMap(TileType[][] map)
        {
            Console.Clear();
            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map[x].GetUpperBound(0); y++)
                {
                    Console.SetCursorPosition(x, y);
                    switch (map[x][y])
                    {
                        case TileType.Wall:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("W");
                            break;
                        case TileType.Exit:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("e");
                            break;
                        case TileType.Enemy:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("@");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }
            Console.SetCursorPosition(playerX, playerY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("P");

            Console.ResetColor();
            Console.SetCursorPosition(0, map[0].Length);
        }


        public static void DrawPath(List<Node> path)
        {
            // Store the current cursor position so we can reset it
            int cx = Console.CursorLeft;
            int cy = Console.CursorTop;

            // Draw the path
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (Node node in path)
            {
                Console.SetCursorPosition(node.X, node.Y);
                Console.Write("#");
            }

            // Redraw the player and enemy
            Console.SetCursorPosition(playerX, playerY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("P");

            Console.SetCursorPosition(enemyX, enemyY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("@");

            // Reset the console
            Console.ResetColor();
            Console.SetCursorPosition(cx, cy);
        }


        public static TileType[][] LoadMap(string filename)
        {
            string[] mapdata = File.ReadAllLines(filename);
            int mapWidth = mapdata[0].Length;
            int mapHeight = mapdata.Length;

            TileType[][] map = new TileType[mapWidth][];
            for (int x = 0; x < mapWidth; x++)
            {
                map[x] = new TileType[mapHeight];
            }

            for (int y = 0; y < mapdata.Length; y++)
            {
                string line = mapdata[y];
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (line[x])
                    {
                        case 'W':
                            map[x][y] = TileType.Wall;
                            break;
                        case '@':
                            map[x][y] = TileType.Enemy;
                            enemyX = x;
                            enemyY = y;
                            break;
                        case 'P':
                            map[x][y] = TileType.Floor;
                            playerX = x;
                            playerY = y;
                            break;
                        case 'e':
                            map[x][y] = TileType.Exit;
                            break;
                        default:
                            map[x][y] = TileType.Floor;
                            break;
                    }
                }
            }

            return map;
        }

        public static int PlayerX
        {
            get { return playerX; }
        }

        public static int PlayerY
        {
            get { return playerY; }
        }

        public static int EnemyX
        {
            get { return enemyX; }
        }

        public static int EnemyY
        {
            get { return enemyY; }
        }
    }
}
