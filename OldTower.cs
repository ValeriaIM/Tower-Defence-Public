using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Map
    {
        readonly Enemy[,] EnemiesMap;
        readonly Tower[,] TowersMap;
        readonly int Height;
        readonly int Width;

        public Map(int width, int height)
        {
            Height = height;
            Width = width;
            EnemiesMap = new Enemy[Height, Width];
            TowersMap = new Tower[Height, Width];
        }

        public void InstallTower(Tower tower, Point position)
        {
            TowersMap[position.X, position.Y] = tower;
        }

        public void MoveTower(Point oldPos, Point newPos)
        {
            TowersMap[newPos.X, newPos.Y] = TowersMap[oldPos.X, oldPos.Y];
            TowersMap[oldPos.X, oldPos.Y] = null;
        }

        public void RemoveTower(Point position)
        {
            TowersMap[position.X, position.Y] = null;
        }
    }

    public class Player
    {
        readonly int HP;
        readonly int Points;
        readonly List<Tower> Towers;

        public Player(int hp, int points)
        {
            Points = points;
            HP = hp;
            Towers = new List<Tower>();
        }
    }

    public class Wave
    {
        readonly Enemy[] Enemies;

        public Wave(Enemy[] enemies)
        {
            Enemies = enemies;
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

        public void ReduceHP(int damage)
        {
            HP -= damage;
        }

        public void ReduceSpeed(int speed)
        {
            Speed -= speed;
        }
    }

    public class Tower
    {
        public readonly int Damage;
        public readonly int DamageRadius;
        public readonly Point Position;

        public Tower(int damage, int squre, int x, int y)
        {
            Damage = damage;
            DamageRadius = squre;
            Position = new Point(x, y);
        }
    }
}
