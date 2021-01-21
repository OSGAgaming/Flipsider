
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider
{
    public class Camera
    {
        public Matrix ParallaxedTransform(float Paralax)
        {
            return Matrix.CreateTranslation(new Vector3(-playerpos.AddParallaxAcrossX(Paralax), 0)) *
                      Matrix.CreateScale(GetScreenScale()) *
                      Matrix.CreateRotationZ(rotation) *
                      Matrix.CreateTranslation(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2, 0);
        }
        public Matrix Transform { get; set; }
        public float scale { get; set; }
        public float rotation { get; set; }

        public static int screenShake;
        public Vector2 CamPos => playerpos - new Vector2(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2) / scale;
        public float targetScale;
        public Vector3 GetScreenScale()
        {
            float scaleX = 1;
            float scaleY = 1;
            return new Vector3(scaleX * scale, scaleY * scale, 1.0f);
        }
        public Vector2 playerpos;
        public Vector2 offset;
        public float LeftBound => Main.ActualScreenSize.X / (2 * scale);

        public void FixateOnPlayer(Player player)
        {

            //Temporarily here only
            if (screenShake > 0) screenShake--;

            var shake = new Vector2(Main.rand.Next(-screenShake, screenShake), Main.rand.Next(-screenShake, screenShake));

            playerpos = playerpos.ReciprocateToInt(player.Center, 16f);
            int height = (int)Main.ActualScreenSize.Y;

            playerpos += offset;
            rotation = Main.rand.Next(-screenShake, screenShake) / 300f;
            if (!Main.Editor.IsActive && Main.CurrentScene.Name != "Main Menu")
            {
                playerpos.X = Math.Clamp(playerpos.X, LeftBound, 100000);
                playerpos.Y = Math.Clamp(playerpos.Y, -100000, height - (height / (2 * scale)) - (Main.ActualScreenSize.Y - Utils.BOTTOM));
            }
            else
            {

            }
            Transform =
                 Matrix.CreateTranslation(new Vector3(-playerpos + shake, 0)) *
                 Matrix.CreateScale(GetScreenScale()) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2, 0);
        }

    }
}
