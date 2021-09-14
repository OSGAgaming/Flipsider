using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class TileCache : ILoadable
    {
        public void Load()
        {
            TileManager.AddTileType(0, TextureCache.TileSet1);
            TileManager.AddTileType(1, TextureCache.TileSet2);
            TileManager.AddTileType(2, TextureCache.TileSet3);
            TileManager.AddTileType(3, TextureCache.TileSet4);

        }
    }
}
