using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Flipsider.Engine
{
    [Serializable]
    public class LevelInfo
    {
        public Tile[,]? tiles;
        public Prop[] props;
        public Water[] WaterBodies;
        public LayerManagerInfo LMI;
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(
                world.tileManager.tiles, 
                world.propManager.props.ToArray(), 
                world.WaterBodies,
                world.layerHandler);
        }

        public void LoadToWorld(World world)
        {
            world.layerHandler = LMI.Load();
            for (int i = 0; i < tiles?.GetLength(0); i++)
            {
                for (int j = 0; j < tiles?.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        world.tileManager.AddTile(world, tiles[i, j].type, new Vector2(i, j), tiles[i, j].Layer);
                    }
                }
            }
            for (int i = 0; i < props?.Length; i++)
            {
                string prop = Encoding.UTF8.GetString(props[i].propEncode, 0, props[i].propEncode.Length);
                world.propManager?.AddProp(world, prop, props[i].Center,props[i].Layer);
            }
            for (int i = 0; i < WaterBodies?.Length; i++)
            {
                world.WaterBodies.Components.Add(new Water(WaterBodies[i]._frame, WaterBodies[i].Layer));
            }
        }
        public LevelInfo(Tile[,] tiles, Prop[] props, Manager<Water> WaterBodies, LayerHandler layerHandler)
        {
            this.tiles = tiles;
            this.props = props;
            this.WaterBodies = WaterBodies.Components.ToArray();
            LMI = layerHandler.Info;
        }
    }
}
