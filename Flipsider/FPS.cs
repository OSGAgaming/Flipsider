using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flipsider
{
        public class FPS
        {
            private float frames = 0;
            private float updates = 0;
            private float elapsed = 0;
            private float last = 0;
            private float now = 0;
            public double msgFrequency = 0.001f;
            public string msg = "";

            public void Update(GameTime gameTime)
            {
                now = (float)gameTime.TotalGameTime.TotalSeconds;
                elapsed = now - last;
                if (elapsed > msgFrequency)
                {
                    msg = " Fps: " + ((int)(frames / elapsed)).ToString();
                    elapsed = 0;
                    frames = 0;
                    updates = 0;
                    last = now;
                }
                updates++;
            }

            public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
            {
                spriteBatch.DrawString(font,Main.mainCamera.CamPos.ToString(), fpsDisplayPosition, fpsTextColor, 0, Vector2.Zero, 0.6f, 0, 0);
                frames++;
            }
        }
}
