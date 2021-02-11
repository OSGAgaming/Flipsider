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
           // world.layerHandler = LMI.Load();
        }

        public void Serialize(Stream stream)
        {
            LMI.Serialize(stream);
            tileManager.Serialize(stream);
        }

        public LevelInfo Deserialize(Stream stream)
        {
            LayerManagerInfo lmfao = LMI.Deserialize(stream);
            Main.CurrentWorld.layerHandler = lmfao.Load();
            TileManager TM = tileManager.Deserialize(stream);
            return new LevelInfo(TM, lmfao.Load());
        }

        public LevelInfo(TileManager tileManager, LayerHandler layerHandler)
        {
            this.tileManager = tileManager;
            LMI = layerHandler.Info;
        }
    }
}
