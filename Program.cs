using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
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

    public class Wave
    {
        public readonly Enemy Enemy1;
        public readonly int QuantityEnemy1;
        public readonly Enemy Enemy2;
        public readonly int QuantityEnemy2;
        public readonly Enemy Enemy3;
        public readonly int QuantityEnemy3;
        public readonly Enemy Enemy4;
        public readonly int QuantityEnemy4;

        public Wave(int damage, int squre, int x, int y)
        {
            Damage = damage;
            DamageRadius = squre;
            Position = new Point(x, y);
        }
    }
    
}
