﻿using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Flipsider.Engine.Maths
{
    public class CollideableHanlder
    {
        public HashSet<Collideable> collideables = new HashSet<Collideable>();
        public void RemoveThroughEntity(Entity entity)
        {
            Collideable[] Buffer = new Collideable[collideables.Count];
            collideables.CopyTo(Buffer);
            foreach (Collideable Collideable in Buffer)
            {
                if (ReferenceEquals(entity, Collideable.BindableEntity))
                {
                    Collideable.Dispose();
                }
            }
        }
        public void AddCustomHitBox(bool isStatic, bool HasBindableEntity, RectangleF frame)
        {
            new Collideable(Main.player, isStatic, HasBindableEntity, frame);
        }
        public void Update()
        {


        }
    }
}
