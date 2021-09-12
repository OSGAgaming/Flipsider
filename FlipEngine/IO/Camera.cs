using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace FlipEngine
{
    public class CameraTransform : IUpdateGT
    {
        public Vector2 Offset;
        public Vector2 Position;
        public Vector2 TransformPosition;

        public float targetScale { get; set; } = 1;

        public Matrix Transform { get; set; }

        public float Scale { get; set; } = 1;

        public float Rotation { get; set; }

        public virtual Vector2 TargetSize { get; } = FlipGame.ActualScreenSize;

        public virtual Vector2 MatrixOffset { get; set; }

        public virtual Vector2 OffsetPosition => TransformPosition + MatrixOffset;

        public float LeftBound => TargetSize.X / (float)(2 * Scale);

        public float LowerBound => Utils.BOTTOM - (TargetSize.Y / (float)(2 * Scale));

        private const float Smoothing = 16;

        protected virtual void OnUpdateTransform() { }

        protected virtual void TransformConfiguration()
        {
            Transform =
            Matrix.CreateTranslation(new Vector3(-TransformPosition, 0)) *
            Matrix.CreateRotationZ(Rotation)  *
            Matrix.CreateScale(GetScreenScale());        
        }

        public void UpdateTransform()
        {
            OnUpdateTransform();

            TransformConfiguration();
        }

        public void Update(GameTime gameTime)
        {
            TransformPosition = Position + Offset;

            TransformPosition.X = Math.Max(TransformPosition.X, LeftBound);
            TransformPosition.Y = Math.Min(TransformPosition.Y, LowerBound);

            Scale += (targetScale - Scale) / Smoothing;

            UpdateTransform();
        }

        public Vector3 GetScreenScale()
        {
            float scaleX = 1;
            float scaleY = 1;

            return new Vector3(scaleX * Scale, scaleY * Scale, 1.0f);
        }
        public Matrix ParallaxedTransform(float Paralax) => Transform * Matrix.CreateTranslation(new Vector3(-TransformPosition.AddParallaxAcrossX(Paralax) + TransformPosition, 0));
        
        public Matrix GetTransform() => Transform;

        public Chunk ActiveChunk => FlipGame.World.tileManager.GetChunkToWorldCoords(TransformPosition);

        public CameraTransform()
        {
            FlipE.GameTimeUpdateables.Add(this);
        }
    }
}
