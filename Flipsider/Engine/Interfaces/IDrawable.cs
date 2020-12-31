﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Flipsider.Engine.Interfaces
{
    public interface IDrawable
    {
        public void Draw(SpriteBatch spriteBatch);
    }
}