
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FlipEngine
{
    // TODO holy shit this hurts
#nullable disable
    internal partial class Main : Game
    {
        public static Random rand;
        public static Main instance;
        public static bool isLoading = true;
        public static GameTime gameTime;
        public static SpriteFont font;
        public static Renderer renderer;
        public static World World;
        public static Manager<Primitive> Primitives;
        public FPS fps = new FPS();
        public Main()
        {
            renderer = new Renderer(this);
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
            rand = new Random();
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
            //Textures.LoadTextures();

            Instatiate();

            // Register controls
            Camera.targetScale = 2f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderer.Load();
            World = new World(2000, 2000);
            font = Content.Load<SpriteFont>("FlipFont");
            instance = this;
            LoadGUI();
            isLoading = false;
            Primitives = new Manager<Primitive>();

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
        public static List<IUpdate> UpdateablesOffScreen = new List<IUpdate>();
        public static event Action LoadQueue;
        protected override void Update(GameTime gameTime)
        {
            instance.fps.Update(gameTime);
            AScreenSize = graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            Main.gameTime = gameTime;
            SceneManager.Instance.Update();
            LoadQueue?.Invoke();
            LoadQueue = null;
            base.Update(gameTime);
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw();
            base.Draw(gameTime);
        }
    }
}
