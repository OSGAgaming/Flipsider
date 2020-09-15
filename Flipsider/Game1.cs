
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Flipsider
{
    public class Main : Game
    {
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
        public static Vector2 screenSize => new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
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
            // Verlet examples
            /*
            verletEngine.CreateStickMan(new Vector2(100, 100));
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
            player = new Player(new Vector2(100, 100));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
        }

        protected override void Update(GameTime gameTime)
        {
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
            //I was lazy to make another instance variable, so I just calculated my average press time lol
            if (state.IsKeyDown(Keys.Z) && delay == 0)
            {
                delay = 30;
                EditorMode = !EditorMode;
                if(EditorMode)
                {
                    targetScale = 0.8f;
                }
                else
                {
                    targetScale = 1.2f;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            verletEngine.Update();
            if (!EditorMode)
            {
                player.Update();
                mainCamera.offset -= mainCamera.offset / 16f;
            }
            mainCamera.FixateOnPlayer(player);
            mainCamera.rotation = 0;
            ControlEditorScreen();
            Debug.Write(screenSize);
            scrollBuffer = mouseState.ScrollWheelValue;
            base.Update(gameTime);
        }
        void ControlEditorScreen()
        {
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
            verletEngine.GlobalRenderPoints();
            player.RenderPlayer();
            TileManager.ShowTileCursor();
            TileManager.RenderTiles();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            Render();
            //debuganthinghere
            //spriteBatch.DrawString(font, player.position.ToString(), new Vector2(100, 100).ToScreen(), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
