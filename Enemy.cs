using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Enemy
    {
        public int HP { get; private set; }
        public int Damage { get; private set; }
        public int Value { get; private set; }

        public Enemy(int hp, int damage, int speed, int value)
        {
            HP = hp;
            Damage = damage;
            Value = value;
        }

        public void ReduceHP(int damage)
        {
            HP -= damage;
        }
    }
}
