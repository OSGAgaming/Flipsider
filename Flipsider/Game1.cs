
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        public Main()
        {
            rand = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private Verlet verletEngine;
        protected override void Initialize()
        {
            verletEngine = new Verlet();
            // Verlet examples
            verletEngine.CreateStickMan(new Vector2(100, 100));
            verletEngine.CreateVerletSquare(new Vector2(150, 150), 30);
            int firstPoint = verletEngine.CreateVerletPoint(new Vector2(100, 100),true);
            int secondPoint = verletEngine.CreateVerletPoint(new Vector2(100, 120));
            int thirdPoint = verletEngine.CreateVerletPoint(new Vector2(100, 140));
            int fourthPoint = verletEngine.CreateVerletPoint(new Vector2(100, 160));
            verletEngine.BindPoints(firstPoint, secondPoint);
            verletEngine.BindPoints(secondPoint, thirdPoint);
            verletEngine.BindPoints(thirdPoint, fourthPoint);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            instance = this;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            verletEngine.Update();
            if (pixel == null)
            {
                pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
                pixel.SetData(new Color[] { Color.White });
            }

            base.Update(gameTime);
        }
        protected override void UnloadContent()
        {
            pixel.Dispose();
            base.UnloadContent();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            verletEngine.GlobalRenderPoints();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
