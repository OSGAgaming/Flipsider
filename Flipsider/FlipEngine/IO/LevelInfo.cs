﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FlipEngine
{
    [Serializable]
    public class LevelInfo : ISerializable<LevelInfo>
    {
        public TileManager tileManager { get; set; }
        public LayerManagerInfo LMI { get; set; }
        public Skybox skybox { get; set; }

        public static TileManager? TileManagerBuffer { get; set; }

        public static LevelInfo Convert(World world)
        {
            return new LevelInfo(
                world.tileManager,
                world.layerHandler,
                world.Skybox);
        }
        

        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            binaryWriter.Write(FormatVersion.CurrentVersion);

            LMI.Serialize(stream);
            tileManager.Serialize(stream);
            skybox.Serialize(stream);

            binaryWriter.Close();
        }
        public LevelInfo Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);

            FormatVersion.Version = binaryReader.ReadByte();

            TileScreen.AutoFrame = false;

            LayerManagerInfo lmfao = LMI.Deserialize(stream);

            FlipGame.World.layerHandler = lmfao.Load();
            TileManager TM = tileManager.Deserialize(stream);
            Skybox SKB = skybox.Deserialize(stream);

            LayerScreen.Instance?.Recalculate();

            TileManagerBuffer = null;

            TileScreen.AutoFrame = true;

            binaryReader.Close();
            return new LevelInfo(TM, lmfao.Load(), SKB);
        }

        public LevelInfo(TileManager tileManager, LayerHandler layerHandler, Skybox skybox)
        {
            this.tileManager = tileManager;
            LMI = layerHandler.Info;
            this.skybox = skybox;
        }
    }
}
