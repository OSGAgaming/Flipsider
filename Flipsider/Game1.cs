
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
        public static Vector2 screenSize => new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        public static int[,] tiles;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Scale = graphics.PreferredBackBufferWidth / 320;
        }

        private Verlet verletEngine;
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferMultiSampling = false;
            tiles = new int[(int)screenSize.X / TileManager.tileRes, (int)screenSize.Y / TileManager.tileRes];
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Contents = Content;
            mainCamera = new Camera();
            TextureCache.LoadTextures();
            instance = this;
            player = new Player(new Vector2(100, 100));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
        }

        protected override void Update(GameTime gameTime)
        {
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
            if (state.IsKeyDown(Keys.Z) && gameTime.TotalGameTime.Ticks % 5 == 0)
            {
                EditorMode = !EditorMode;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            verletEngine.Update();
            player.Update();
            mainCamera.FixateOnPlayer(player);

            if(EditorMode)
            {
                mainCamera.scale += (0.7f - mainCamera.scale)/16f;
            }
            else
            {
                mainCamera.scale += (1 - mainCamera.scale) / 16f;
            }
            mainCamera.rotation = 0;
            Debug.Write(screenSize);
            base.Update(gameTime);
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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
