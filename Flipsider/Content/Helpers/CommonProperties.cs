using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable
    internal partial class Main : Game
    {
        private void GetAllTypes()
        {
            Type[] NPCTypes = Utils.GetInheritedClasses(typeof(NPC));

            NPC.NPCTypes = new NPC.NPCInfo[NPCTypes.Length];
            for (int i = 0; i < NPCTypes.Length; i++)
                NPC.NPCTypes[i].type = NPCTypes[i];

            Type[] StoreableTypes = Utils.GetInheritedClasses(typeof(IStoreable));

            Item.ItemTypes = new Type[StoreableTypes.Length];
            for (int i = 0; i < StoreableTypes.Length; i++)
                Item.ItemTypes[i] = StoreableTypes[i];

            Type[] PropEntityTypes = Utils.GetInheritedClasses(typeof(PropEntity));

            for (int i = 0; i < PropEntityTypes.Length; i++)
            {
                PropEntity PE = (PropEntity)Activator.CreateInstance(PropEntityTypes[i]);
                PropEntity.keyValuePairs.Add(PE.Prop, PE);
            }
        }
        public static void AppendToLayer(ILayeredComponent ilc) => CurrentWorld.layerHandler.AppendMethodToLayer(ilc);
        public static void AutoAppendToLayer(ILayeredComponent ilc) => CurrentWorld.layerHandler.AutoAppendMethodToLayer(ref ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => CurrentWorld.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => CurrentWorld.layerHandler;
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => mainCamera.targetScale;
        public static TileManager tileManager => CurrentWorld.tileManager;
        public static SpriteBatch spriteBatch => renderer.spriteBatch;
        public static Player player => CurrentWorld.MainPlayer;
        public static GraphicsDeviceManager graphics => renderer.graphics;
        public static Camera mainCamera => renderer.mainCamera;
        public static Lighting lighting => renderer.lighting;
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
