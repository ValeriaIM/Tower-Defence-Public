using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    class Program
    {
        static void Main(string[] args)
        {
            var way = GetWay(AddressWay1); // потом можно создать файл со списком адресов распол-я карт, и в зависимости от ур-ня выбирать нужную карту
            var map = new Map(20, 2, 500, way);
            var pl = new Player(300);
            int level = 1;
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
            GameMode(map, level, pl);
        }


        private static void GameMode(Map map, int level, Player pl)
        {
            var wave = new Wave(new Enemy[] { new SimpleEnemy() }, new int[] { 10 }, new Enemy ReinforcedEnemy());
            while (pl.HP > 0)
            {
                if (wave.QuantityEnemies[0] != 0)
                    map.EnemiesMap[0, 0] = wave.Enemies[0];
                var height = map.EnemiesMap.GetLength(0);
                var width = map.EnemiesMap.GetLength(1);
                var step = 0;
                for (int i = height - 1; i >= 0; i--)
                {
                    for (int j = width - 1; j >= 0; j--)
                    {
                        if (map.EnemiesMap[i, j] == null)
                            continue;
                        step = map.EnemiesMap[i, j].Speed;
                        var posi = i;
                        var posj = j;
                        while (step != 0)
                        {
                            if (map.Way[posi, posj] == '-')
                            {
                                map.EnemiesMap[posi, posj + q] = map.EnemiesMap[posi, posj];
                                map.EnemiesMap[posi, posj] = null;
                                posj++;
                            }
                            // здесь будут позже другие варианты, как можно ходить
                            if (map.Way[posi, posj] == 'E')
                            {
                                pl.GetDamage(map.Way[posi, posj].WatchDamage());
                                map.EnemiesMap[posi, posj] = null;
                            }
                            step--;
                        }
                    }
                }

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (map.TowersMap[i, j] == null)
                            continue;
                        var radius = map.TowersMap[i, j].DamageRadius;
                        // тут д.б. повреждение врагов
                    }
                }
            }

        }

        private static string AddressWay1 = @"C:\...\way1.txt"; // тут какой-то путь

        private static string[] GetWay(string address)
        {
            try
            {
                //считываем построчно
                var way = new List<string>();
                using (StreamReader sr = new StreamReader(address, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        way.Add(line);
                    }
                }
                return way.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
