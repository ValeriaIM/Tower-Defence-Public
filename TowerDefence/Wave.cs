using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    public class Wave
    {
        public Enemy[] Enemies { get; private set; }

        public static Dictionary<string, Enemy> EnemiesTypes = new Dictionary<string, Enemy>()
        {
            {"easy-1", new Enemy(50, 50, 25) },
            {"easy-2", new Enemy(70, 70, 30) },
            {"easy-3", new Enemy(90, 80, 35) },
            {"medium-1", new Enemy(90, 90, 40) },
            {"medium-2", new Enemy(110, 100, 45) },
            {"medium-3", new Enemy(130, 110, 50) },
        };

        public Wave(Dictionary<string, int> countEnemies)
        {
            int count = 0;
            foreach (int c in countEnemies.Values)
            {
                count += c;
            }
            Enemies = new Enemy[count];
            foreach (string e_type in countEnemies.Keys)
            {
                for (var i = 0; i < countEnemies[e_type]; i++)
                {                    
                    Enemy e = EnemiesTypes[e_type];
                    Enemy e_copy = new Enemy(e.HP, e.Damage, e.Value);
                    Enemies[i] = e_copy;
                }
            }
        }
    }
}
