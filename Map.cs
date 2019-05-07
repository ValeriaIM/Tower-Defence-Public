using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Map
    {
        public Enemy[,] EnemiesMap { get; private set; }
        public char[,] WayMap { get; private set; }
        public Point[] Way { get; private set; }
        public Tower[,] TowersMap { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public bool GameMode { get; private set; }
        public int PowersPlayer { get; private set; }

        public Map(int width, int height, int powers, string[] way)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            WayMap = FillWay(Width, Height, way);
            TowersMap = new Tower[Height, Width];
            PowersPlayer = powers;
        }

        private char[,] FillWay(int width, int height, string[] way)
        {
            var Way = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                var wayElements = way[i].ToCharArray();
                for (var j = 0; j < width; j++)
                {
                    Way[i, j] = wayElements[j];
                }
            }

            return Way;
        }

        public void WatchMapTowers()// только башни
        {
            for (int i = 0; i < TowersMap.GetLength(0); i++)
            {
                for (int j = 0; j < TowersMap.GetLength(1); j++)
                {
                    if (TowersMap[i, j] == null)
                        Console.Write(".");
                    else
                        Console.Write("S"); // S - SimpleTown, (потом)F - FrozenTown or other record
                }
                Console.WriteLine();
            }
        }

        public void InstallTower(Tower tower, Point position)
        {
            if (TowersMap[position.X, position.Y] != null)
            {
                Console.WriteLine("Место занято");
                return;
            }
            if (WayMap[position.X, position.Y] != '.')
            {
                Console.WriteLine("Это путь врагов, здесь нельзя ставить");
                return;
            }
            if (tower.Price > PowersPlayer)
            {
                Console.WriteLine("Денег не хватает");
                return;
            }
            PowersPlayer -= tower.Price;
            TowersMap[position.X, position.Y] = tower;
        }

        public void MoveTower(Point oldPos, Point newPos)
        {
            if (TowersMap[newPos.X, newPos.Y] != null)
            {
                Console.WriteLine("Место занято");
                return;
            }
            if (WayMap[newPos.X, newPos.Y] != '.')
            {
                Console.WriteLine("Это путь врагов, здесь нельзя ставить");
                return;
            }
            TowersMap[newPos.X, newPos.Y] = TowersMap[oldPos.X, oldPos.Y];
            TowersMap[oldPos.X, oldPos.Y] = null;
        }

        public void RemoveTower(Point position)
        {
            if (TowersMap[position.X, position.Y] == null)
                return;
            PowersPlayer += TowersMap[position.X, position.Y].Price;
            TowersMap[position.X, position.Y] = null;
        }

        public void StartGame()
        {
            GameMode = true;
        }

        public static string[] GetWay(string path)
        {
            var way = File.ReadLines(path);
            return way.ToArray();
        }

        public bool IsInBounds(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < Width && point.Y < Height;
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public void AddPowers(int powers)
        {
            PowersPlayer += powers;
        }
    }

}
