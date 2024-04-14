using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    internal class Levels
    {
        private static string AddressWay1 =
            @"C:\Users\mir_s\source\repos\TowerDefence\TowerDefence\way1.txt";
        private static string AddressWay2 =
            @"C:\Users\mir_s\source\repos\TowerDefence\TowerDefence\way2.txt";

        public HashSet<Level> levels { get; private set; }
        public Levels()
        {
            levels = new HashSet<Level>();

            Dictionary<string, int> enemies = new Dictionary<string, int>()
            {
                {"easy-1", 4},
                {"easy-2", 0},
                {"easy-3", 0},
                {"medium-1", 0},
                {"medium-2", 0},
                {"medium-3", 0},
            };

            Wave[] waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(1, AddressWay1, 200, 100, waves));

            enemies["easy-1"] = 6;
            enemies["easy-2"] = 2;
            waves = new Wave[]{ new Wave(enemies) };
            levels.Add(new Level(2, AddressWay1, 200, 100, waves));

            enemies["easy-1"] = 8;
            enemies["easy-2"] = 4;
            enemies["easy-3"] = 2;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(3, AddressWay1, 200, 150, waves));

            enemies["easy-1"] = 0;
            enemies["easy-2"] = 8;
            enemies["easy-3"] = 4;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(4, AddressWay1, 200, 150, waves));

            enemies["easy-2"] = 0;
            enemies["easy-3"] = 8;
            enemies["medium-1"] = 4;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(5, AddressWay1, 200, 150, waves));

            enemies["easy-1"] = 4;
            enemies["easy-2"] = 0;
            enemies["easy-3"] = 0;
            enemies["medium-1"] = 0;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(6, AddressWay2, 200, 200, waves));

            enemies["easy-1"] = 6;
            enemies["easy-2"] = 2;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(7, AddressWay2, 200, 150, waves));

            enemies["easy-1"] = 8;
            enemies["easy-2"] = 4;
            enemies["easy-3"] = 2;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(8, AddressWay2, 200, 150, waves));

            enemies["easy-1"] = 0;
            enemies["easy-2"] = 8;
            enemies["easy-3"] = 4;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(9, AddressWay2, 200, 150, waves));

            enemies["easy-2"] = 0;
            enemies["easy-3"] = 8;
            enemies["medium-1"] = 4;
            waves = new Wave[] { new Wave(enemies) };
            levels.Add(new Level(10, AddressWay2, 200, 150, waves));
        }

    }

    internal class Level
    {
        public int level { get; private set; }
        public string address { get; private set; }
        public int points { get; private set; }
        public int hp { get; private set; }
        public Wave[] waves { get; private set; }

        public Level(int level, string addr, int points, int hp, Wave[] waves)
        {
            this.level = level;
            this.address = addr;
            this.points = points;
            this.hp = hp;
            this.waves = waves;
        }
    }
}
