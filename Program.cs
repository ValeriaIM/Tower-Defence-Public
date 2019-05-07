using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    class Program
    {
        private static string AddressWay1 = 
            @"C:\Users\Ирина Заец\Documents\Visual Studio 2015\Projects\Tower Defence\Tower Defence\Maps\Map1.txt"; // тут какой-то путь

        static void Main()
        {
            var way = Map.GetWay(AddressWay1); // потом можно создать файл со списком адресов распол-я карт, и в зависимости от ур-ня выбирать нужную карту
            var map = new Map(20, 2, 500, way);
            var pl = new Player(300);
            var level = 1;

            Console.WriteLine("                   Добро пожаловать в игру! ");
            Console.WriteLine("Вам необходимо расставить башенки так, чтобы враги вас не убили.");
            Console.WriteLine($"Ваш уровень - {level}.");

            while (!map.GameMode)
            {
                DisplayCommandList();
                //нужно сделать опцию просмотра карты у игрока,(не сейчас)можно также сделать уровни сложности (на легком можно троить во время игры)
                //выход из игры+
                string choice = null;
                while (choice == null)
                {
                    choice = Console.ReadLine();
                }
                ProcessingChoice(pl, map, choice);
            }
            var waves = new Wave[0];
            var game = new GameMode(map, pl, waves);
        }
        
        private static void DisplayCommandList()
        {
            Console.WriteLine("Пожалуйста, выберите команду: ");
            Console.WriteLine("1. Построить башню.");
            Console.WriteLine("2. Перенести башню.");
            Console.WriteLine("3. Удалить башню.");
            Console.WriteLine("4. Посмотреть очки.");
            Console.WriteLine("5. Начать игру.");
            Console.WriteLine("P.S. Вы не потеряете очков во время редактирования, но на следующем уровне у вас уже не будет этих башен.");
            Console.WriteLine("Будьте внимательны, после начала игры будет невозможно строить новые башни");
        }

        private static void ProcessingChoice(Player pl, Map map, string choice)
        {
            Point pos;
            Point pos2;
            switch (Int32.Parse(choice))
            {
                case 1:
                    Console.WriteLine("Пожалуйста, выберите башню: ");
                    Console.WriteLine("1. SimpleTower. Price = 100, Damage = 30, DamageRadius = 2.");
                    string numberTower = null;
                    while (numberTower == null)
                    {
                        numberTower = Console.ReadLine();
                    }
                    Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                    pos = ReceivePoint();
                    pl.BuildTower(map, tower, pos);
                    break;
                case 2:
                    Console.WriteLine("Пожалуйста, напишите текущие координаты через пробел");
                    pos = ReceivePoint();
                    Console.WriteLine("Пожалуйста, напишите новые координаты через пробел");
                    pos2 = ReceivePoint();
                    pl.MoveTower(map, pos, pos2);
                    break;
                case 3:
                    Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                    pos = ReceivePoint();
                    pl.DeleteTower(map, pos);
                    break;
                case 4:
                    pl.WatchPowers(map);
                    break;
                case 5:
                    pl.StartGame(map);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        private static Point ReceivePoint()
        {
            string position = null;
            while (position == null)
            {
                position = Console.ReadLine();
            }
            return new Point(Int32.Parse(position.Split()[0]), Int32.Parse(position.Split()[1]));
        }
    }
}
