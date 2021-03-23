using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Flipsider.Engine;
namespace Flipsider
{
    [Serializable]
    public class Tile : NonLivingEntity, IDrawData
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
                drawData = new DrawData(TM.tileDict[type], new Rectangle(position.ToPoint(), new Point(width, height)), frame, Color.White);
                if (!Surrounded && Buffer1)
                {
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, i, j);
                    AddModule("Collision", new Collideable(this, true, CollisionPoly, true, default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                if (Surrounded && !Buffer1)
                {
                    Chunk.Colliedables.RemoveThroughEntity(this);
                }
                Buffer1 = Surrounded;
                if(Main.CurrentWorld.IsActive(i, j))
                {
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
        }
        public DrawData drawData { get; set; }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (world != null)
            {
               if (InFrame && Active)
               {
                spriteBatch.Draw(TM.tileDict[type], new Rectangle(position.ToPoint(), new Point(width, height)), frame, Color.White);
                    return;
               }
            }   
            drawData = DrawData.Null;
        }
        public override void Dispose()
        {
            Active = false;
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            Chunk.Colliedables.RemoveThroughEntity(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(type);
            binaryWriter.Write(i);
            binaryWriter.Write(j);
            binaryWriter.Write(frame);
        }
        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            int type = binaryReader.ReadInt32();
            int x = binaryReader.ReadInt32();
            int y = binaryReader.ReadInt32();
            Rectangle R = binaryReader.ReadRect();
            Tile tile = new Tile(type, R, new Vector2(x, y));
            return Main.tileManager.AddTile(Main.CurrentWorld, tile);
        }


        protected override void PostConstructor()   
        {
            try
            {
                if (TileManager.CanPlace && Main.tileManager.GetTile(i, j) != null)
                {
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, i, j);
                    AddModule("Collision", new Collideable(this, true, CollisionPoly, true, default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
            }
            catch
            {

            }
        }
        public Tile(int type, Rectangle frame, Vector2 pos, bool ifWall = false) : base()
        {
            width = 32;
            height = 32;
            this.type = type;
            this.frame = frame;
            Active = true;
            InFrame = true;
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
            Active = false;
            wall = ifWall;
            world = Main.CurrentWorld;
        }
        public Tile() : base()
        {
            Active = false;
            world = Main.CurrentWorld;
        }
    }
}
