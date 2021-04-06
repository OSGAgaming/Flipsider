using Microsoft.Xna.Framework;
using System;

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
        public Vector2 CamPos;
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

            Vector2 shake = new Vector2(Main.rand.Next(-screenShake, screenShake), Main.rand.Next(-screenShake, screenShake));

            playerpos = playerpos.ReciprocateTo(player.Center + offset, 16f);
            float height = Main.ActualScreenSize.Y;

            rotation = Main.rand.Next(-screenShake, screenShake) / 800f;
            if (!Main.Editor.IsActive && Main.CurrentScene.Name != "Main Menu")
            {
                playerpos.X = Math.Clamp(playerpos.X, LeftBound, 100000);
                playerpos.Y = Math.Clamp(playerpos.Y, -100000, height - (height / (float)(2 * scale)) - (Main.ActualScreenSize.Y - Utils.BOTTOM));
            }
            else
            {

            }
            Transform =
                 Matrix.CreateTranslation(new Vector3(-playerpos + shake, 0)) *
                 Matrix.CreateScale(GetScreenScale()) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2, 0);
            CamPos = playerpos - (Main.ActualScreenSize/2f) / scale;
        }

    }
}
