using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Graphics
{
    /// <summary>
    /// Represents a set of data to draw.
    /// </summary>
    public struct DrawData
    {
        private readonly Asset<Texture2D>? texture;
        private readonly Asset<SpriteFont>? font;
        private readonly string? text;

        public Vector2 WorldPosition { get => ScreenPosition.ToWorldCoordinates(); set => ScreenPosition = value.ToScreenCoordinates(); }
        public Vector2 ScreenPosition { get; set; }
        /// <summary>
        /// The tint of this object. Defaults to <see cref="Color.White"/>.
        /// </summary>
        public Color Tint { get; set; }
        /// <summary>
        /// The scale of this object in each axis. Defaults to <see cref="Vector2.One"/>.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// The source rectangle of this object. Irrelevant for text data. Defaults to null, meaning the entire texture is drawn.
        /// </summary>
        public Rectangle? Frame { get; set; }
        public Rotation Rotation { get; set; }
        /// <summary>
        /// Where drawing and rotating should be centered around. Defaults to null, for the center of the image or text.
        /// </summary>
        public Vector2? Origin { get; set; }
        public SpriteEffects Effects { get; set; }
        public float ZDepth { get; set; }

        private DrawData(bool defaults)
        {
            texture = null;
            font = null;
            text = null;
            ScreenPosition = Vector2.Zero;
            Effects = default;
            ZDepth = 0;
            Tint = Color.White;
            Scale = Vector2.One;
            Frame = null;
            Rotation = new Rotation();
            Origin = null;
        }

        /// <summary>
        /// Initializes a <see cref="DrawData"/> struct that can draw a texture.
        /// </summary>
        /// <param name="texture"></param>
        public DrawData(Asset<Texture2D> texture) : this(true)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Initializes a <see cref="DrawData"/> struct that can draw text.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        public DrawData(Asset<SpriteFont> font, string text) : this(true)
        {
            this.font = font;
            this.text = text;
        }

        public void Draw(SafeSpriteBatch spriteBatch) => Draw(spriteBatch.Sb);
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture.Value, ScreenPosition, Frame, Tint, Rotation.RadF, Origin ?? texture.Value.Size() / 2, Scale, Effects, ZDepth);
            }
            else if (text != null && font != null)
            {
                spriteBatch.DrawString(font, text, ScreenPosition, Tint, Rotation.RadF, Origin ?? font.Value.MeasureString(text) / 2, Scale, Effects, ZDepth);
            }
        }
    }
}