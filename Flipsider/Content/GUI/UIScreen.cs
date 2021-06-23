using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flipsider.GUI
{
    public class UIScreen : IComponent
    {
        public List<UIElement> elements = new List<UIElement>();
        public bool active;

        public void AddElement(UIElement ui)
        {
            elements.Add(ui);
            ui.Parent = this;
        }
        public UIScreen()
        {
            UIScreenManager.Instance?.AddComponent(this);
            OnLoad();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            OnDraw();
            if (active)
            {
                foreach (UIElement element in elements)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        public void DrawToScreenDirect(SpriteBatch spriteBatch)
        {
            OnDrawToScreenDirect();
            if (active)
            {
                foreach (UIElement element in elements)
                {
                    element.DrawOnScreenDirect(spriteBatch);
                }
            }
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnDraw() { }
        internal virtual void DrawToScreen() { }
        internal virtual void OnDrawToScreenDirect() { }

        protected virtual void OnLoad() { }

        public void Update()
        {
            if (active)
            {
                OnUpdate();
                foreach (UIElement element in elements.ToArray())
                {
                    element.Update();
                }
            }
        }
    }
}
