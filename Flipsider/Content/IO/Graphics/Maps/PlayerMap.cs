using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider
{
    public class PlayerMap : MapPass
    {
        public override float Priority => 0.5f;
        protected override Effect? MapEffect => null;
    }
}