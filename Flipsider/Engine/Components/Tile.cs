using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
using Flipsider.Engine.Interfaces;

namespace Flipsider
{
    [Serializable]
    public class Tile : ILayeredComponent
    {
        public int type;
        [NonSerialized]
        public Rectangle frame;
        public bool active;
        public bool wall;
        public int i;
        public int j;
        [NonSerialized]
        public World world;
        public int frameX;
        public int frameY;
        bool inFrame => ParalaxedI > SafeBoundX.X - 5 && j > SafeBoundY.X - 5 && ParalaxedI < SafeBoundX.Y + 5 && j < SafeBoundY.Y + 5;
        Vector2 ParalaxedIJ => new Vector2(i, j).AddParalaxAcrossX(Main.layerHandler.Layers[Layer].paralax);
        int ParalaxedI => (int)ParalaxedIJ.X;
        Vector2 SafeBoundX => new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ActualScreenSize.X / Main.ScreenScale) / 32;
        Vector2 SafeBoundY => new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ActualScreenSize.Y / Main.ScreenScale) / 32;
        public TileManager TM => Main.CurrentWorld.tileManager;
        public void Draw(SpriteBatch spriteBatch)
        {
            if (i > 0 && j > 0 && i < world.MaxTilesX && i < world.MaxTilesY && inFrame)
            {
                if (TM.tiles[i, j] != null)
                {
                    if (TM.tiles[i, j].active)
                    {
                        if (TM.tiles[i, j].type != -1)
                        {
                            spriteBatch.Draw(TM.tileDict[TM.tiles[i, j].type], new Rectangle(i * TileManager.tileRes, j * TileManager.tileRes, TileManager.tileRes, TileManager.tileRes), new Rectangle(new Point(frameX, frameY), new Point(32, 32)), Color.White);
                        }
                    }
                }
            }

        }
        public int Layer { get; set; }
        public Tile(int type, Rectangle frame, Vector2 pos, bool ifWall = false)
        {
            this.type = type;
            this.frame = frame;
            this.frameX = frame.Location.X;
            this.frameY = frame.Location.Y;
            active = false;
            wall = ifWall;
            Layer = LayerHandler.CurrentLayer;
            i = (int)pos.AddParalaxAcrossX(Main.layerHandler.Layers[Layer].paralax).X;
            j = (int)pos.AddParalaxAcrossX(Main.layerHandler.Layers[Layer].paralax).Y;
            world = Main.CurrentWorld;
            i = ParalaxedI;
            Main.AppendToLayer(this);
        }
        public Tile(int type, Rectangle frame, bool ifWall = false)
        {
            this.type = type;
            this.frame = frame;
            active = false;
            wall = ifWall;
            world = Main.CurrentWorld;
        }
    }
}
