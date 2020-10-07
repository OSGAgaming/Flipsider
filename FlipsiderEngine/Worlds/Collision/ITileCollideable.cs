using Flipsider.Worlds.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Worlds.Entities
{
    public interface ITileCollideable
    {
        IEnumerable<Tile> Touching { set; }
    }
}
