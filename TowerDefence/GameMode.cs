using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    internal class GameMode
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

        public bool Play()
        {
            foreach (var wave in waves)
            {
                foreach (var enemy in wave.Enemies)
                {
                    MoveEnemies(enemy);
                    if (player.HP <= 0)
                        return false;
                    HarmEnemies();
                    map.WatchMap();
                }

                while (liveEnemies > 0 && player.HP > 0)
                {
                    MoveEnemies();
                    HarmEnemies();
                    if (player.HP <= 0)
                        return false;
                    map.WatchMap();
                }
            }
            return true;
        }

        private void MoveEnemies(Enemy newEnemy = null)
        {
            if (map.EnemiesMap[map.End.X, map.End.Y] != null)
            {
                player.ReduceHP(map.EnemiesMap[map.End.X, map.End.Y].Damage);
                map.EnemiesMap[map.End.X, map.End.Y] = null;
                liveEnemies--;
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
            if (newEnemy != null) liveEnemies++;
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
                            if (map.IsInBounds(i + dx, j + dy) && map.EnemiesMap[i + dx, j + dy] != null)
                            {
                                map.EnemiesMap[i + dx, j + dy].ReduceHP(tower.Damage);
                                if (map.EnemiesMap[i + dx, j + dy].HP <= 0)
                                {
                                    map.AddPowers(map.EnemiesMap[i + dx, j + dy].Value);
                                    map.EnemiesMap[i + dx, j + dy] = null;
                                    liveEnemies--;
                                }
                                //return;
                            }
                        }
                }
        }
    }
}
