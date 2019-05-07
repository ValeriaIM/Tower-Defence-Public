using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    class GameMode
    {
        private Map map;
        private Player player;
        private Wave[] waves;
        private int liveEnemies;

        public GameMode(Map map, Player player, Wave[] waves)
        {
            this.map = map;
            this.player = player;
            this.waves = waves;
        }

        public void Play()
        {
            foreach (var wave in waves)
            {
                foreach (var enemy in wave.Enemies)
                {
                    MoveEnemies(enemy);
                    if (player.HP <= 0)
                        return;
                    HarmEnemies();
                }

                while (liveEnemies > 0 && player.HP > 0)
                {
                    MoveEnemies();
                    HarmEnemies();
                    if (player.HP <= 0)
                        return;
                }
            }
        }

        private void MoveEnemies(Enemy newEnemy = null)
        {
            if (map.EnemiesMap[map.Width - 1, map.Height - 1] != null)
            {
                player.ReduceHP(map.EnemiesMap[map.Width - 1, map.Height - 1].Damage);
                map.EnemiesMap[map.Width - 1, map.Height - 1] = null;
                if (player.HP <= 0)
                    return;
            }
            for (var i = map.Way.Length - 2; i > -1; i--)
            {
                var current = map.Way[i];
                if (map.EnemiesMap[current.X, current.Y] == null)
                    continue;
                var next = map.Way[i + 1];
                map.EnemiesMap[next.X, next.Y] = map.EnemiesMap[current.X, current.Y];
                map.EnemiesMap[current.X, current.Y] = null;
            }
            map.EnemiesMap[map.Way[0].X, map.Way[0].Y] = newEnemy;
            liveEnemies++;
        }

        private void HarmEnemies()
        {
            for (var i = 0; i < map.Height; i++)
                for (var j = 0; j < map.Width; j++)
                {
                    if (map.TowersMap[i, j] == null)
                        continue;
                    var tower = map.TowersMap[i, j];
                    for (var dx = -tower.DamageRadius; dx < tower.DamageRadius + 1; dx++)
                        for (var dy = -tower.DamageRadius; dy < tower.DamageRadius + 1; dy++)
                        {
                            if (map.InBounds(i + dx, j + dy) && map.EnemiesMap[i + dx, j + dy] != null)
                            {
                                map.EnemiesMap[i + dx, j + dy].ReduceHP(tower.Damage);
                                if (map.EnemiesMap[i + dx, j + dy].HP <= 0)
                                {
                                    map.AddPowers(map.EnemiesMap[i + dx, j + dy].Value);
                                    map.EnemiesMap[i + dx, j + dy] = null;
                                    liveEnemies--;
                                }
                            }
                        }
                }
        }
    }
}
