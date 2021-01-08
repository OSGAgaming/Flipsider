using System;
using System.Collections.Generic;
using System.Text;

using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Scenes
{
    public class MainMenu : Scene
    {
        public MainMenu()
        {

        }
        public override string? Name => "Main Menu";
        public override void Update()
        {
            foreach (IUpdate updateable in Main.UpdateablesOffScreen.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.renderer.RenderUI();
            Main.renderer.lighting?.DrawLightMap(Main.CurrentWorld);
            Main.renderer.RenderUI();
        }
    }
}
