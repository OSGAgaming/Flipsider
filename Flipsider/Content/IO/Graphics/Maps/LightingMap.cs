using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class LightingMap : MapPass
    {
        public override int Priority => 3;
        protected override Effect? MapEffect => EffectCache.LightingMap;

        internal override void OnApplyShader()
        {
            MapEffect?.Parameters["LightMap"]?.SetValue(Utils.GetMap("LightingOcclusionMap").MapTarget);
            base.OnApplyShader();
        }
    }
}