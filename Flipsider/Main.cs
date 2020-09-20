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

namespace Flipsider
{
    public class Main : Game
    {
        public SceneManager sceneManager;

        public static Random rand;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static Main instance;
        private Hud hud;
        private TileGUI tileGUI;
        private NPCGUI npcGUI;
        //Terraria PTSD
        public static Texture2D character;
        public static Player player;
        public static bool EditorMode;
        public static GameTime gameTime;
        public static Camera mainCamera;
        public static SpriteFont font;
        public float targetScale = 1;
        public static Texture2D currentAtlas;
        public static Rectangle currentFrame;
        public static List<Entity> entities = new List<Entity>();
        public static bool TileEditorMode;
        public static bool NPCSpawnerMode;
        public static int MaxTilesX
        {
            get => 1000;
        }

        public static int MaxTilesY
        {
            get => 1000;
        }
        public static float ScreenScale => mainCamera.scale;
        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
        public static Point MouseScreen => (Mouse.GetState().Position.ToVector2() / mainCamera.scale).ToPoint() + mainCamera.CamPos.ToPoint();
        public static Tile[,] tiles;

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
        protected override void Initialize()
        {
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

            sceneManager = new SceneManager();
            sceneManager.SetNextScene(new DebugScene(), null);

            tiles = new Tile[MaxTilesX, MaxTilesY];
            verletEngine = new Verlet();
            rand = new Random();
            player = new Player(new Vector2(100, 100));

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
            Type[] NPCTypes = ReflectionHelpers.GetInheritedClasses(typeof(NPC));
            NPC.NPCTypes = new NPC.NPCInfo[NPCTypes.Length];
            for (int i = 0; i < NPCTypes.Length; i++)
            {
                NPC.NPCTypes[i].type = NPCTypes[i];
            }

            // Verlet examples

            /* verletEngine.CreateStickMan(new Vector2(100, 100));
             verletEngine.CreateVerletSquare(new Vector2(150, 150), 30);
             int firstPoint = verletEngine.CreateVerletPoint(new Vector2(150, 100),true);
             int secondPoint = verletEngine.CreateVerletPoint(new Vector2(100, 120));
             int thirdPoint = verletEngine.CreateVerletPoint(new Vector2(100, 140));
             int fourthPoint = verletEngine.CreateVerletPoint(new Vector2(100, 160));
             verletEngine.BindPoints(firstPoint, secondPoint);
             verletEngine.BindPoints(secondPoint, thirdPoint);
             verletEngine.BindPoints(thirdPoint, fourthPoint);
             */
            targetScale = 1.2f;
            //  NPC.SpawnNPC<Blob>(player.position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: Create SFX and Music bank (boffin or salv's job, based on who ends up doing the fmod studio stuff.)
            //GameAudio.Instance.LoadBank("SFX", "Audio\\SFX.bank");

            font = Content.Load<SpriteFont>("FlipFont");
            mainCamera = new Camera();
            TextureCache.LoadTextures(Content);

            TestParticleSystem = new ParticleSystem(200);
            TestParticleSystem.SpawnRate = 10f;
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
            TestParticleSystem.UpdateModules.Add(condition);

            AddTileType(TextureCache.TileSet1, "TileSet1");
            AddTileType(TextureCache.TileSet2, "TileSet2");
            tileGUI = new TileGUI();
            npcGUI = new NPCGUI();
            hud = new Hud();
            instance = this;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            GameInput.Instance.UpdateInput();

            verletEngine.points[0].point = player.position + new Vector2((player.spriteDirection == -1 ? -8 : 0) + 18, 30) + player.velocity;
            verletEngine.points[1].point = player.position + new Vector2((player.spriteDirection == -1 ? -8 : 0) + 25, 30) + player.velocity;

            Main.gameTime = gameTime;

            sceneManager.Update();

            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                AddTile();
            }
            if (GameInput.Instance["EdtiorRemoveTile"].IsDown())
            {
                RemoveTile();
            }

            if (GameInput.Instance["EditorSwitchModes"].IsJustPressed())
            {
                SwitchModes();
            }

            if (EditorMode)
            {
                if (GameInput.Instance["EditorTileEditor"].IsJustPressed())
                {
                    SwitchToTileEditorMode();
                }
                if (GameInput.Instance["NPCEditor"].IsJustPressed())
                {
                    SwitchToNPCEditorMode();
                }
            }

            verletEngine.Update();

            tileGUI.active = TileEditorMode;

            if (!EditorMode)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Update();
                    entities[i].UpdateTrailCache();
                }
                mainCamera.offset -= mainCamera.offset / 16f;
            }

            ControlEditorScreen();
            hud.Update();
            tileGUI.Update();
            npcGUI.Update();

            TestParticleSystem.Position = GameInput.Instance.MousePosition;
            TestParticleSystem.Update();

            base.Update(gameTime);
        }
        void SwitchToTileEditorMode()
        {
            TileEditorMode = !TileEditorMode;
        }
        void SwitchToNPCEditorMode()
        {
            NPCSpawnerMode = !NPCSpawnerMode;
        }
        void SwitchModes()
        {
            EditorMode = !EditorMode;
            if (EditorMode)
            {
                targetScale = 0.8f;
            }
            else
            {
                targetScale = 1.2f;
            }
        }

        void ControlEditorScreen()
        {
            mainCamera.FixateOnPlayer(player);
            mainCamera.rotation = 0;

            float scrollSpeed = 0.02f;
            float camMoveSpeed = 0.2f;
            if (EditorMode)
            {
                if (GameInput.Instance["EditorZoomIn"].IsDown())
                {
                    targetScale += scrollSpeed;
                }
                if (GameInput.Instance["EditorZoomOut"].IsDown())
                {
                    targetScale -= scrollSpeed;
                }

                if (GameInput.Instance["MoveRight"].IsDown())
                {
                    mainCamera.offset.X += camMoveSpeed;
                }
                if (GameInput.Instance["MoveLeft"].IsDown())
                {
                    mainCamera.offset.X -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveUp"].IsDown())
                {
                    mainCamera.offset.Y -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveDown"].IsDown())
                {
                    mainCamera.offset.Y += camMoveSpeed;
                }
            }
            else
            {

            }
            mainCamera.scale += (targetScale - mainCamera.scale) / 16f;
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        void Render()
        {
            RenderSkybox();

            //TODO: Move this later
            for (int k = 0; k < entities.Count; k++)
            {
                Entity entity = entities[k];
                entity.Draw(spriteBatch);
            }

            verletEngine.GlobalRenderPoints();

            RenderTiles();
            ShowTileCursor();
            RenderUI();
        }

        void RenderSkybox()
        {
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(TextureCache.skybox, Vector2.Zero.AddParralaxAcross(5), Color.White);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: mainCamera.Transform, samplerState: SamplerState.PointClamp);
        }

        void RenderUI()
        {
            spriteBatch.End();
            spriteBatch.Begin();

            hud.active = true;
            hud.Draw(spriteBatch);
            tileGUI.active = true;
            tileGUI.Draw(spriteBatch);
            npcGUI.active = true;
            npcGUI.Draw(spriteBatch);
            //debuganthinghere
            fps.DrawFps(spriteBatch, font, new Vector2(10, 36), Color.Black);
        }

        FPS fps = new FPS();
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            fps.Update(gameTime);
            
            spriteBatch.Begin(transformMatrix: mainCamera.Transform, samplerState: SamplerState.PointClamp);
            
            Render();

            spriteBatch.End();

            spriteBatch.Begin();

            TestParticleSystem.Draw(spriteBatch);

            spriteBatch.End();

            sceneManager.Draw();

            base.Draw(gameTime);
        }
    }
}
