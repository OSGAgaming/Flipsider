﻿using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public interface IEntityModifier : IDisposable
    {
        public void Update(in Entity entity);
    }
    public interface IDrawableEntityModifier : IEntityModifier
    {
        public void Draw(in Entity entity, SpriteBatch sb);
    }
}
