using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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

        public Vector2 Offset { get; set; }

        AABBCollisionSet? CollisionSet { get; set; }
        List<Collideable> CollideablesFromSet { get; set; }
        public List<Collideable> GetCollideables()
        {
            if (CollisionSet != null && BindableEntity != null)
            {
                List<Collideable> col = new List<Collideable>();

                foreach (RectangleF rF in CollisionSet.AABBs)
                {
                    Rectangle colFrame = BindableEntity.CollisionFrame;

                    RectangleF processedRectangle = new RectangleF(
                        BindableEntity.Position.X + colFrame.Width * rF.x,
                        BindableEntity.Position.Y + colFrame.Height * rF.y,
                        BindableEntity.Size.X * rF.w,
                        BindableEntity.Size.Y * rF.h);
                    col.Add(new Collideable(BindableEntity, isStatic, processedRectangle.ToPolygon()));
                }

                return col;
            }
            return new List<Collideable>();
        }
        public void Update(in Entity entity)
        {
            if (BindableEntity != null)
            {
                if (!isStatic) Logger.NewText(Offset);

                if (CollisionSet == null)
                {
                    Polygon.Center = BindableEntity.Center + Offset;
                }
                else
                {
                    for (int i = 0; i < CollideablesFromSet.Count; i++)
                    {
                        Entity? cEntity = CollideablesFromSet[i].BindableEntity;
                        if (cEntity != null) CollideablesFromSet[i].Polygon.Center = cEntity.Center + CollideablesFromSet[i].Offset;
                    }
                }

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
                                        if (collideable2.PolyType == PolyType.ConvexPoly)
                                        {
                                            RectVPoly(this, collideable2);
                                        }
                                    }
                                }
                            }
                            foreach (Collideable collideable2 in chunk.Colliedables.collideables)
                            {
                                if (collideable2.BindableEntity != null)
                                {
                                    if (!LivingEntity.onSlope && collideable2.BindableEntity.InFrame)
                                    {
                                        if (collideable2.CollisionSet == null)
                                        {
                                            if (collideable2.PolyType == PolyType.Rectangle)
                                            {
                                                RectVRect(this, collideable2);
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < collideable2.CollideablesFromSet.Count; i++)
                                            {
                                                RectVRect(collideable2.CollideablesFromSet[i], collideable2);
                                            }
                                        }
                                    }
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

            if (BindableEntity != null)
            {
                BindableEntity.Chunk.Colliedables.collideables.Remove(this);

                foreach (Collideable c in CollideablesFromSet)
                {
                    if (BindableEntity.Chunk.Colliedables.collideables.Contains(c))
                        BindableEntity.Chunk.Colliedables.collideables.Remove(c);
                }
            }

            if (CollideablesFromSet.Count > 0) CollideablesFromSet.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Polygon.Draw(PolyType == PolyType.Rectangle ? Color.Green : Color.Purple);
        }

        public int Layer { get; set; }

        public Collideable(Entity? entity, bool isStatic, Polygon polygon, PolyType polyType = default, AABBCollisionSet? colSet = null)
        {
            BindableEntity = entity;
            this.isStatic = isStatic;
            Polygon = polygon;
            PolyType = polyType == default ? PolyType.Rectangle : polyType;
            entity?.Chunk.Colliedables.collideables.Add(this);

            if (BindableEntity != null) Offset = polygon.Center - BindableEntity.Center;

            CollisionSet = colSet;
            CollideablesFromSet = GetCollideables();

            FlipGame.AppendToLayer(this);
        }
    }
}
