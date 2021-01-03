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
        public SceneManager sceneManager;
        public static Random rand;
        public static Main instance;
        //Terraria PTSD
        public static GameTime gameTime;
        public static SpriteFont font;
        public static Renderer renderer;
        public static World CurrentWorld;
        private ParticleSystem TestParticleSystem;

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
            Type[] NPCTypes = ReflectionHelpers.GetInheritedClasses(typeof(NPC));

            NPC.NPCTypes = new NPC.NPCInfo[NPCTypes.Length];
            for (int i = 0; i < NPCTypes.Length; i++)
                NPC.NPCTypes[i].type = NPCTypes[i];

            Type[] StoreableTypes = ReflectionHelpers.GetInheritedClasses(typeof(IStoreable));

            Item.ItemTypes = new Type[StoreableTypes.Length];
            for (int i = 0; i < StoreableTypes.Length; i++)
                Item.ItemTypes[i] = StoreableTypes[i];
        }

        private void Instatiate()
        {
            GetAllTypes();
            sceneManager = new SceneManager();
            sceneManager.SetNextScene(new DebugScene(), null);
            rand = new Random();

        }
        protected override void Initialize()
        {
            TextureCache.LoadTextures(Content);
            CurrentWorld = new World(1000, 1000);
            CurrentWorld.AppendPlayer(new Player(new Vector2(100, 100)));
            Instatiate();
            // Register controls
            RegisterControls.Invoke();
            mainCamera.targetScale = 1.2f;
            //  NPC.SpawnNPC<Blob>(player.position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderer.Load();
            font = Content.Load<SpriteFont>("FlipFont");
            #region testparticles
            TestParticleSystem = new ParticleSystem(200);
            #endregion
            instance = this;
            PropManager.LoadProps();
            LoadGUI();
        }

        private void LoadGUI()
        {
            foreach (Type type in ReflectionHelpers.GetInheritedClasses(typeof(UIScreen)))
            {
                Activator.CreateInstance(type);
            }
        }

        public static List<IUpdate> Updateables = new List<IUpdate>();

        protected override void Update(GameTime gameTime)
        {
            Main.gameTime = gameTime;
            foreach (IUpdate updateable in Updateables.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
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
