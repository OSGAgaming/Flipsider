using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace FlipEngine
{
    public partial class Utils
    {
        public static readonly int BOTTOM = 3000;
        public static readonly int PrefferedWidth = 1980;
        public static readonly int PrefferedHeight = 1080;

        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
            Stream stream = File.OpenWrite(LocalWorldPath + Name + ".flip");
            FlipGame.World.levelInfo.Serialize(stream);
        }
        public static string WorldPath => FlipGame.MainPath + $@"Content\Worlds\";
        public static string CutscenePath => FlipGame.MainPath + $@"Content\Cutscenes\";
        public static string CollisionSetPath => LocalDirectory + $@"\Content\CollisionSets\";
        public static string LocalWorldPath => LocalDirectory + $@"\Content\Worlds\";
        public static string LocalCutscenePath => LocalDirectory + $@"Content\Cutscenes\";

        public static Vector2 SafeBoundX => new Vector2(FlipGame.Camera.TransformPosition.X, FlipGame.Camera.TransformPosition.X + FlipGame.ActualScreenSize.X / FlipGame.ScreenScale);
        public static Vector2 SafeBoundY => new Vector2(FlipGame.Camera.TransformPosition.Y, FlipGame.Camera.TransformPosition.Y + FlipGame.ActualScreenSize.Y / FlipGame.ScreenScale);
        public static Rectangle CameraBounds => new Rectangle(FlipGame.Camera.TransformPosition.ToPoint(), (FlipGame.ActualScreenSize / FlipGame.ScreenScale).ToPoint());
        public static void AppendToLayer(ILayeredComponent ilc) => FlipGame.World.layerHandler.AppendMethodToLayer(ilc);
        public static void AppendPrimitiveToLayer(IMeshComponent ilc) => FlipGame.World.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => FlipGame.World.layerHandler;
        public static TileManager tileManager => FlipGame.World.tileManager;
        public static SpriteBatch spriteBatch => FlipGame.Renderer.SpriteBatch;
        public static GraphicsDeviceManager? graphics => FlipGame.Renderer.Graphics;
        public static Lighting? lighting => FlipGame.Renderer.Lighting;
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / TileManager.tileRes, MouseScreen.Y / TileManager.tileRes);
        public static Vector2 ScreenSize => FlipGame.graphics.GraphicsDevice == null ? Vector2.One : FlipGame.Renderer.PreferredSize;
        public static Vector2 ActualScreenSize => FlipGame.AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => FlipGame.ActualScreenSize - FlipGame.ScreenSize;
        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);
        public static bool MouseInBounds => FlipGame.Renderer.Destination.Contains(Mouse.GetState().Position);

    }
}
