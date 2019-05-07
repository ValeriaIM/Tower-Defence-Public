using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class Player
    {
        public int HP { get; private set; }

        public Player(int hp)
        {
            HP = hp;
        }

        public void GetDamage(int damage)
        {
            HP -= damage;
        }

        public void BuildTower(Map map, Tower tower, Point position)
        {
            if (map.GameMode)
                return;
            map.InstallTower(tower, position);
        }

        public void MoveTower(Map map, Point pos, Point pos2)
        {
            if (map.GameMode)
                return;
            map.MoveTower(pos, pos2);
        }

        public void DeleteTower(Map map, Point position)
        {
            if (map.GameMode)
                return;
            map.RemoveTower(position);
        }

        public void ReduceHP(int damage)
        {
            HP -= damage;
        }

        public void StartGame(Map map)
        {
            map.StartGame();
        }

        public void EndGame(Map map) // потом
        {

        }
    }
}
