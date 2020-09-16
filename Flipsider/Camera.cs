
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public float scale { get; set; }
        public float rotation { get; set; }
        public static int screenShake;

        public Vector2 CamPos => playerpos - new Vector2(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / 2);
            

        public Vector3 GetScreenScale()
        {
            var scaleX = 1;
            var scaleY = 1;
            return new Vector3(scaleX * scale, scaleY * scale, 1.0f);
        }

        Vector2 playerpos;
        public Vector2 offset;

        public void FixateOnPlayer(Player player)
        {
            //Temporarily here only
            if (screenShake > 0) screenShake--;

            var shake = new Vector2(Main.rand.Next(-screenShake, screenShake), Main.rand.Next(-screenShake, screenShake));

            playerpos += (player.Center - playerpos) / 12f;
            int width = (int)Main.ScreenSize.X;
            int height = (int)Main.ScreenSize.Y;

            playerpos += offset;

            if (scale >= 1)
            {
                playerpos.X = Math.Clamp(playerpos.X, width / (2 * scale), 100000);
                playerpos.Y = Math.Clamp(playerpos.Y, height / (2 * scale) - 200, height - (height / (2 * scale)));
            }
            else
            {
                playerpos.X = width / 2;
                playerpos.Y = height / 2;
            }

            playerpos += offset;
            Transform =
                 Matrix.CreateTranslation(new Vector3(-playerpos + shake, 0)) *
                 Matrix.CreateScale(GetScreenScale()) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Main.ScreenSize.X / 2, Main.ScreenSize.Y / 2, 0);
        }

    }
}
