using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.GUI.HUD;
using Flipsider.Scenes;
using Flipsider.Engine.Particles;
using Flipsider.Engine;
using Flipsider.Engine.Audio;
using Flipsider.Engine.Input;
using Flipsider.GUI.TilePlacementGUI;
using System.Reflection;
using System.Linq;
using System.Threading;
using Flipsider.Weapons;
using Flipsider.Engine.Interfaces;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable
    internal partial class Main : Game
    {
        public static void AppendToLayer(ILayeredComponent ilc) => renderer.layerHandler.AppendMethodToLayer(ilc);
        public static void AutoAppendToLayer(ILayeredComponent ilc) => renderer.layerHandler.AutoAppendMethodToLayer(ref ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => renderer.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => renderer.layerHandler;
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => mainCamera.targetScale;
        public static TileManager tileManager => CurrentWorld.tileManager;
        public static SpriteBatch spriteBatch => renderer.spriteBatch;
        public static Player player => CurrentWorld.MainPlayer;
        public static GraphicsDeviceManager graphics => renderer.graphics;
        public static Camera mainCamera => renderer.mainCamera;
        public static Lighting lighting => renderer.lighting;
        public static List<Entity> entities => CurrentWorld.entityManager.Components;
        public static List<Water> WaterBodies => CurrentWorld.WaterBodies.Components;
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / TileManager.tileRes, MouseScreen.Y / TileManager.tileRes);
        public static float ScreenScale => renderer.mainCamera.scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : renderer.PreferredSize;
        public static Vector2 AScreenSize;
        public static Vector2 ActualScreenSize => AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => ActualScreenSize - ScreenSize;

        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);

        public static Scene CurrentScene => instance.sceneManager.Scene;
    }
}
