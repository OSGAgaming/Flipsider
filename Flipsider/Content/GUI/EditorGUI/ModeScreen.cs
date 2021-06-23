using Flipsider.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    //Hello Im doing work. THis is work. I am working. This is a work thingie. Give me raise lmfao xd
    internal class ModeScreen : UIScreen
    {
        public virtual Mode Mode { get; }

        public virtual int PreviewHeight { get; }

        public virtual void DrawToSelector(SpriteBatch sb) { }

        public virtual void CustomDrawToScreen() { }

        public virtual void CustomUpdate() { }

        internal override void DrawToScreen()
        {
            if(EditorModeGUI.mode == Mode) CustomDrawToScreen();
        }

        protected override void OnUpdate()
        {
            if(EditorModeGUI.mode == Mode) CustomUpdate();
        }
        internal override void OnDrawToScreenDirect() { }
    }

    internal class PreviewElement : UIElement
    {
        public ActiveModeSelectPreview? Parent => EditorModeGUI.B;

        protected Rectangle RelativeDimensions { get; set; }

        protected virtual void CustomUpdate() { }
        protected override void OnUpdate()
        {
            if (Parent != null)
            {
                Point p = Parent.dimensions.Location;
                Point Position = new Point(p.X + RelativeDimensions.X, p.Y + RelativeDimensions.Y - (int)Parent.ScrollValue);

                dimensions = new Rectangle(Position, RelativeDimensions.Size);
            }
            CustomUpdate();
        }
    }
}


