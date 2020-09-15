
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Flipsider.GUI;
using Flipsider.GUI.HUD;

namespace Flipsider
{
    public class Main : Game
    {
        private Hud hud = new Hud();

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
        private SpriteFont font;
        public float targetScale = 1;
        private int scrollBuffer;
        int delay;

        public static int MaxTilesX
        {
            get => 1000;
        }

        public static int MaxTilesY
        {
            get => 1000;
        }

        public static Vector2 ScreenSize => graphics == null ? Vector2.Zero : new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        public static Point MouseScreen => Mouse.GetState().Position;
        public static int[,] tiles;

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
            tiles = new int[MaxTilesX, MaxTilesY];
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
            instance = this;
            
            spriteBatch = new SpriteBatch(GraphicsDevice);         
        }

        protected override void Update(GameTime gameTime)
        {
            verletEngine.points[0].point = player.position + new Vector2((player.spriteDirection == - 1 ? 5 : 0) + 12, 30);
            verletEngine.points[1].point = player.position + new Vector2((player.spriteDirection == -1 ? 5 : 0) + 22, 30);
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
                TileManager.AddTile();
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                TileManager.RemoveTile();
            }
            //I was lazy to make another instance variable, so I just calculated my average press time lol
            if (state.IsKeyDown(Keys.Z) && delay == 0)
            {
                SwitchModes();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            verletEngine.Update();
            if (!EditorMode)
            {
                player.Update();
                mainCamera.offset -= mainCamera.offset / 16f;
            }
            ControlEditorScreen();
            scrollBuffer = mouseState.ScrollWheelValue;
            base.Update(gameTime);
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
            float camMoveSpeed = 2;
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
            player.RenderPlayer();
            verletEngine.GlobalRenderPoints();
            TileManager.ShowTileCursor();
            TileManager.RenderTiles();

            RenderUI();
        }

        void RenderUI()
        {
            spriteBatch.End();
            spriteBatch.Begin();
            hud.active = true;
            hud.Draw(spriteBatch);
            //spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            Render();
            //debuganthinghere
            spriteBatch.DrawString(font, player.onGround.ToString(), new Vector2(100, 100).ToScreen(), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
