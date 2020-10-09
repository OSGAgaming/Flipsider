using Flipsider.Core;
using Flipsider.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider.GUI
{
    /// <summary>
    /// The default implementation for <see cref="IGuiState"/>.
    /// </summary>
    public class GuiState : IGuiState
    {
        private readonly LinkedList<GuiElement> rootElements = new LinkedList<GuiElement>();

        public void AddElement(GuiElement element)
        {
            rootElements.AddLast(element);
        }

        public void Draw(SafeSpriteBatch spriteBatch)
        {
            foreach (var item in rootElements)
            {
                try
                {
                    item.Draw(this, spriteBatch);
                }
                catch (Exception e)
                {
                    Logger.Warn(e);
                }
            }
        }

        public void Update()
        {
            foreach (var item in rootElements)
            {
                try
                {
                    item.Update(this);
                }
                catch (Exception e)
                {
                    Logger.Warn(e);
                }
            }
        }
    }
}
