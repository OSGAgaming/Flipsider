using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

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
        public bool inFrame => ParallaxPosition.X > Utils.SafeBoundX.X - 100 && position.Y > Utils.SafeBoundY.X - 100 && ParallaxPosition.X < Utils.SafeBoundX.Y + 100 && position.Y < Utils.SafeBoundY.Y + 100;
        public bool Surrounded => Main.CurrentWorld.IsActive(i,j-1) && Main.CurrentWorld.IsActive(i, j + 1) && Main.CurrentWorld.IsActive(i - 1, j - 1) && Main.CurrentWorld.IsActive(i + 1, j);
        public TileManager TM => Main.CurrentWorld.tileManager;
        bool Buffer1;
        public override void OnUpdateInEditor()
        {
            if (world != null && inFrame)
            {
                if(!Surrounded && Buffer1)
                {
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, i, j);
                    AddModule("Collision", new Collideable(this, true, CollisionPoly, true, default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                if (Surrounded && !Buffer1)
                {
                    Chunk.Colliedables.RemoveThroughEntity(this);
                }
                Buffer1 = Surrounded;
                if (TM.GetTile(i, j) != null)
                {
                    InFrame = true;
                }
                else
                {
                    InFrame = false;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (world != null)
            {
             if (InFrame && Active)
             {
               spriteBatch.Draw(TM.tileDict[TM.GetTile(i, j).type], new Rectangle(i * 32, j * 32, 32, 32), frame, Color.White);
             }
            }
        }
        public void Kill()
        {
            Active = false;
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            Chunk.Colliedables.RemoveThroughEntity(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
        }
        public Tile(int type, Rectangle frame, Vector2 pos, bool ifWall = false) : base()
        {
            width = 32;
            height = 32;
            this.type = type;
            this.frame = frame;
            Active = true;
            wall = ifWall;
            position = pos*32;
            i = (int)(ParallaxPosition.X/ 32);
            j = (int)(ParallaxPosition.Y/ 32);
            world = Main.CurrentWorld;
        }
        public Tile(int type, Rectangle frame, bool ifWall = false) : base()
        {
            this.type = type;
            this.frame = frame;
            Active = true;
            wall = ifWall;
            world = Main.CurrentWorld;
        }
    }
}
