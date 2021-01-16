using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Flipsider
{
    public class Utils
    {
        public static readonly int BOTTOM = 3000;
        public static void AppendToLayer(ILayeredComponent ilc) => Main.renderer.layerHandler.AppendMethodToLayer(ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => Main.renderer.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => Main.renderer.layerHandler;
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => Main.mainCamera.targetScale;
        public static TileManager tileManager => Main.CurrentWorld.tileManager;
        public static SpriteBatch spriteBatch => Main.renderer.spriteBatch;
        public static Player? player => Main.CurrentWorld.MainPlayer;
        public static GraphicsDeviceManager? graphics => Main.renderer.graphics;
        public static Camera? mainCamera => Main.renderer.mainCamera;
        public static Lighting? lighting => Main.renderer.lighting;
        public static List<Entity> entities => Main.CurrentWorld.entityManager.Components;
        public static List<Water> WaterBodies => Main.CurrentWorld.WaterBodies.Components;
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / TileManager.tileRes, MouseScreen.Y / TileManager.tileRes);
        //public static float ScreenScale => Main.renderer.mainCamera.scale;
        public static Vector2 ScreenSize => Main.graphics.GraphicsDevice == null ? Vector2.One : Main.renderer.PreferredSize;

        public static Vector2 AScreenSize;
        public static Vector2 ActualScreenSize => AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => Main.ActualScreenSize - Main.ScreenSize;

        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);

        public static Scene? CurrentScene => Main.instance.sceneManager.Scene;
    }
}
