using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlipEngine
{
    public static class Assets
    {
        public static Texture2D GetTexture(string Path) => AutoloadTextures.Assets[Path];
    }
}
