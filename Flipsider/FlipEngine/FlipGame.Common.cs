using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlipEngine
{
    // TODO holy shit this hurts
#nullable disable
    public partial class FlipGame : Game
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
                PropEntity.PropEntities.Add(PE.Prop, PE);
            }
        }
        public static void AppendToLayer(ILayeredComponent ilc) => World.layerHandler.AppendMethodToLayer(ilc);
        public static void AutoAppendToLayer(ILayeredComponent ilc) => World.layerHandler.AutoAppendMethodToLayer(ref ilc);
        public static void AppendPrimitiveToLayer(IPrimitiveLayeredComponent ilc) => World.layerHandler.AppendPrimitiveToLayer(ilc);
        public static LayerHandler layerHandler => World.layerHandler;
        public static TileManager tileManager => World.tileManager;
        public static SpriteBatch spriteBatch => Renderer.SpriteBatch;
        public static GraphicsDeviceManager graphics => Renderer.Graphics;
        public static CameraTransform Camera => Renderer.MainCamera;
        public static Lighting lighting => Renderer.Lighting;
        public static Vector2 MouseTile => (MouseToDestination().ToVector2() / TileManager.tileRes).Snap(1);
        public static float ScreenScale => Renderer.MainCamera.Scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : Renderer.PreferredSize;
        public static Chunk[] GetActiveChunks()
        {
            Point TL = TileManager.ToChunkCoords(Camera.Position.ToPoint());
            Point BR = TileManager.ToChunkCoords((Camera.Position + ActualScreenSize / ScreenScale).ToPoint());

            List<Chunk> ChunkBuffer = new List<Chunk>();
            for(int i = TL.X; i <= BR.X; i++)
            {
                for (int j = TL.Y; j <= BR.Y; j++)
                {
                    if(i >= 0 && j >= 0 && 
                       i < tileManager.chunks.GetLength(0) && 
                       j < tileManager.chunks.GetLength(1)) 
                        ChunkBuffer.Add(tileManager.GetChunk(new Point(i,j)));
                }
            }

            return ChunkBuffer.ToArray();
        }


        public static Vector2 AScreenSize;
        public static Vector2 ActualScreenSize => AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Point MouseToDestination()
        {
            Vector2 R = ActualScreenSize / Renderer.Destination.Size.ToVector2();
            Vector2 v = (Mouse.GetState().Position.ToVector2() - Renderer.Destination.Location.ToVector2() ) * R / Camera.Scale + Camera.TransformPosition;
            return v.ToPoint();
        }

        public static Point PreviousMouseToDestination()
        {
            Vector2 R = ActualScreenSize / Renderer.Destination.Size.ToVector2();
            Vector2 v = (GameInput.Instance.PreviousMouseState.Position.ToVector2() - Renderer.Destination.Location.ToVector2()) * R / Camera.Scale + Camera.TransformPosition;
            return v.ToPoint();
        }

        public static Vector2 ScreenCenterUI => ActualScreenSize / 2;

        public static Scene CurrentScene => SceneManager.Instance.Scene;
    }
}
