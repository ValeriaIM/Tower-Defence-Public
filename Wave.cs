using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Wave
    {
        public Enemy[] Enemies { get; private set; }
        public int[] QuantityEnemies { get; private set; }

        public Wave(Enemy[] enemies, int[] quantityEnemies)
        {
            Enemies = enemies;
            QuantityEnemies = quantityEnemies;
            if (Enemies.Length != QuantityEnemies.Length)
            {
                throw new Exception(); //не уверена в правильности
            }
        }
    }
}
