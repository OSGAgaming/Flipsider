
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    public class UIScreen : IComponent
    {
        public List<UIElement> elements = new List<UIElement>();
        public bool active = true;

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
            if (active)
            {
                OnDraw();

                foreach (UIElement element in elements)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        public void DrawToScreenDirect(SpriteBatch spriteBatch)
        {
            if (active)
            {
                OnDrawToScreenDirect();

                foreach (UIElement element in elements)
                {
                    element.DrawOnScreenDirect(spriteBatch);
                }
            }
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnUpdatePassive() { }

        protected virtual void OnDraw() { }

        internal virtual void DrawToScreen() { }

        internal virtual void OnDrawToScreenDirect() { }

        protected virtual void OnLoad() { }

        public void Update()
        {
            OnUpdatePassive();

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
