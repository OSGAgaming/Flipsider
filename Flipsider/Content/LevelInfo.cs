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
        public Chunk[,] chunks;
        public LayerManagerInfo LMI;
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(
                world.tileManager.chunks,
                world.layerHandler);
        }

        public void LoadToWorld(World world)
        {
            world.layerHandler = LMI.Load();
        }
        public LevelInfo(Chunk[,] chunks, LayerHandler layerHandler)
        {
            this.chunks = chunks;
            LMI = layerHandler.Info;
        }
    }
}
