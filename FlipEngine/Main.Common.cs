using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlipEngine
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
        public static void AppendToLayer(ILayeredComponent ilc) => World.layerHandler.AppendMethodToLayer(ilc);
        public static void AutoAppendToLayer(ILayeredComponent ilc) => World.layerHandler.AutoAppendMethodToLayer(ref ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => World.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => World.layerHandler;
        public static TileManager tileManager => World.tileManager;
        public static SpriteBatch spriteBatch => Renderer.SpriteBatch;
        public static GraphicsDeviceManager graphics => Renderer.Graphics;
        public static CameraTransform Camera => Renderer.MainCamera;
        public static Lighting lighting => Renderer.Lighting;
        public static List<Water> WaterBodies => World.WaterBodies.Components;
        public static Vector2 MouseTile => MouseToDestination().ToVector2() / TileManager.tileRes;
        public static float ScreenScale => Renderer.MainCamera.Scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : Renderer.PreferredSize;

        public static Vector2 AScreenSize;
        public static Vector2 ActualScreenSize => AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Point MouseToDestination()
        {
            Point p = Mouse.GetState().Position;
            Vector2 R = ActualScreenSize / Renderer.Destination.Size.ToVector2();
            Vector2 v = (p.ToVector2() - Renderer.Destination.Location.ToVector2() ) * R / Camera.Scale + Camera.Position;
            return v.ToPoint();
        }

        public static Vector2 ScreenCenterUI => ActualScreenSize / 2;

        public static Scene CurrentScene => SceneManager.Instance.Scene;
    }
}
