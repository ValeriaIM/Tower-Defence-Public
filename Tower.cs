using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Tower
    {
        public string ImagePath { get; private set; }
        public int Price { get; private set; }
        public int Damage { get; private set; }
        public int DamageRadius { get; private set; }

        public Tower(string imagePath, int price, int damage, int damageRadius)
        {
            ImagePath = imagePath;
            Price = price;
            Damage = damage;
            DamageRadius = damageRadius;
        }
    }
}
