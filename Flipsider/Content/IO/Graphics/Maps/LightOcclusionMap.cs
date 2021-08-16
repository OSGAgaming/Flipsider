using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class LightingOcclusionMap : MapPass
    {
        public override int Priority => 2;
        protected override Effect? MapEffect => null;
        internal override void OnApplyShader() { }
    }
}