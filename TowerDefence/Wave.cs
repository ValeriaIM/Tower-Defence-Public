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
            {"easy-1", new Enemy(50, 50, 15) },
            {"easy-2", new Enemy(80, 70, 20) },
            {"easy-3", new Enemy(110, 80, 25) },
            {"medium-1", new Enemy(120, 90, 30) },
            {"medium-2", new Enemy(140, 100, 35) },
            {"medium-3", new Enemy(150, 110, 40) },
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
