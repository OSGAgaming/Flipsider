using Flipsider.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using static Flipsider.TileManager;

namespace Flipsider.GUI
{
    internal class ActiveModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(20, 32);

        public override Point PreivewDimensions => new Point(160, Main.renderer.Destination.Height);

        public override Color Color => Color.Black;

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            if(EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode)) EditorModeGUI.GetActiveScreen().DrawToSelector(sb);
        }

        protected override void OnUpdate()
        {
            if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode))
            {
                PreviewAlpha = PreviewAlpha.ReciprocateTo(1);
                if(Active)
                {
                    PreviewHeight = EditorModeGUI.GetActiveScreen().PreviewHeight;
                }
            }
            else
            {
                PreviewAlpha = PreviewAlpha.ReciprocateTo(0);
            }
        }
    }

    internal class BottomModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(Main.renderer.Destination.Left, Main.renderer.Destination.Bottom);

        public override Point PreivewDimensions => new Point(Main.renderer.Destination.Width, (int)Main.ActualScreenSize.Y - Main.renderer.Destination.Bottom);

        public override Color Color => new Color(20, 20, 20);

        public static LayerScreen? Screen;

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            if(Screen != null)
            PreviewHeight = Screen.PreviewHeight;

            Screen?.DrawToBottomPanel(sb);
            if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode)) EditorModeGUI.GetActiveScreen().DrawToBottomPanel(sb);
        }
    }
}


