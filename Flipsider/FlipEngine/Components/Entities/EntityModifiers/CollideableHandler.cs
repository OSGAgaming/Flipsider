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
        public void AddCustomHitBox(Entity? entity, bool isStatic, Polygon poly)
        {
            new Collideable(entity, isStatic, poly);
        }
        public void Update()
        {


        }
    }
}
