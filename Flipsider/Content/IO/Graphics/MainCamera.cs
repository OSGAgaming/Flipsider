using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Flipsider.Content.IO.Graphics
{
    public class GameCamera : CameraTransform
    {
        public Vector2 Offset;

        public int ScreenShake { get; set; }

        public Entity? FocalEntity { get; set; }

        public float FollowSpeed => 32f;

        public float RotationalScreenShake => 800f;

        public float MouseVision => 20f;

        public Vector2 EntityIsolatedPosition;

        public override Vector2 TargetSize => Main.ActualScreenSize;

        public float LeftBound => TargetSize.X / (float)(2 * Scale);

        public float LowerBound => TargetSize.Y - (TargetSize.Y / (float)(2 * Scale)) - (TargetSize.Y - Utils.BOTTOM);

        public float targetScale { get; set; }

        protected override void OnUpdateTransform()
        {
            if (ScreenShake > 0) ScreenShake--;
            Vector2 shake = new Vector2(Main.rand.Next(-ScreenShake, ScreenShake), Main.rand.Next(-ScreenShake, ScreenShake));

            Vector2 mouseDisp = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Utils.ScreenSize / 2) * MouseVision;
            EntityIsolatedPosition = EntityIsolatedPosition.ReciprocateTo(Main.player.Center + MatrixOffset, FollowSpeed);
            Rotation = Main.rand.Next(-ScreenShake, ScreenShake) / RotationalScreenShake;

            if (!Main.Editor.IsActive && Main.CurrentScene.Name != "Main Menu")
            {
                Vector2 newCenter = Main.player.Center + Offset + mouseDisp;

                EntityIsolatedPosition = EntityIsolatedPosition.ReciprocateTo(newCenter, FollowSpeed);
                EntityIsolatedPosition.X = Math.Max(EntityIsolatedPosition.X, LeftBound);
                EntityIsolatedPosition.Y = Math.Min(EntityIsolatedPosition.Y, LowerBound);
            }

            Position = EntityIsolatedPosition - (Main.ActualScreenSize / 2f) / Scale;
            MatrixOffset = shake + Offset;
        }
        public Matrix ParallaxedTransform(float Paralax)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.AddParallaxAcrossX(Paralax), 0)) *
                      Matrix.CreateScale(GetScreenScale()) *
                      Matrix.CreateRotationZ(Rotation);
        }

        private static float ScaleEstimate => ((2560 - Main.ActualScreenSize.X) / 2560f + (1440 - Main.ActualScreenSize.Y) / 2560f) / 2;
        public static float Scaling => 1 + ScaleEstimate;
    }
}
