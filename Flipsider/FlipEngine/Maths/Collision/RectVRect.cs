
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace FlipEngine
{
    public partial class Collideable : IEntityModifier
    {
        public void RectVRect(Collideable collideable1, Collideable collideable2)
        {
            if (collideable1.BindableEntity != null)
            {
                if ((collideable2.isStatic) || collideable2.BindableEntity == null)
                {

                    CollisionInfo CI =
                        Collision.AABBResolvePolyOut(
                        collideable1.Polygon,
                        collideable1.lastCollisionBox,
                        collideable2.Polygon);

                    collideable1.collisionInfo.AABB = CI.AABB;
                    collideable1.BindableEntity.Position += CI.d;
                    if (collideable1.BindableEntity is LivingEntity)
                    {
                        var LivingEntity = (LivingEntity)collideable1.BindableEntity;
                        if (CI.AABB != Bound.None)
                        {
                            switch (collideable1.collisionInfo.AABB)
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

        public void RectVPoly(Collideable collideable1, Collideable collideable2)
        {
            if (collideable1.BindableEntity != null)
            {
                if ((collideable2.isStatic) || collideable2.BindableEntity == null)
                {
                    if (collideable1.BindableEntity is LivingEntity)
                    {
                        var LivingEntity = (LivingEntity)collideable1.BindableEntity;

                        CollisionInfo CI =
                            Collision.Raycast(
                            collideable1.Polygon,
                            collideable2.Polygon, 100, collideable1.BindableEntity.Height / 2);

                        collideable1.collisionInfo.AABB = CI.AABB;

                        if (CI.AABB != Bound.None)
                        {
                            switch (collideable1.collisionInfo.AABB)
                            {
                                case (Bound.Top):
                                    {
                                        LivingEntity.onGround = true;
                                        //LivingEntity.onSlope = true;
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
                            collideable1.BindableEntity.Position += CI.d;
                            LivingEntity.isColliding = true;
                        }
                    }
                }
            }
        }
    }
}
