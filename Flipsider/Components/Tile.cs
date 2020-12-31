using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
namespace Flipsider
{
    [Serializable]
    public class Tile
    {
        public int type;
        [NonSerialized]
        public Rectangle frame;
        public bool active;
        public bool wall;
        public Tile(int type, Rectangle frame, bool ifWall = false)
        {
            this.type = type;
            this.frame = frame;
            active = false;
            wall = ifWall;
        }
    }
}
