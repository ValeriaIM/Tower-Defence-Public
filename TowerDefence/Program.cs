using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    class Program
    {
        private static string AddressWay1 =
            @"C:\Users\mir_s\source\repos\TowerDefence\TowerDefence\way1.txt";

        static void Main()
        {
            Console.WriteLine("                   Добро пожаловать в игру! ");
            Console.WriteLine("Вам необходимо расставить башенки так, чтобы враги вас не убили.");
            
            Levels levels = new Levels();
            int result = 0;
            Tower[,] oldTowers = null;
            string oldAddr = "";

            foreach (Level l in levels.levels)
            {
                int oldResult = result;
                var resultTowers = ProcessLevel(l, oldResult, oldTowers, oldAddr);
                result = resultTowers.Item1;

                while (result == 0)
                {
                    resultTowers = ProcessLevel(l, oldResult, oldTowers, oldAddr);
                    result = resultTowers.Item1;
                    Console.WriteLine();
                }

                oldTowers = resultTowers.Item2;
                oldAddr = resultTowers.Item3;
            }
        }

        private static (int, Tower[,], string) ProcessLevel(Level level, int oldPoints=0, Tower[,] oldTowers=null, string oldAdrr="")
        {
            var way = Map.GetWay(level.address);            
            var pl = new Player(level.hp);
            var map = createMap(way[0].Length, way.Length, level.points + oldPoints, way, oldTowers, 
                oldTowers != null && level.address == oldAdrr);
            Console.WriteLine($"Ваш уровень - {level.level}.");
            Console.WriteLine();
            Console.WriteLine("Карта уровня");
            map.WatchMap();

            while (!map.GameMode)
            {
                DisplayCommandList();
                string choice = null;
                while (choice == null)
                {
                    choice = Console.ReadLine();
                }
                ProcessingChoice(pl, map, choice);
            }

            var game = new GameMode(map, pl, level.waves);
            bool result = game.Play();
            if (result)
            {
                Console.WriteLine("Уровень пройден!");
                return (map.PowersPlayer, map.TowersMap, level.address);
            }
            else
            {
                Console.WriteLine("Уровень не пройден...");
                return (0, map.TowersMap, level.address);
            }
        }

        private static Map createMap(int width, int height, int points, string[] way, Tower[,] oldTowers, bool fl)
        {
            if (fl) return new Map(width, height, points, way, oldTowers);
            return new Map(width, height, points, way);
        }

        private static void DisplayCommandList()
        {
            Console.WriteLine("Пожалуйста, выберите команду: ");
            Console.WriteLine("1. Построить башню.");
            Console.WriteLine("2. Перенести башню.");
            Console.WriteLine("3. Удалить башню.");
            Console.WriteLine("4. Посмотреть очки.");
            Console.WriteLine("5. Начать игру.");
            Console.WriteLine("P.S. Вы не потеряете очков во время редактирования.");
            Console.WriteLine("P.S. Будьте внимательны, после начала игры будет невозможно строить новые башни");
        }

        private static void ProcessingChoice(Player pl, Map map, string choice)
        {
            Point pos;
            Point pos2;
            switch (Int32.Parse(choice))
            {
                case 1:
                    Console.WriteLine("Пожалуйста, выберите башню: ");
                    string[] typesTower = new[] { 
                        "1. Type = Tower[T]. Price = 200. Damage = 10. DamageRadius = 2.",
                        "2. Type = SmallTower[S]. Price = 100. Damage = 10. DamageRadius = 1.",
                        "3. Type = MediumTower[M]. Price = 400. Damage = 15. DamageRadius = 2."
                    };
                    foreach (string typeTower in typesTower)
                    {
                        Console.WriteLine(typeTower);
                    }
                    string numberTower = null;
                    while (numberTower == null)
                    {
                        numberTower = Console.ReadLine();
                        if (int.TryParse(numberTower, out int parsedNumber))
                        {
                            // Проверяем, лежит ли число в пределах допустимого диапазона
                            if (parsedNumber < 1 || parsedNumber > typesTower.Length)
                            {
                                Console.WriteLine("Недопустимый номер башни");
                                numberTower = null;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Должно быть одно число");
                            numberTower = null;
                        }
                    }
                    Tower tower = CreateTower(typesTower[int.Parse(numberTower) - 1]);
                    Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                    Console.WriteLine("Первое число должно быть больше 0 и меньше " + map.Height.ToString());
                    Console.WriteLine("Второе число должно быть больше 0 и меньше " + map.Width.ToString());
                    pos = ReceivePoint(map.Height, map.Width);
                    pl.BuildTower(map, tower, pos);
                    map.WatchMap();
                    break;
                case 2:
                    Console.WriteLine("Пожалуйста, напишите текущие координаты через пробел");
                    pos = ReceivePoint(map.Height, map.Width);
                    Console.WriteLine("Пожалуйста, напишите новые координаты через пробел");
                    pos2 = ReceivePoint(map.Height, map.Width);
                    pl.MoveTower(map, pos, pos2);
                    map.WatchMap();
                    break;                    
                case 3:
                    Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                    pos = ReceivePoint(map.Height, map.Width);
                    pl.DeleteTower(map, pos);
                    map.WatchMap();
                    break;
                case 4:
                    Console.WriteLine(pl.WatchPowers(map));
                    break;
                case 5:
                    pl.StartGame(map);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        private static Point ReceivePoint(int heigth, int width)
        {
            string position = null;
            while (position == null)
            {
                position = Console.ReadLine();
                var posSplit = position.Split();
                if (posSplit.Length < 2) 
                {
                    Console.WriteLine("Введите два числа");
                    position = null;
                    continue;
                }
                if (!(int.TryParse(posSplit[0], out var h) && int.TryParse(posSplit[1], out var w)))
                {
                    Console.WriteLine("Введите два числа");
                    position = null;
                    continue;
                }
                else
                {
                    if ((h >= heigth) || (w >= width))
                    {
                        Console.WriteLine("Первое число должно быть больше 0 и меньше " + heigth.ToString());
                        Console.WriteLine("Второе число должно быть больше 0 и меньше " + width.ToString());
                        position = null;
                        continue;
                    }
                }
            }

            return new Point(Int32.Parse(position.Split()[0]), Int32.Parse(position.Split()[1]));
        }

        private static Tower CreateTower(string name)
        {
            var settings = name.Split('.');
            var typeTower = settings[1].Split(' ')[3];
            var price = int.Parse(settings[2].Split(' ')[3]);
            var damage = int.Parse(settings[3].Split(' ')[3]);
            var damageRadius = int.Parse(settings[4].Split(' ')[3]);
            Tower tower = new Tower(typeTower, price, damage, damageRadius);
            return tower;
        }
    }
}
