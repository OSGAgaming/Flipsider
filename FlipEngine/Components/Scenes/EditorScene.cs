using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
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
            int YToRight = (int)(XToRight * (FlipGame.ActualScreenSize.Y / FlipGame.ActualScreenSize.X));

            FlipGame.Renderer.Destination = new Rectangle(XToLeft, YToLeft, (int)FlipGame.ActualScreenSize.X - XToRight, (int)FlipGame.ActualScreenSize.Y - YToRight);

            DisplayScene?.Update();
            FlipGame.World.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DisplayScene?.Draw(spriteBatch);
            FlipGame.World.Draw(spriteBatch);
        }
    }
}
