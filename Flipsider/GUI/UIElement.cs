using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.GUI
{
    class UIElement
    {
        public Rectangle dimensions;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void OnUpdate() { }



        public void SetDimensions(int x, int y, int width, int height)
        {
            dimensions = new Rectangle(x, y, width, height);
        }

        public void SetDimensions(float x, float y, int width, int height)
        {
            dimensions = new Rectangle((int)(x * Main.screenSize.X), (int)(y * Main.screenSize.Y), width, height);
        }
    }
}
