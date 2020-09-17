
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Flipsider.GUI;
using Flipsider.GUI.HUD;
using System.Collections.Generic;
using Flipsider.GUI.TilePlacementGUI;
using static Flipsider.TileManager;

namespace Flipsider
{
    public class Main : Game
    {
        private Hud hud;
        private TileGUI tileGUI;
        //Terraria PTSD
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Main instance;
        public static Random rand;
        public static Texture2D pixel;
        public static Texture2D character;
        public static Player player;
        public static bool EditorMode;
        public static GameTime gameTime;
        public static  ContentManager Contents;
        public static Camera mainCamera;
        public static float Scale;
        public static SpriteFont font;
        public float targetScale = 1;
        private int scrollBuffer;
        public static Texture2D currentAtlas;
        public static Rectangle currentFrame;
        int delay;
        public static List<Entity> entities = new List<Entity>();
        public static bool TileEditorMode;

        public static int MaxTilesX
        {
            get => 1000;
        }

        public static int MaxTilesY
        {
            get => 1000;
        }

        public static Vector2 ScreenSize => graphics.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
        public static Point MouseScreen => (Mouse.GetState().Position.ToVector2()/mainCamera.scale).ToPoint() + mainCamera.CamPos.ToPoint();
        public static Tile[,] tiles;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        private Verlet verletEngine;
        protected override void Initialize()
        {
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
                     verletEngine.BindPoints(verletEngine.points.Count - 1, connectionOne,true,Color.White);
                 }
                 else
                 {
                     verletEngine.BindPoints(verletEngine.points.Count - 1, verletEngine.points.Count - 2,true, Color.White);
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
                     verletEngine.BindPoints(verletEngine.points.Count - 1, verletEngine.points.Count - 2,true, Color.White);
                 }
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
             MouseState mouseState = Mouse.GetState();
            scrollBuffer = mouseState.ScrollWheelValue;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Contents = Content;
            font = Content.Load<SpriteFont>("FlipFont");
            mainCamera = new Camera();
            TextureCache.LoadTextures();
            AddTileType(TextureCache.TileSet1, "TileSet1");
            tileGUI = new TileGUI();
            hud = new Hud();
            instance = this;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            verletEngine.points[0].point = player.position + new Vector2((player.spriteDirection == - 1 ? 5 : 0) + 12, 30) + player.velocity;
            verletEngine.points[1].point = player.position + new Vector2((player.spriteDirection == -1 ? 5 : 0) + 22, 30) + player.velocity;
            if (delay > 0)
                delay--;
            //this is vile, please change, cause I dont know whats being passed
            Main.gameTime = gameTime;
            if (pixel == null)
            {
                pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
                pixel.SetData(new Color[] { Color.White });
            }
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                AddTile();
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveTile();
            }
            //I was lazy to make another instance variable, so I just calculated my average press time lol
            if (state.IsKeyDown(Keys.Z) && delay == 0)
            {
                SwitchModes();
            }
            if(EditorMode)
            {
                if(state.IsKeyDown(Keys.T) && delay == 0)
                {
                    SwitchToTileEditorMode();
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            verletEngine.Update();

            tileGUI.active = TileEditorMode;

            if (!EditorMode)
            {
                for(int i = 0; i<entities.Count; i++)
                {
                    entities[i].Update();
                    entities[i].UpdateTrailCache();
                }
                mainCamera.offset -= mainCamera.offset / 16f;
            }

            ControlEditorScreen();
            scrollBuffer = mouseState.ScrollWheelValue;
            hud.Update();
            tileGUI.Update();
            base.Update(gameTime);
        }
        void SwitchToTileEditorMode()
        {
            delay = 30;
            TileEditorMode = !TileEditorMode;
        }
        void SwitchModes()
        {
            delay = 30;
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
            MouseState mouseState = Mouse.GetState();
            KeyboardState state = Keyboard.GetState();
            float scrollSpeed = 0.02f;
            float camMoveSpeed = 0.2f;
            if (EditorMode)
            {
                if (scrollBuffer < mouseState.ScrollWheelValue)
                {
                    targetScale += scrollSpeed;
                }
                if (scrollBuffer > mouseState.ScrollWheelValue)
                {
                    targetScale -= scrollSpeed;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    mainCamera.offset.X += camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    mainCamera.offset.X -= camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    mainCamera.offset.Y -= camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.S))
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
            pixel.Dispose();
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
            ShowTileCursor();
            hud.active = true;
            hud.Draw(spriteBatch);
            tileGUI.active = true;
            tileGUI.Draw(spriteBatch);
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

            base.Draw(gameTime);
        }
    }
}
