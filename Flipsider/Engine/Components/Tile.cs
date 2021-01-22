using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Flipsider
{
    [Serializable]
    public class Tile : NonLivingEntity
    {
        public int type;
        [NonSerialized]
        public Rectangle frame;
        public bool wall;
        public int i;
        public int j;
        [NonSerialized]
        public World world;
        public int frameX;
        public int frameY;
        public bool inFrame => ParallaxedI > SafeBoundX.X - 5 && j > SafeBoundY.X - 5 && ParallaxedI < SafeBoundX.Y + 5 && j < SafeBoundY.Y + 5;

        private Vector2 ParallaxedIJ => new Vector2(i, j).AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);

        private int ParallaxedI => (int)ParallaxedIJ.X;

        private Vector2 SafeBoundX => new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ActualScreenSize.X / Main.ScreenScale) / 32;

        private Vector2 SafeBoundY => new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ActualScreenSize.Y / Main.ScreenScale) / 32;
        public TileManager TM => Main.CurrentWorld.tileManager;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (world != null)
            {
                if (world.IsTileInBounds(i, j))
                {
                    if (TM.tiles[i, j].type != -1)
                    {
                        spriteBatch.Draw(TM.tileDict[TM.tiles[i, j].type], new Rectangle(i * TileManager.tileRes, j * TileManager.tileRes, TileManager.tileRes, TileManager.tileRes), new Rectangle(new Point(frameX, frameY), new Point(32, 32)), Color.White);
                    }
                }
            }
        }
        public void Kill()
        {
            Active = false;
            Main.CurrentWorld.entityManager.RemoveComponent(this);
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            Main.Colliedables.RemoveThroughEntity(this);
            UpdateModules.Clear();
            Main.Updateables.Remove(this);
        }
        public Tile(int type, Rectangle frame, Vector2 pos, bool ifWall = false) : base()
        {
            width = 32;
            height = 32;
            maxHeight = 32;
            this.type = type;
            this.frame = frame;
            frameX = frame.Location.X;
            frameY = frame.Location.Y;
            Active = false;
            wall = ifWall;
            i = (int)pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).X;
            j = (int)pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).Y;
            world = Main.CurrentWorld;
            i = ParallaxedI;
            Main.AutoAppendToLayer(this);
            position = new Vector2(i * TileManager.tileRes, j * TileManager.tileRes);
        }
        public Tile(int type, Rectangle frame, Vector2 pos, int Layer, bool ifWall = false) : base()
        {
            width = 32;
            height = 32;
            maxHeight = 32;
            this.type = type;
            this.frame = frame;
            frameX = frame.Location.X;
            frameY = frame.Location.Y;
            Active = false;
            wall = ifWall;
            i = (int)pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).X;
            j = (int)pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).Y;
            world = Main.CurrentWorld;
            i = ParallaxedI;
            this.Layer = Layer;
            Main.AppendToLayer(this);
            position = new Vector2(i * TileManager.tileRes, j * TileManager.tileRes);
        }
        public Tile(int type, Rectangle frame, bool ifWall = false) : base()
        {
            this.type = type;
            this.frame = frame;
            Active = false;
            wall = ifWall;
            world = Main.CurrentWorld;
        }
    }
}
