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

        public Wave(Enemy[] enemies)
        {
            Enemies = enemies;
        }
    }
}
