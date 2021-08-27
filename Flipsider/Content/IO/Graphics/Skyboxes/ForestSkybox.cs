using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class ForestSkybox : Skybox
    {
        public override void LoadSkyboxes()
        {
            Vector2 offset = new Vector2(0, Utils.BOTTOM - Main.ScreenSize.Y + 150);

            Layers.Add(new ParalaxLayer(@"Backgrounds\Skybox", -0.9f, 0, offset, 0.6f));
            Layers.Add(new ParalaxLayer(@"Backgrounds\ForestBackground3", -0.8f, 0, offset, 0.6f));
            Layers.Add(new ParalaxLayer(@"Backgrounds\ForestBackground2", -0.7f, 0, offset, 0.6f));
            Layers.Add(new ParalaxLayer(@"Backgrounds\ForestBackground1", -0.6f, 0, offset, 0.6f));
        }
    }
}
