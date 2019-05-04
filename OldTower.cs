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
            Map map = new Map (20, 20, 500);
            Player pl = new Player(300);
            int level = 1;
            Console.WriteLine("                   Добро пожаловать в игру! ");
            Console.WriteLine("Вам необходимо расставить башенки так, чтобы враги вас не убили.");
            Console.WriteLine($"Ваш уровень - {level}.")
            while (!map.GameMode)
            {
                DisplayCommandList();
                //нужно сделать опцию просмотра карты у игрока,(не сейчас)можно также сделать уровни сложности (на легком можно троить во время игры)
                //выход из игры+
                string choice; 
                while (choise == null)
                {
                    choice = Console.ReadLine();
                }
                switch (Int32.Parse(choice))// вынести в отдельный метод потом
                {
                    case 1:
                        Console.WriteLine("Пожалуйста, выберите башню: ");
                        Console.WriteLine("1. SimpleTower. Price = 100, Damage = 30, DamageRadius = 2.");
                        string numberTower; 
                        while (numberTower == null)
                        {
                            numberTower = Console.ReadLine(); // надо преобразовать
                        }
                        Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                        string position; 
                        while (position == null)
                        {
                            position = Console.ReadLine(); // надо преобразовать
                        }
                        pl.BuildTower(map, tower, position);
                        break;
                    case 2:
                        Console.WriteLine("Пожалуйста, напишите координаты через пробел");
                        string position; 
                        while (position == null)
                        {
                            position = Console.ReadLine(); // надо преобразовать
                        }
                        pl.DeleteTower(map, position);
                        break;
                    case 3:
                        pl.WatchPowers(map);
                        break;
                    case 4:
                        pl.StartGame(map);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                
                // тело цикла
            }
            GameMode(map, level);
            // тело программы
        }
        
        private void DisplayCommandList()
        {
            Console.WriteLine("Пожалуйста, выберите команду: ");
            Console.WriteLine("1. Построить башню.");
            Console.WriteLine("2. Удалить башню.");
            Console.WriteLine("3. Посмотреть очки.");
            Console.WriteLine("4. Начать игру.");
            Console.WriteLine("P.S. Вы не потеряете очков во время редактирования, но на следующем уровне у вас уже не будет этих башен.");
            Console.WriteLine("Будьте внимательны, после начала игры будет невозможно строить новые башни");
        }
        
        private void GameMode(Map map, int level)
        {
        
        }
    }
    
    
    
    public class Map
    {
        readonly Enemy[,] EnemiesMap;
        readonly Tower[,] TowersMap;
        readonly int Height;
        readonly int Width;
        private bool GameMode = false;
        private int PowersPlayer;

        public Map(int width, int height, int powers)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            TowersMap = new Tower[Height, Width];
            PowersPlayer = powers;
        }
        
        public void WatchMapTowers()// только башни
        {
            for (int i = 0; i < TowersMap.GetLength(0); i++)
            {
                for (int j = 0; j < TowersMap.GetLength(1); j++)
                {
                    if (TowersMap[position.X, position.Y] == null)
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
        
        public int WatchPowers()
        {
            return PowersPlayer;
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
        
        public void BuildTower(Map map, Tower tower, Point position)
        {
            if (map.GameMode)
                return;            
            map.InstallTower(tower, position);
        }
        
        public void DeleteTower(Map map, Point position)
        {
            if (map.GameMode)
                return;            
            map.RemoveTower(position);
        }
        
        public int WatchPowers(Map map)
        {
            return map.WatchPowers();
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

        public Wave(Enemy[] enemies, int[] quantityEnemies)
        {
            Enemies = enemies;
            QuantityEnemies = quantityEnemies;
            if (Enemies.Length != QuantityEnemies.Length)
            {
                throw new LogicalException(); //не уверена в правильности
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

        public void ReduceHP(Map map, Point pos, int damage)
        {
            HP -= damage;
            if (HP <= 0)
                map.RemoveEnemy(pos)
        }

        public void ReduceSpeed(int speed)
        {
            Speed -= speed;
        }
        
        public int WatchValue()
        {
            return Value;
        }
    }
    
    public class SimpleEnemy: Enemy //не уверена в правильности
    {
        Damage = 100;
        Speed = 1.0;
        HP = 100;
        Value = 30;
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
    }
    
    public class SimpleTower: Tower //не уверена в правильности
    {
        Price = 100;
        Damage = 30;
        DamageRadius = 2;
    }
}
