
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;


namespace FlipEngine
{
    internal class ModeScreen : UIScreen
    {
        public virtual Mode Mode { get; }

        public virtual int PreviewHeight { get; }
        public virtual int PreviewWidth { get; }

        public virtual void DrawToSelector(SpriteBatch sb) { }

        public virtual void DrawToBottomPanel(SpriteBatch sb) { }

        public virtual void CustomDrawToScreen() { }

        public virtual void CustomUpdate() { }

        internal override void DrawToScreen()
        {
            if(EditorModeGUI.mode == Mode || Mode == Mode.None) CustomDrawToScreen();
        }

        protected override void OnUpdate()
        {
            if(EditorModeGUI.mode == Mode || Mode == Mode.None) CustomUpdate();
        }
        internal override void OnDrawToScreenDirect() { }
    }

    internal abstract class PreviewElement : UIElement
    {
        public ScrollPanel? PreviewPanel { get; set; }
        
        public Rectangle RelativeDimensions { get; set; }

        public PreviewElement(ScrollPanel? Parent)
        {
            FlipE.LoadQueue += () =>
            {
                PreviewPanel = Parent;
            };      
        }

        protected virtual void ChangeDimensions() { }
        protected virtual void CustomUpdate() { }
        protected override void OnUpdate()
        {
            if (PreviewPanel != null)
            {
                Point p = PreviewPanel.dimensions.Location;
                Point Position = new Point(p.X + RelativeDimensions.X - (int)PreviewPanel.ScrollValueX, p.Y + RelativeDimensions.Y - (int)PreviewPanel.ScrollValueY);

                ChangeDimensions();
                dimensions = new Rectangle(Position, RelativeDimensions.Size);
            }

            CustomUpdate();
        }
    }
}


