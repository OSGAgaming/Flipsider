using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Core
{
    public class Camera
    {
        public Viewport Viewport => FlipsiderGame.GameInstance.GraphicsDevice == null ? new Viewport(0, 0, 1, 1) : FlipsiderGame.GameInstance.GraphicsDevice.Viewport;
        public Vector2 MouseScreen => Mouse.GetState().Position.ToVector2() / scale + Translation2D;

        private Vector2 scale;
        public Vector2 Scale
        {
            get => scale;
            set => Transform *= Matrix.CreateScale(new Vector3(scale = value, 1));
        }

        public Vector2 Translation2D
        {
            get
            {
                // cache translation value AS MUCH AS POSSIBLE to save computation
                var translation = Transform.Translation;
                return new Vector2(translation.X, translation.Y);
            }
        }

        public Matrix Transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(1) * Matrix.CreateRotationZ(0);
    }
}
