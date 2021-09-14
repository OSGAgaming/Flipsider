
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    // TODO holy shit this hurts
#nullable disable
    public partial class FlipGame : Game
    {
        public static Renderer Renderer { get; set; }
        public static World World { get; set; }

        public static FlipGame Instance;
        public static bool isLoading = true;

        protected virtual void OnLoadContent() { }
        protected virtual void OnInitialize() { }
        protected virtual void LoadContentEnd() { }
        protected virtual void InitializeEnd() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }

        public static string MainPath => Environment.CurrentDirectory + $@"\";

        public FlipGame()
        {
            Renderer = new Renderer(this);
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        protected override void OnExiting(object sender, EventArgs args) => Utils.SaveCurrentWorldAs("CurrentWorld");
        
        private void Instatiate()
        {
            GetAllTypes();
        }
        protected override void Initialize()
        {
            OnInitialize();
            AScreenSize = graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();

            FlipE.Load(Content);
            Instatiate();

            InitializeEnd();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            OnLoadContent();
            Renderer.Load();

            Instance = this;
            isLoading = false;

            World.RetreiveLevelInfo("CurrentWorld.flip");
            LoadContentEnd();
        }

        protected override void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);

            AScreenSize =
                graphics.GraphicsDevice == null ? Vector2.One :
                graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();

            FlipE.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            OnDraw(gameTime);
            Renderer.Draw();
        }
    }
}
