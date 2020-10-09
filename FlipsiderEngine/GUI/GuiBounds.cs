using Flipsider.Core;
using Microsoft.Xna.Framework;

namespace Flipsider.GUI
{
    /// <summary>
    /// Represents a GUI element position.
    /// </summary>
    public struct GuiBounds
    {
        public GuiBounds(Vector2 offset_pixels, Vector2 offset_percent, float width, float height)
        {
            PixelOffset = offset_pixels;
            PercentOffset = offset_percent;
            Width = width;
            Height = height;
        }

        public Vector2 PixelOffset;
        public Vector2 PercentOffset;
        public float Width;
        public float Height;

        public Vector2 PositionScreen => PixelOffset + PercentOffset * FlipsiderGame.GameInstance.CurrentCamera.ScreenSize;
    }
}
