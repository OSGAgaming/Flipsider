using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Flipsider.Engine
{
    [Serializable]
    public class LevelInfo : ISerializable<LevelInfo>
    {
        public TileManager tileManager;
        public LayerManagerInfo LMI;
        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(
                world.tileManager,
                world.layerHandler);
        }
        public void LoadToWorld(World world)
        {
            world.tileManager = tileManager;
            world.layerHandler = LMI.Load();
        }

        public void Serialize(Stream stream)
        {
            tileManager.Serialize(stream);
            LMI.Serialize(stream);
        }

        public LevelInfo Deserialize(Stream stream)
        {
            return new LevelInfo(tileManager.Deserialize(stream), LMI.Deserialize(stream).Load());
        }

        public LevelInfo(TileManager tileManager, LayerHandler layerHandler)
        {
            this.tileManager = tileManager;
            LMI = layerHandler.Info;
        }
    }
}
