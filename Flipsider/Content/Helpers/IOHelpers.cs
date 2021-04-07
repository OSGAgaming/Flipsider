using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Flipsider
{
    public static class Assets
    {
        public static Texture2D GetTexture(string Path) => AutoloadTextures.Assets[Path];
    }
}
