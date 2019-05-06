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
        public int Speed { get; private set; }
        public int Value { get; private set; }

        public void ReduceHP(int damage)
        {
            HP -= damage;
        }

        public void ReduceSpeed(int speed)
        {
            Speed -= speed;
        }
    }
}
