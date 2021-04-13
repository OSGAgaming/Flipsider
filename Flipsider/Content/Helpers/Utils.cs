using Flipsider.Content.IO.Graphics;
using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public partial class Utils
    {
        public static readonly int BOTTOM = 3000;
        public static readonly int PrefferedWidth = 1980;
        public static readonly int PrefferedHeight = 1080;
        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
            Stream stream = File.OpenWrite(Main.MainPath + Name + ".flip");
            Main.CurrentWorld.levelInfo.Serialize(stream);
            //Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name + ".flip");
        }
        public static void SaveCurrentWorldAsWithExtension(string Name)
        {
          //  Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name);
        }
        public static InputBinding LeftClick => GameInput.Instance["EditorPlaceTile"];
        public static Vector2 SafeBoundX => new Vector2(Main.Camera.Position.X, Main.Camera.Position.X + Main.ActualScreenSize.X / Main.ScreenScale);
        public static Vector2 SafeBoundY => new Vector2(Main.Camera.Position.Y, Main.Camera.Position.Y + Main.ActualScreenSize.Y / Main.ScreenScale);
        public static void AppendToLayer(ILayeredComponent ilc) => Main.CurrentWorld.layerHandler.AppendMethodToLayer(ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => Main.CurrentWorld.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => Main.CurrentWorld.layerHandler;
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => Main.Camera.targetScale;
        public static TileManager tileManager => Main.CurrentWorld.tileManager;
        public static SpriteBatch spriteBatch => Main.renderer.spriteBatch;
        public static Player? player => Main.CurrentWorld.MainPlayer;
        public static GraphicsDeviceManager? graphics => Main.renderer.graphics;
        public static GameCamera? mainCamera => Main.renderer.mainCamera;
        public static Lighting? lighting => Main.renderer.lighting;
        public static List<Water> WaterBodies => Main.CurrentWorld.WaterBodies.Components;
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / TileManager.tileRes, MouseScreen.Y / TileManager.tileRes);
        public static Vector2 ScreenSize => Main.graphics.GraphicsDevice == null ? Vector2.One : Main.renderer.PreferredSize;
        public static Vector2 ActualScreenSize => Main.AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => Main.ActualScreenSize - Main.ScreenSize;
        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);
        public static Scene? CurrentScene => Main.instance.sceneManager.Scene;
    }
}
