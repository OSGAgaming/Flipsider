
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    // TODO holy shit this hurts
#nullable disable
    internal partial class Main : Game
    {
        public static Main instance;
        public static bool isLoading = true;
        public static Renderer Renderer { get; set; }
        public static World World { get; set; }

        public Main()
        {
            Renderer = new Renderer(this);
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Utils.SaveCurrentWorldAs("CurrentWorld");
        }
        private void Instatiate()
        {
            GetAllTypes();
            SceneManager.Instance.SetNextScene(new EditorScene(), null);
        }
        public static string MainPath => Environment.CurrentDirectory + $@"\";
        protected override void Initialize()
        {
            AutoloadTextures.GetAllAssetPaths(Utils.AssetDirectory);

            AScreenSize = graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();

            //Obsolete and needs to be replaced
            TextureCache.LoadTextures(Content);
            EffectCache.LoadEffects(Content);
            Fonts.LoadFonts(Content);
            AutoloadTextures.LoadTexturesToAssetCache(Content);
            Textures.LoadTextures();
            FlipE.Load(Content);
            Instatiate();

            // Register controls
            Camera.targetScale = 2f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Load();
            World = new World(2000, 2000);
            instance = this;
            LoadGUI();

            foreach (Type type in Utils.GetInheritedClasses(typeof(IUpdateGT)))
            {
                IUpdateGT Screen = (IUpdateGT)Activator.CreateInstance(type);
                GameTimeUpdateables.Add(Screen);
            }

            isLoading = false;

            TileManager.CanPlace = true;

            World.RetreiveLevelInfo("CurrentWorld.flip");

        }
        private void LoadGUI()
        {
            foreach (Type type in Utils.GetInheritedClasses(typeof(UIScreen)))
            {
                UIScreen Screen = (UIScreen)Activator.CreateInstance(type);
                if (Screen is ModeScreen s)
                {
                    if (s.Mode != Mode.None)
                        EditorModeGUI.AddScreen(s);
                    else BottomModeSelectPreview.Screen = s as LayerScreen;
                }
            }
        }
        public static List<IUpdate> Updateables = new List<IUpdate>();
        public static List<IUpdateGT> GameTimeUpdateables = new List<IUpdateGT>();
        public static List<IUpdate> AlwaysUpdate = new List<IUpdate>();

        public static event Action LoadQueue;
        protected override void Update(GameTime gameTime)
        {
            AScreenSize =
                graphics.GraphicsDevice == null ? Vector2.One :
                graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();

            FlipE.Update(gameTime);

            foreach (IUpdateGT gt in GameTimeUpdateables.ToArray()) gt.Update(gameTime);
            foreach (IUpdate gt in Updateables.ToArray()) gt.Update();
            foreach (IUpdate gt in AlwaysUpdate.ToArray()) gt.Update();

            LoadQueue?.Invoke();
            LoadQueue = null;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.Draw();
            base.Draw(gameTime);
        }
    }
}
