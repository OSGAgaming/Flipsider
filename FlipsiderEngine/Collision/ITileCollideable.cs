using Flipsider.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Entities
{
    public interface ITileCollideable
    {
        IEnumerable<Tile> Touching { set; }
    }
}
