using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace Flipsider
{
    public class LightingMap : MapPass
    {
        public override float Priority => 3;
        protected override Effect? MapEffect => EffectCache.LightingMap;

        internal override void OnApplyShader()
        {
            MapEffect?.Parameters["LightMap"]?.SetValue(Utils.GetMap("LightingOcclusionMap").MapTarget);
            base.OnApplyShader();
        }
    }
}