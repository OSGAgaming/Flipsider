using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace FlipEngine
{
    public enum PolyType
    {
        None,
        Rectangle,
        Triangle,
        ConvexPoly,
        Line
    }
    public partial class Collideable : IEntityModifier,ILayeredComponent
    {
        public Polygon CustomPolyCollide = Polygon.Null;
        public Entity BindableEntity;
        public bool HasBindableEntity;
        public bool OnSlope;
        public PolyType PolyType;
        public bool Collides;
        public CollisionInfo collisionInfo;
        public RectangleF CustomHitBox;

        private Rectangle r => HasBindableEntity ? BindableEntity.CollisionFrame : CustomHitBox.ToR();
        public Polygon collisionBox => CustomPolyCollide.Center == Vector2.Zero ? r.ToPolygon() : CustomPolyCollide;
        public Polygon lastCollisionBox => BindableEntity.PreCollisionFrame.ToPolygon();

        public bool isStatic;
        internal void BindEntityToCollideable(Entity entity) =>
            BindableEntity = entity;

        public void Update(in Entity entity)
        {
            if (!entity.Active && HasBindableEntity)
            {
                entity.Chunk.Colliedables.collideables.Remove(this);
            }
            if (!isStatic && entity is LivingEntity)
            {
                LivingEntity LivingEntity = (LivingEntity)entity;
                LivingEntity.onSlope = false;
                LivingEntity.onGround = false;
                LivingEntity.isColliding = false;

                foreach (Chunk chunk in FlipGame.GetActiveChunks())
                {
                    if(chunk.Active)
                    {
                        foreach (Collideable collideable2 in chunk.Colliedables.collideables)
                        {
                            if (collideable2.BindableEntity.InFrame)
                            {
                                if (collideable2.PolyType == PolyType.ConvexPoly && PolyType == PolyType.Rectangle)
                                {
                                    RectVPoly(this, collideable2);
                                }
                            }
                        }
                        foreach (Collideable collideable2 in chunk.Colliedables.collideables)
                        {
                            if (collideable2.BindableEntity.InFrame)
                            {
                                if (collideable2.PolyType == PolyType.Rectangle && PolyType == PolyType.Rectangle && !LivingEntity.onSlope)
                                {
                                    if (collideable2.r.Intersects(new Rectangle(r.Location - new Point(50, 50), r.Size + new Point(100, 100))))
                                        RectVRect(this, collideable2);
                                }
                            }
                        }
                    }
                }

            }
        }
        public void Dispose()
        {
            BindableEntity.Chunk.Colliedables.collideables.Remove(this);
            FlipGame.layerHandler.Layers[Layer].Drawables.Remove(this);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //collisionBox.Draw();
        }
        public int Layer { get; set; }
        public Collideable(Entity entity, bool isStatic, bool HasBindableEntity = true, RectangleF frame = default, PolyType polyType = default)
        {
            CustomHitBox = frame;
            this.HasBindableEntity = HasBindableEntity;
            BindableEntity = entity;
            this.isStatic = isStatic;
            PolyType = polyType == default ? PolyType.Rectangle : polyType;
            entity.Chunk.Colliedables.collideables.Add(this);
            FlipGame.AppendToLayer(this);
        }
        public Collideable(Entity entity, bool isStatic,Polygon polygon, bool HasBindableEntity = true, RectangleF frame = default, PolyType polyType = default)
        {
            CustomHitBox = frame;
            this.HasBindableEntity = HasBindableEntity;
            BindableEntity = entity;
            this.isStatic = isStatic;
            CustomPolyCollide = polygon;
            PolyType = polyType == default ? PolyType.Rectangle : polyType;
            entity.Chunk.Colliedables.collideables.Add(this);
            FlipGame.AppendToLayer(this);
        }
    }
}
