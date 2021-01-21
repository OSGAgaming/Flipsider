using System;

namespace Flipsider.Engine
{
    [Serializable]
    public class LevelInfo
    {
        public Tile[,]? tiles;
        public Prop[] props;
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(world.tileManager.tiles, world.propManager.props.ToArray());
        }
        public LevelInfo(Tile[,] tiles, Prop[] props)
        {
            this.tiles = tiles;
            this.props = props;
        }
    }
}
