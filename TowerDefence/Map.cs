using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    public class Map
    {
        public Enemy[,] EnemiesMap { get; private set; }
        public char[,] WayMap { get; private set; }
        public HashSet<Point> WayEnemySet { get; private set; }
        public Point[] Way { get; private set; }
        public Tower[,] TowersMap { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public bool GameMode { get; private set; }
        public int PowersPlayer { get; private set; }
        public Point Begin {  get; private set; }
        public Point End { get; private set; }

        public Map(int width, int height, int powers, string[] way)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            WayMap = FillWayMap(Width, Height, way);
            TowersMap = new Tower[Height, Width];
            PowersPlayer = powers;
            WayEnemySet = new HashSet<Point>();
            FillWay(way, 0, 0, 0);
            Way = WayEnemySet.ToArray();
        }
        public Map(int width, int height, int powers, string[] way, Tower[,] towers)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            WayMap = FillWayMap(Width, Height, way);
            TowersMap = towers;
            PowersPlayer = powers;
            WayEnemySet = new HashSet<Point>();
            FillWay(way, 0, 0, 0);
            Way = WayEnemySet.ToArray();
        }

        private char[,] FillWayMap(int width, int height, string[] way)
        {
            var wayMap = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                var wayElements = way[i].ToCharArray();
                for (var j = 0; j < width; j++)
                {
                    wayMap[i, j] = wayElements[j];
                }
            }

            return wayMap;
        }

        private void FillWay(string[] way, int i, int j, int count)
        {
            if (i < 0 || j < 0 || i >= Height || j >= Width) return;
            var wayElements = way[i].ToCharArray();
            if (wayElements[j] == 'B')
            {
                Begin = new Point(i, j);
                WayEnemySet.Add(Begin);
                FillWay(way, i + 1, j, count + 1);
                FillWay(way, i, j + 1, count + 1);
            }
            else if (wayElements[j] == 'E')
            {
                End = new Point(i, j);
                WayEnemySet.Add(End);
                return;
            }
            else if (wayElements[j] == '-')
            {
                if (!WayEnemySet.Contains(new Point(i, j)))
                    WayEnemySet.Add(new Point(i, j));
                else
                    return;
                FillWay(way, i, j + 1, count + 1);
                FillWay(way, i, j - 1, count + 1);
            }
            else if (wayElements[j] == '|')
            {
                if (!WayEnemySet.Contains(new Point(i, j)))
                    WayEnemySet.Add(new Point(i, j));
                else
                    return;
                FillWay(way, i + 1, j, count + 1);
                FillWay(way, i - 1, j, count + 1);
            }
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
            return x >= 0 && y >= 0 && x < Height && y < Width;
        }

        public void AddPowers(int powers)
        {
            PowersPlayer += powers;
        }

        public int WatchPowers()
        {
            return PowersPlayer;
        }

        public void WatchMap()
        {
            Console.WriteLine();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (TowersMap[i, j] != null)
                    {
                        var typeTower = TowersMap[i, j].ImagePath;
                        char sign = typeTower[typeTower.Length - 2];
                        Console.Write(sign.ToString() + " ");
                    }                        
                    else if (EnemiesMap[i, j] != null)
                        Console.Write("V" + " ");
                    else Console.Write(WayMap[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Ваше количество очков - {PowersPlayer}.");
            Console.WriteLine();
        }
    }
}
