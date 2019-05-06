using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class SimpleEnemy : Enemy //не уверена в правильности
    {
        int Damage = 100;
        double Speed = 1.0;
        int HP = 100;
        int Value = 30;
    }

    public class SimpleTower : Tower //не уверена в правильности
    {
        int Price = 100;
        int Damage = 30;
        int DamageRadius = 2;
    }
}