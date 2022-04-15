using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace FlipEngine
{
    public class CollideableHanlder
    {
        public List<Collideable> collideables = new List<Collideable>();
        public void RemoveThroughEntity(Entity entity)
        {
            foreach (Collideable Collideable in collideables.ToArray())
            {
                if (ReferenceEquals(entity, Collideable.BindableEntity))
                {
                    Collideable.Dispose();
                }
            }
        }
        public Collideable AddCustomHitBox(Entity? entity, bool isStatic, Polygon poly, AABBCollisionSet? set = null)
        {
            return new Collideable(entity, isStatic, poly, default, set);
        }
    }
}
