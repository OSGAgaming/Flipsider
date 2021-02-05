﻿using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Flipsider.GUI;
using Flipsider.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable
    internal partial class Main : Game
    {
        public SceneManager sceneManager;
        public static Random rand;
        public static Main instance;
        public static bool isLoading = true;
        public static GameTime gameTime;
        public static SpriteFont font;
        public static Renderer renderer;
        public static World CurrentWorld;
        public static PrimTrailManager Primitives;
        private PropInteraction PI;
        public FPS fps = new FPS();
        public static Serializers serializers = new Serializers();
        public Main()
        {
            renderer = new Renderer(this);
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }
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
        }

        private void Instatiate()
        {
            GetAllTypes();
            sceneManager = new SceneManager();
            sceneManager.SetNextScene(new MainMenu(), null);
            rand = new Random();
        }
        public static string MainPath => Environment.CurrentDirectory + $@"\";
        protected override void Initialize()
        {
            AScreenSize = graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            TextureCache.LoadTextures(Content);
            Instatiate();
            // Register controls
            RegisterControls.Invoke();
            mainCamera.targetScale = 1.2f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderer.Load();
            CurrentWorld = new World(200, 200);
            PI = new PropInteraction(CurrentWorld.propManager);
            CurrentWorld.AppendPlayer(new Player(new Vector2(100, Utils.BOTTOM)));
            new EntityBloom(player, player.texture, 2.1f);
        //    lighting.AddLight(player, player.texture, 4f);
            font = Content.Load<SpriteFont>("FlipFont");
            #region testparticles
            #endregion
            instance = this;
            CurrentWorld.propManager.LoadProps();
            LoadGUI();
            isLoading = false;
            Primitives = new PrimTrailManager();
        }
        private void LoadGUI()
        {
            foreach (Type type in Utils.GetInheritedClasses(typeof(UIScreen)))
            {
                Activator.CreateInstance(type);
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
            sceneManager.Update();
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
