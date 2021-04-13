using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class CameraTransform
    {
        public Matrix Transform { get; set; }

        public float Scale { get; set; }

        public float Rotation { get; set; }

        public Vector2 Position;

        public virtual Vector2 TargetSize { get; }

        public virtual Vector2 MatrixOffset { get; set; }

        public virtual Vector2 OffsetPosition => Position + MatrixOffset;

        protected virtual void OnUpdateTransform() { }

        protected virtual void TransformConfiguration()
        {
            Transform =
            Matrix.CreateTranslation(new Vector3(-Position, 0)) *
            Matrix.CreateScale(GetScreenScale()) *
            Matrix.CreateRotationZ(Rotation);       
        }

        public void UpdateTransform()
        {
            OnUpdateTransform();

            TransformConfiguration();
        }
        public Vector3 GetScreenScale()
        {
            float scaleX = 1;
            float scaleY = 1;

            return new Vector3(scaleX * Scale, scaleY * Scale, 1.0f);
        }

        public Matrix GetTransform() => Transform;
    }
}
