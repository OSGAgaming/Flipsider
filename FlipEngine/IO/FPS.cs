using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlipEngine
{
    public class FPS : IUpdateGT
    {
        private float frames = 0;
        private float updates = 0;
        private float elapsed = 0;
        private float last = 0;
        private float now = 0;
        public double msgFrequency = 0.001f;
        public string msg = "";
        int average = 0;
        int torture;

        public static FPS Instance;

        static FPS(){ Instance = new FPS(); }

        public void Update(GameTime gameTime)
        {
            torture++;
            now = (float)gameTime.TotalGameTime.TotalSeconds;
            elapsed = now - last;
            if (elapsed > msgFrequency)
            {
                average += (int)(frames / elapsed);
                elapsed = 0;
                frames = 0;
                updates = 0;
                last = now;
            }
            if (torture % 60 == 0)
            {
                msg = " Fps: " + (average / 60).ToString();
                average = 0;
            }
            updates++;
        }

        public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
        {
            spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor, 0, Vector2.Zero, 1.5f, 0, 0);
            frames++;
        }
    }
}
