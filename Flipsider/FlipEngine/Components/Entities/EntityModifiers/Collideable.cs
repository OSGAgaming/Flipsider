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
    public partial class Collideable : IEntityModifier, ILayeredComponent
    {
        public Polygon Polygon = Polygon.Null;
        public Entity? BindableEntity;
        public PolyType PolyType;
        public CollisionInfo collisionInfo;

        public Polygon lastCollisionBox => BindableEntity?.PreCollisionFrame.ToPolygon() ?? Polygon.Null;

        public bool isStatic;

        public Vector2 StartPointEntity;
        public Vector2 StartPointCollideable;

        public Vector2 DeltaStart => (BindableEntity?.Center ?? Vector2.Zero) - StartPointEntity;
        public Vector2 NewCollideablePoint => StartPointCollideable + DeltaStart;

        public void Update(in Entity entity)
        {
            if (!entity.Active && BindableEntity != null)
            {
                entity.Chunk.Colliedables.collideables.Remove(this);
            }

            if (BindableEntity != null)
            {
                Polygon.Center = NewCollideablePoint;

                if (!isStatic && entity is LivingEntity)
                {
                    LivingEntity LivingEntity = (LivingEntity)entity;
                    LivingEntity.onSlope = false;
                    LivingEntity.onGround = false;
                    LivingEntity.isColliding = false;

                    foreach (Chunk chunk in FlipGame.GetActiveChunks())
                    {
                        if (chunk.Active)
                        {
                            foreach (Collideable collideable2 in chunk.Colliedables.collideables)
                            {
                                if (collideable2.BindableEntity != null)
                                {
                                    if (collideable2.BindableEntity.InFrame)
                                    {
                                        if (collideable2.PolyType == PolyType.ConvexPoly && PolyType == PolyType.Rectangle)
                                        {
                                            RectVPoly(this, collideable2);
                                        }
                                    }
                                }
                            }
                            foreach (Collideable collideable2 in chunk.Colliedables.collideables)
                            {
                                if (collideable2.PolyType == PolyType.Rectangle && PolyType == PolyType.Rectangle)
                                {
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
            BindableEntity?.Chunk.Colliedables.collideables.Remove(this);
            FlipGame.layerHandler.Layers[Layer].Drawables.Remove(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Polygon.Draw();
        }

        public int Layer { get; set; }

        public Collideable(Entity? entity, bool isStatic, Polygon polygon, PolyType polyType = default)
        {
            BindableEntity = entity;
            this.isStatic = isStatic;
            Polygon = polygon;
            PolyType = polyType == default ? PolyType.Rectangle : polyType;
            entity?.Chunk.Colliedables.collideables.Add(this);

            if (BindableEntity != null) StartPointEntity = BindableEntity.Center;
            StartPointCollideable = polygon.Center;

            FlipGame.AppendToLayer(this);
        }
    }
}
