using Microsoft.Xna.Framework;
using System;

namespace Flipsider.Engine
{
    [Serializable]
    public class LevelInfo
    {
        public Tile[,]? tiles;
        public Prop[] props;
        public Manager<Water> WaterBodies = new Manager<Water>();
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(world.tileManager.tiles, world.propManager.props.ToArray(), world.WaterBodies);
        }

        public void LoadToWorld(World world)
        {
            for (int i = 0; i < tiles?.GetLength(0); i++)
            {
                for (int j = 0; j < tiles?.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        world.tileManager.AddTile(world, tiles[i, j].type, new Vector2(i, j));
                    }
                }
            }

            for (int i = 0; i < props?.Length; i++)
            {
                world.propManager?.AddProp(world, props[i].prop, props[i].position);
            }
        }
        public LevelInfo(Tile[,] tiles, Prop[] props, Manager<Water> WaterBodies)
        {
            this.tiles = tiles;
            this.props = props;
            this.WaterBodies = WaterBodies;
        }
    }
}
