using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class Camera
    {
        public Matrix ParallaxedTransform(float Paralax)
        {
            return Matrix.CreateTranslation(new Vector3(-CameraPlayerPosition.AddParallaxAcrossX(Paralax), 0)) *
                      Matrix.CreateScale(GetScreenScale()) *
                      Matrix.CreateRotationZ(rotation) *
                      Matrix.CreateTranslation(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2, 0);
        }
        private static float ScaleEstimate => ((2560 - Main.ActualScreenSize.X) / 2560f + (1440 - Main.ActualScreenSize.Y) / 2560f)/2;
        public static float Scaling => 1 + ScaleEstimate;
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
        public Vector2 CameraPlayerPosition;
        public Vector2 Offset;
        public float LeftBound => Main.ActualScreenSize.X / (2 * scale);

        public void FixateOnPlayer(Player player)
        {

            //Temporarily here only
            if (screenShake > 0) screenShake--;

            Vector2 shake = new Vector2(Main.rand.Next(-screenShake, screenShake), Main.rand.Next(-screenShake, screenShake));

            CameraPlayerPosition = CameraPlayerPosition.ReciprocateTo(player.Center + Offset, 16f);
            float height = Main.ActualScreenSize.Y;

            rotation = Main.rand.Next(-screenShake, screenShake) / 800f;
            if (!Main.Editor.IsActive && Main.CurrentScene.Name != "Main Menu")
            {
                CameraPlayerPosition.X = Math.Clamp(CameraPlayerPosition.X, LeftBound, 100000);
                CameraPlayerPosition.Y = Math.Clamp(CameraPlayerPosition.Y, -100000, height - (height / (float)(2 * scale)) - (Main.ActualScreenSize.Y - Utils.BOTTOM));
            }
            else
            {

            }
            Transform =
                 Matrix.CreateTranslation(new Vector3(-CameraPlayerPosition + shake, 0)) *
                 Matrix.CreateScale(GetScreenScale()) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Main.ActualScreenSize.X / 2, Main.ActualScreenSize.Y / 2, 0);
            CamPos = CameraPlayerPosition - (Main.ActualScreenSize/2f) / scale;
        }

    }
}
