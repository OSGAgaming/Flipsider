using Flipsider.Content.IO.Graphics;
using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.GUI;
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
        public static void AppendToLayer(ILayeredComponent ilc) => World.layerHandler.AppendMethodToLayer(ilc);
        public static void AutoAppendToLayer(ILayeredComponent ilc) => World.layerHandler.AutoAppendMethodToLayer(ref ilc);
        public static void AppendPrimitiveToLayer(ILayeredComponent ilc) => World.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => World.layerHandler;
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => Camera.targetScale;
        public static TileManager tileManager => World.tileManager;
        public static SpriteBatch spriteBatch => renderer.SpriteBatch;
        public static Player player => World.MainPlayer;
        public static GraphicsDeviceManager graphics => renderer.Graphics;
        public static GameCamera Camera => renderer.MainCamera;
        public static Lighting lighting => renderer.Lighting;
        public static List<Water> WaterBodies => World.WaterBodies.Components;
        public static Vector2 MouseTile => new Vector2(MouseToDestination().X / TileManager.tileRes, MouseToDestination().Y / TileManager.tileRes);
        public static float ScreenScale => renderer.MainCamera.Scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : renderer.PreferredSize;

        public static Vector2 AScreenSize;
        public static Vector2 ActualScreenSize => AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Point MouseToDestination()
        {
            Point p = Mouse.GetState().Position;
            Vector2 R = ActualScreenSize / renderer.Destination.Size.ToVector2();
            Vector2 v = (p.ToVector2() - renderer.Destination.Location.ToVector2() ) * R / Camera.Scale + Camera.Position;
            return v.ToPoint();
        }

        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);

        public static Scene CurrentScene => instance.sceneManager.Scene;
    }
}
