
using FlipEngine;
using FlipEngine;
using Microsoft.Xna.Framework;
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
            foreach (IUpdate updateable in FlipE.AlwaysUpdate.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
            Main.Editor.Update();

            Main.Renderer.RenderUITarget = true;
            Main.Renderer.Destination = new Rectangle(0, 0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y);
        }
    }
}
