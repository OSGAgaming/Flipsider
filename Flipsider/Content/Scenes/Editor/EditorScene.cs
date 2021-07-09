
using Flipsider.Engine;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Scenes
{
    public class EditorScene : Scene
    {
        public override string? Name => "Editor";
        public static Scene? DisplayScene { get; set; }
        public EditorScene(Scene Display)
        {
            DisplayScene = Display;
        }

        public EditorScene() { }

        public override void Update()
        {
            int XToLeft = 200;
            int YToLeft = 32;

            int XToRight = XToLeft + 32;
            int YToRight = (int)(XToRight * (Main.ActualScreenSize.Y / Main.ActualScreenSize.X));

            Main.renderer.Destination = new Rectangle(XToLeft, YToLeft, (int)Main.ActualScreenSize.X - XToRight, (int)Main.ActualScreenSize.Y - YToRight);

            DisplayScene?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DisplayScene?.Draw(spriteBatch);
        }
    }
}
