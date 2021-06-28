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
    internal class ActiveModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(20, 32);

        public override Point PreivewDimensions => new Point(160, Main.renderer.Destination.Height);

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            EditorModeGUI.GetActiveScreen().DrawToSelector(sb);
        }
    }
}


