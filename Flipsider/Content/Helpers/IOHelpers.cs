using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using FlipEngine;

namespace Flipsider
{
    public static class Assets
    {
        public static Texture2D GetTexture(string Path) => AutoloadTextures.Assets[Path];
    }
}
