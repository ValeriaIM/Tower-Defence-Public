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
            var wave = new Wave(new Enemy[]{new SimpleEnemy()}, new int[]{10}, new Enemy ReinforcedEnemy());            
            while(pl.HP > 0)
            {
                if (wave.QuantityEnemies[0] != 0)
                    map.EnemiesMap[0,0] = map.Enemies[0];
                var height = map.EnemiesMap.GetLength(0);
                var width = map.EnemiesMap.GetLength(1);
                var step = 0;
                for (int i = height - 1; i >= 0; i--)
                {
                    for (int j = width - 1; j >= 0; j--)
                    {
                        if (map.EnemiesMap[i, j] == null)
                            continue;
                        step = map.EnemiesMap[i, j].WatchSpeed();
                        var posi = i;
                        var posj = j;
                        while (step != 0)
                        {
                            if (map.Way[posi, posj] == '-')
                            { 
                                map.EnemiesMap[posi, posj + 1] = map.EnemiesMap[posi, posj];
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
                RealizeInteractionOfTowersAndEnemies(height, width, map);
            }
        }
        
        private void RealizeInteractionOfTowersAndEnemies(int height, int width, Map map)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map.TowersMap[i, j] == null)
                        continue;
                    var radius = map.TowersMap[i, j].WatchRadius();
                    for (int k = 0; k > radius; k++) 
                    { 
                        if (i - k > 0) 
                            LineCheck(i - k, j, k, false, map);
                        if (i + k < height) 
                            LineCheck(i + k, j, k, false, map); 
                        LineCheck(i - k, j, k, true, map); 
                    }
                }
            }
        }
        
        private void LineCheck(int i, int j, int k, bool isMedium, Map map)
        {
            if (j - k > 0)
                if (map.EnemiesMap[i, j - k] != null)
                    map.EnemiesMap[i, j - k].ReduceHP(map.TowersMap[i, j].Damage);
            if (j + k < width)
                if (map.EnemiesMap[i, j + k] != null)
                    map.EnemiesMap[i, j + k].ReduceHP(map.TowersMap[i, j].Damage);
            if ((!isMedium) and (map.EnemiesMap[i, j] != null))
                map.EnemiesMap[i, j].ReduceHP(map.TowersMap[i, j].Damage);
        }
        
        private static string AddressWay1 = @"C:\...\way1.txt"; // тут какой-то путь
        
        private string[] GetWay(string address)
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
        
        private void ProcessingChoice(Player pl, Map map, string choice)
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
        
        private Point ReceivePoint()
        {
            string position = null;
            while (position == null)
            {
                position = Console.ReadLine(); 
            }
            return new Point(Int32.Parse(position.Split()[0]), Int32.Parse(position.Split()[1]));
        }
    }



    public class Map
    {
        readonly Enemy[,] EnemiesMap;
        readonly char[,] Way;
        readonly Tower[,] TowersMap;
        readonly int Height;
        readonly int Width;
        public bool GameMode = false;
        private int powersPlayer;

        public int PowersPlayer
        {
            get { return powersPlayer; }
            private set { }
        }

        public Map(int width, int height, int powers, string[] way)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            Way = FillWay(Width, Height, way);            
            TowersMap = new Tower[Height, Width];
            PowersPlayer = powers;
        }

        private char[,] FillWay(int width, int height, string[] way)
        {
            var Way = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                Way[i] = way[i].ToCharArray();              
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
            if (Way[position.X, position.Y] != '.')
            {
                Console.WriteLine("Это путь врагов, здесь нельзя ставить");
                return;
            }
            if (tower.WatchPrice() > PowersPlayer)
            {
                Console.WriteLine("Денег не хватает");
                return;
            }
            PowersPlayer -= tower.WatchPrice();
            TowersMap[position.X, position.Y] = tower;
        }

        public void MoveTower(Point oldPos, Point newPos)
        {
            if (TowersMap[newPos.X, newPos.Y] != null)
            {
                Console.WriteLine("Место занято");
                return;
            }
            if (Way[newPos.X, newPos.Y] != '.')
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
            PowersPlayer += TowersMap[position.X, position.Y].WatchPrice();
            TowersMap[position.X, position.Y] = null;
        }

        public void StartGame()
        {
            GameMode = true;
        }
    }

    public class Player
    {
        readonly int HP;

        public Player(int hp)
        {
            HP = hp;
        }
        
        public void GetDamage(int damage)
        {
            Hp -= damage;
        }

        public void BuildTower(Map map, Tower tower, Point position)
        {
            if (map.GameMode)
                return;
            map.InstallTower(tower, position);
        }
        
        public void MoveTower(Map map, Point pos, Point pos2)
        {
            if (map.GameMode)
                return;
            map.MoveTower(map, pos, pos2)
        }

        public void DeleteTower(Map map, Point position)
        {
            if (map.GameMode)
                return;
            map.RemoveTower(position);
        }

        public void StartGame(Map map)
        {
            map.StartGame();
        }

        public void EndGame(Map map) // потом
        {

        }
    }

    public class Wave
    {
        readonly Enemy[] Enemies;
        readonly int[] QuantityEnemies;
        readonly Enemy Boss;

        public Wave(Enemy[] enemies, int[] quantityEnemies, Enemy boss)
        {
            Enemies = enemies;
            QuantityEnemies = quantityEnemies;
            Boss = boss;
            if (Enemies.Length != QuantityEnemies.Length)
            {
                throw new Exception(); //не уверена в правильности
            }
        }
    }

    public class Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Enemy
    {
        private int Damage;
        private int Speed;
        private int HP;
        private int Value;

        public void ReduceHP(int damage)
        {
            HP -= damage;
        }

        public void ReduceSpeed(int speed)
        {
            Speed -= speed;
        }

        public int WatchValue()
        {
            return Value;
        }
        
        public int WatchSpeed()
        {
            return Speed;
        }
        
        public int WatchDamage()
        {
            return Damage;
        }
    }

    public class SimpleEnemy : Enemy //не уверена в правильности
    {
        int Damage = 100;
        double Speed = 2.0;
        int HP = 100;
        int Value = 30;
    }
    
    public class ReinforcedEnemy : Enemy //не уверена в правильности
    {
        int Damage = 300;
        double Speed = 3.0;
        int HP = 200;
        int Value = 100;
    }

    public class Tower
    {
        private int Price;
        private int Damage;
        private int DamageRadius;

        public int WatchPrice()
        {
            return Price;
        }
        
        public int WatchRadius()
        {
            return DamageRadius;
        }
    }

    public class SimpleTower : Tower //не уверена в правильности
    {
        int Price = 100;
        int Damage = 30;
        int DamageRadius = 2;
    }
}
