

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public class Tile : NonLivingEntity, IDrawData
    {
        public int type;
        public Rectangle frame;
        public int i;
        public int j;

        public bool Surrounded => 
            FlipGame.World.IsTileActive(i, j - 1) && 
            FlipGame.World.IsTileActive(i, j + 1) && 
            FlipGame.World.IsTileActive(i - 1, j - 1) && 
            FlipGame.World.IsTileActive(i + 1, j);

        private bool SurroundedBuffer;
        public override void OnUpdateInEditor()
        {
            if (FlipGame.World != null && InFrame)
            {
                Utils.DrawToMap("LightingOcclusionMap", (SpriteBatch sb) => sb.Draw(TileManager.tileDict[type], new Rectangle(Position.ToPoint(), new Point(Width, Height)), frame, Color.White));
                drawData = new DrawData(TileManager.tileDict[type], new Rectangle(Position.ToPoint(), new Point(Width, Height)), frame, Color.White);
                if (!Surrounded && SurroundedBuffer)
                {
                    Polygon CollisionPoly = Framing.GetPolygon(FlipGame.World, i, j);
                    AddModule("Collision", new Collideable(this, true, CollisionPoly, CollisionPoly.IsRectangle ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                if (Surrounded && !SurroundedBuffer)
                {
                    Chunk.Colliedables.RemoveThroughEntity(this);
                }
                SurroundedBuffer = Surrounded;
            }
        }
        public DrawData drawData { get; set; }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipGame.World != null)
            {
                if (InFrame && Active)
                {
                    spriteBatch.Draw(TileManager.tileDict[type], new Rectangle(Position.ToPoint(), new Point(Width, Height)), frame, Color.White);
                    return;
                }
            }
            drawData = DrawData.Null;
        }
        public override void Dispose()
        {
            Active = false;

            FlipGame.layerHandler.Layers[Layer].Drawables.Remove(this);
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
            binaryWriter.Write(Layer);
            binaryWriter.Write(frame);
        }
        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);

            int type = binaryReader.ReadInt32();
            int x = binaryReader.ReadInt32();
            int y = binaryReader.ReadInt32();
            int layer = binaryReader.ReadInt32();
            Rectangle R = binaryReader.ReadRect();
            Tile tile = new Tile(type, R, new Vector2(x, y), layer);
            return FlipGame.tileManager.AddTile(FlipGame.World, tile);
        }

        protected override void PostConstructor()
        {
            if (FlipGame.World.IsTileActive(i, j))
            {
                if (TileManager.CanPlace && FlipGame.tileManager.GetTile(i, j) != null)
                {
                    Polygon CollisionPoly = Framing.GetPolygon(FlipGame.World, i, j);
                    Logger.NewText(CollisionPoly.IsRectangle);
                    AddModule("Collision", new Collideable(this, true, CollisionPoly, CollisionPoly.IsRectangle ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
            }
        }
        public Tile(int type, Rectangle frame, Vector2 pos, int layer = -1) : base()
        {
            Width = 32;
            Height = 32;
            this.type = type;
            this.frame = frame;
            Active = true;
            Position = pos * 32;
            if (layer == -1)
                Layer = LayerHandler.CurrentLayer;
            else
                Layer = layer;
            i = (int)(ParallaxPosition.X / 32);
            j = (int)(ParallaxPosition.Y / 32);
        }
        public Tile(int type, Rectangle frame) : base()
        {
            this.type = type;
            this.frame = frame;
            Active = false;
            Layer = LayerHandler.CurrentLayer;
        }
        public Tile() : base()
        {
            Active = false;
        }
    }
}
