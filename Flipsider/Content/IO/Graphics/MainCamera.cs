using FlipEngine;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Flipsider.Content.IO.Graphics
{
    public class GameCamera : CameraTransform
    {
        public int ScreenShake { get; set; }

        public Entity? FocalEntity { get; set; }

        public float RotationalScreenShake => 3000f;

        public float MouseVision => 20f;

        public Vector2 EntityIsolatedPosition;

        public override Vector2 TargetSize => Main.ActualScreenSize;

        protected override void OnUpdateTransform()
        {
            if (ScreenShake > 0) ScreenShake--;
            Vector2 shake = new Vector2(FlipE.rand.Next(-ScreenShake, ScreenShake), FlipE.rand.Next(-ScreenShake, ScreenShake));

            Vector2 mouseDisp = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Utils.ScreenSize / 2) * MouseVision;
            Rotation = FlipE.rand.Next(-ScreenShake, ScreenShake) / RotationalScreenShake;

            if (Main.CurrentScene.Name != "Main Menu")
            {
                Vector2 newCenter = Main.player.Center + mouseDisp + MatrixOffset;

                EntityIsolatedPosition = EntityIsolatedPosition.ReciprocateTo(newCenter, Smoothing);
                EntityIsolatedPosition.X = Math.Max(EntityIsolatedPosition.X, LeftBound);
                EntityIsolatedPosition.Y = Math.Min(EntityIsolatedPosition.Y, LowerBound);
            }
            else
            {
                EntityIsolatedPosition = EntityIsolatedPosition.ReciprocateTo(Main.player.Center, Smoothing);
            }

            Position = EntityIsolatedPosition - Main.ActualScreenSize / (2 * Scale) + shake;
            MatrixOffset = Offset;
        }
        public Matrix ParallaxedTransform(float Paralax)
        {
            return Transform * Matrix.CreateTranslation(new Vector3(-Position.AddParallaxAcrossX(Paralax) + Position, 0));
        }

        private static float ScaleEstimate => ((2560 - Main.ActualScreenSize.X) / 2560f + (1440 - Main.ActualScreenSize.Y) / 2560f) / 2;
        public static float Scaling => 1 + ScaleEstimate;
    }
}
