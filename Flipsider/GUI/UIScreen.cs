using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.GUI
{
    class UIScreen
    {
        protected List<UIElement> elements = new List<UIElement>();
        public bool active;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                foreach (UIElement element in elements)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        protected virtual void OnUpdate() { }

        public void Update()
        {
            if (active)
            {
                OnUpdate();
                foreach (UIElement element in elements)
                {
                    element.Update();
                }
            }
        }
    }
}
