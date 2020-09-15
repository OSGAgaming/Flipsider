using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.GUI
{
    class UIElement
    {
        public Rectangle dimensions;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        protected virtual void OnUpdate() { }

        protected virtual void OnHover() { }

        protected virtual void OnLeftClick() { }

        protected virtual void OnRightClick() { }

        public void Update()
        {
            if (dimensions.Contains(Main.MouseScreen)) OnHover();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) OnLeftClick();
            if (Mouse.GetState().RightButton == ButtonState.Pressed) OnRightClick();
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            dimensions = new Rectangle(x, y, width, height);
        }

        public void SetDimensionsPercentage(float x, float y, int width, int height)
        {
            dimensions = new Rectangle((int)(x * Main.ScreenSize.X), (int)(y * Main.ScreenSize.Y), width, height);
        }
    }
}
