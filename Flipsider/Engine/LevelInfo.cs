using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    [Serializable]
    public class LevelInfo
    {
        public Tile[,]? tiles;
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(world.tileManager.tiles);
        }
        public LevelInfo(Tile[,] tiles)
        {
            this.tiles = tiles;
        }
    }
}
