﻿
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public enum PolyType
    {
        None,
        Rectangle,
        Triangle,
        ConvexPoly
    }
    public class Collideable : IEntityModifier, ILayeredComponent
    {
        public Entity BindableEntity;
        public bool HasBindableEntity;
        public PolyType PolyType;
        public bool Collides;
        public CollisionInfo collisionInfo;
        public RectangleF CustomHitBox;
        private Rectangle r => HasBindableEntity ? BindableEntity.CollisionFrame : CustomHitBox.ToR();
        public Polygon collisionBox => r.ToPolygon();
        public Polygon lastCollisionBox => BindableEntity.PreCollisionFrame.ToPolygon();

        public float mass;
        public bool isStatic;
        internal void BindEntityToCollideable(Entity entity) =>
            BindableEntity = entity;
        public void Update(in Entity entity)
        {
            if(!BindableEntity.Active && HasBindableEntity)
            {
                Main.Colliedables.collideables.Remove(this);
            }
            if (!isStatic && BindableEntity is LivingEntity)
            {
                LivingEntity? LivingEntity = (LivingEntity)BindableEntity;
                LivingEntity.onGround = false;
                LivingEntity.isColliding = false;
                foreach (Collideable collideable2 in Main.Colliedables.collideables)
                {

                    if (PolyType == PolyType.Rectangle &&
                       collideable2.PolyType == PolyType.Rectangle)
                    {
                        if ((collideable2.isStatic && collideable2.BindableEntity.Active) || !collideable2.HasBindableEntity)
                        {
                            CollisionInfo CI =
                                Collision.AABBResolvePoly(
                                collisionBox,
                                lastCollisionBox,
                                collideable2.collisionBox);

                            collisionInfo.AABB = CI.AABB;
                            BindableEntity.position += CI.d;
                            if (CI.AABB != Bound.None)
                            {
                                switch (collisionInfo.AABB)
                                {
                                    case (Bound.Top):
                                        {
                                            LivingEntity.onGround = true;
                                            LivingEntity.velocity.Y = 0;
                                            break;
                                        }
                                    case (Bound.Bottom):
                                        {
                                            LivingEntity.velocity.Y = 0;
                                            break;
                                        }
                                    case (Bound.Left):
                                        {
                                            LivingEntity.velocity.X = 0;
                                            break;
                                        }
                                    case (Bound.Right):
                                        {
                                            LivingEntity.velocity.X = 0;
                                            break;
                                        }
                                }
                                LivingEntity.isColliding = true;
                            }
                        }

                    }
                }
            }
        }
        public int Layer { get; set; }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public Collideable(Entity entity, bool isStatic, bool HasBindableEntity = true, RectangleF frame = default)
        {
            CustomHitBox = frame;
            this.HasBindableEntity = HasBindableEntity;
            BindableEntity = entity;
            this.isStatic = isStatic;
            PolyType = PolyType.Rectangle;
            Main.Colliedables.collideables.Add(this);
        }
    }
}
