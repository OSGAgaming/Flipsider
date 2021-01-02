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
using static Flipsider.TileManager;

using System.Reflection;
using System.Linq;
using System.Threading;
using Flipsider.Weapons;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable
    public class Main : Game
    {
        public SceneManager sceneManager;
        public static Random rand;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static Main instance;
        Water testWater2 = new Water(new Rectangle(100, 450, 100, 50));
        //Terraria PTSD
        public static Player player;
        public static IStoreable CurrentItem;
        public static RenderTarget2D renderTarget;
        public static GameTime gameTime;
        public static Camera mainCamera;
        public static SpriteFont font;
        public static float targetScale = 1;
        public static List<Entity> entities = new List<Entity>();
        public static List<UIScreen> UIScreens = new List<UIScreen>();

        public static World CurrentWorld;
        public Serializers ser;
        public static EditorMode Editor;
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / tileRes, MouseScreen.Y / tileRes);
        public static float ScreenScale => mainCamera.scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();

        private ParticleSystem TestParticleSystem;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        private Verlet verletEngine;
        void GetAllTypes()
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
        void Instatiate()
        {
            GetAllTypes();
            sceneManager = new SceneManager();
            sceneManager.SetNextScene(new DebugScene(), null);
            verletEngine = new Verlet();
            rand = new Random();
            player = new Player(new Vector2(100, 100));
            mainCamera = new Camera();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < Water.WaterBodies.Count; i++)
            {
                Water.WaterBodies[i].Initialize();
            }
            LoadTiles();
            LoadGUI();
        }
        protected override void Initialize()
        {
            TextureCache.LoadTextures(Content);
            Instatiate();
            // Register controls
            GameInput.Instance.RegisterControl("MoveLeft", Keys.A, Buttons.LeftThumbstickLeft);
            GameInput.Instance.RegisterControl("MoveRight", Keys.D, Buttons.LeftThumbstickRight);
            GameInput.Instance.RegisterControl("MoveUp", Keys.W, Buttons.LeftThumbstickUp);
            GameInput.Instance.RegisterControl("MoveDown", Keys.S, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("Jump", Keys.Space, Buttons.A);
            GameInput.Instance.RegisterControl("NPCEditor", Keys.N, Buttons.DPadUp);
            GameInput.Instance.RegisterControl("EditorPlaceTile", MouseInput.Left, Buttons.RightTrigger);
            GameInput.Instance.RegisterControl("EdtiorRemoveTile", MouseInput.Right, Buttons.RightShoulder);
            GameInput.Instance.RegisterControl("EditorSwitchModes", Keys.Z, Buttons.RightStick);
            GameInput.Instance.RegisterControl("EditorTileEditor", Keys.T, Buttons.LeftStick);
            GameInput.Instance.RegisterControl("EditorZoomIn", MouseInput.ScrollUp, Buttons.DPadUp);
            GameInput.Instance.RegisterControl("EditorZoomOut", MouseInput.ScrollDown, Buttons.DPadDown);
            GameInput.Instance.RegisterControl("WorldSaverMode", Keys.OemSemicolon, Buttons.DPadRight);
            GameInput.Instance.RegisterControl("PropEditorMode", Keys.OemPeriod, Buttons.LeftTrigger);
            GameInput.Instance.RegisterControl("LightEditorMode", Keys.L, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("InvEditorMode", Keys.I, Buttons.BigButton);
            int connectionOne = verletEngine.CreateVerletPoint(player.position + new Vector2(10, 27), true);
            int connectionTwo = verletEngine.CreateVerletPoint(player.position + new Vector2(20, 27), true);

            for (int i = 0; i < 3; i++)
            {
                verletEngine.CreateVerletPoint(player.position + new Vector2(10, i * 2 + 20));
                if (i == 0)
                {
                    verletEngine.BindPoints(verletEngine.points.Count - 1, connectionOne, true, Color.White);
                }
                else
                {
                    verletEngine.BindPoints(verletEngine.points.Count - 1, verletEngine.points.Count - 2, true, Color.White);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                verletEngine.CreateVerletPoint(player.position + new Vector2(20, i * 2 + 20));
                if (i == 0)
                {
                    verletEngine.BindPoints(verletEngine.points.Count - 1, connectionTwo, true, Color.White);
                }
                else
                {
                    verletEngine.BindPoints(verletEngine.points.Count - 1, verletEngine.points.Count - 2, true, Color.White);
                }
            }



            targetScale = 1.2f;
            //  NPC.SpawnNPC<Blob>(player.position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: Create SFX and Music bank (boffin or salv's job, based on who ends up doing the fmod studio stuff.)
            //GameAudio.Instance.LoadBank("SFX", "Audio\\SFX.bank");
            ser = new Serializers();
            Editor = new EditorMode();
            font = Content.Load<SpriteFont>("Textures/FlipFont");
            Lighting.Load(Content);
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, (int)ScreenSize.X, (int)ScreenSize.Y);
            #region testparticles
             TestParticleSystem = new ParticleSystem(200);
            /*TestParticleSystem.SpawnRate = 10f;
            //TestParticleSystem.WorldSpace = true;
            TestParticleSystem.SpawnModules.Add(new SetTexture(TextureCache.pixel));
            TestParticleSystem.SpawnModules.Add(new SetScale(5f));
            TestParticleSystem.SpawnModules.Add(new SetColorBetweenTwoColours(Color.DarkGreen, Color.Lime, Main.rand));
            TestParticleSystem.SpawnModules.Add(new SetVelocity(Vector2.UnitY * -80f));
            TestParticleSystem.SpawnModules.Add(new SetLifetime(5f));
            TestParticleSystem.UpdateModules.Add(new OpacityOverLifetime(Engine.Maths.EaseFunction.ReverseLinear));
            var condition = new ConditionalModifier(new SetScale(10f), new Turn(MathHelper.PiOver2), (Particle[] particles, int index) =>
            {
                return GameInput.Instance.MousePosition.X < Main.ScreenSize.X * 0.5f;
            });
            TestParticleSystem.UpdateModules.Add(condition);*/

            //Flame
            /*TestParticleSystem = new ParticleSystem(200);
            TestParticleSystem.SpawnRate = 10f;
            TestParticleSystem.WorldSpace = true;
            TestParticleSystem.SpawnModules.Add(new SetTexture(TextureCache.pixel));
            TestParticleSystem.SpawnModules.Add(new SetScale(5f));
            TestParticleSystem.SpawnModules.Add(new SetColorBetweenTwoColours(Color.Yellow, Color.Red, Main.rand));
            TestParticleSystem.SpawnModules.Add(new SetLifetime(5f));
            TestParticleSystem.SpawnModules.Add(new SetRandomVelocity(20f, Main.rand));
            TestParticleSystem.UpdateModules.Add(new FloatUp(0.2f, 0.99f));
            */
            #endregion
            instance = this;

        }

        void LoadGUI()
        {
            CurrentWorld = new World(1000,1000);
            foreach(Type type in ReflectionHelpers.GetInheritedClasses(typeof(UIScreen)))
            {
                Activator.CreateInstance(type);
            }
        }
 //       public static string MainPath = @$"C:\Users\{Environment.UserName}\source\repos\Flipsider\Flipsider\";


        protected override void Update(GameTime gameTime)
        {
            NPC.DTH.Update();
            Lighting.Update();
            for (int i = 0; i < Water.WaterBodies.Count; i++)
            {
                Water.WaterBodies[i].Update();
            }
            GameInput.Instance.UpdateInput();
            verletEngine.points[0].point = player.position + new Vector2((player.spriteDirection == -1 ? -8 : 0) + 18, 30) + player.velocity;
            verletEngine.points[1].point = player.position + new Vector2((player.spriteDirection == -1 ? -8 : 0) + 25, 30) + player.velocity;

            Main.gameTime = gameTime;

            sceneManager.Update();

            verletEngine.Update();

            PropInteraction.UpdatePropInteractions();

            if (!Editor.IsActive)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Update();
                }
                mainCamera.offset -= mainCamera.offset / 16f;
            }
            for(int i = 0; i< UIScreens.Count; i++)
            {
                UIScreens[i].Update();
            }
            Editor.Update();

            TestParticleSystem.Position = GameInput.Instance.MousePosition;
            TestParticleSystem.Update();

            base.Update(gameTime);
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        FPS fps = new FPS();
        protected override void Draw(GameTime gameTime)
        {
            Rectangle frame = new Rectangle(0, 0, (int)(ScreenSize.X), (int)(ScreenSize.Y));
            GraphicsDevice.Clear(Color.CornflowerBlue);

            fps.Update(gameTime);
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin(SpriteSortMode.Immediate);
            Renderer.Render();
            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: mainCamera.Transform, samplerState: SamplerState.PointClamp);
            Lighting.ApplyShader();
            verletEngine.GlobalRenderPoints();
            spriteBatch.Draw(renderTarget, Vector2.Zero.ToScreen() + (ScreenSize/ ScreenScale)/2, frame, Color.White,0f, frame.Size.ToVector2() / 2, 1/ ScreenScale, SpriteEffects.None,0f);
            spriteBatch.End();
            spriteBatch.Begin();
            TestParticleSystem.Draw(spriteBatch);

            spriteBatch.End();

            sceneManager.Draw();

            base.Draw(gameTime);
        }
    }
}
