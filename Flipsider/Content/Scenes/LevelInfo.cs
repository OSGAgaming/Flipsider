using Flipsider.Engine.Interfaces;
using Flipsider.GUI.TilePlacementGUI;
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

            Main.Editor.AutoFrame = false;

            LayerManagerInfo lmfao = LMI.Deserialize(stream);

            Main.World.layerHandler = lmfao.Load();
            TileManager TM = tileManager.Deserialize(stream);
            Skybox SKB = skybox.Deserialize(stream);

            Logger.NewText(SKB.Layers.Count);
            Main.World.SetSkybox(new CitySkybox());

            LayerScreen.Instance?.Recalculate();

            TileManagerBuffer = null;

            Main.Editor.AutoFrame = true;

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
