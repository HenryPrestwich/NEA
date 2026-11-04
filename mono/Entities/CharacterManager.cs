using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.Entities
{
    public static class CM
    {
        public static List<Enemy> EnemyList { get; private set; }
        public static Player Player { get; private set; }


        public static void CreatePlayer(Vector2 position)
        {
            if (Player == null)
            {
                Player = new Player(position);
            }
        }

        public static void CreateEnemy(Vector2 position)
        {
            EnemyList.Add(new Enemy(position));
        }
    }
}
