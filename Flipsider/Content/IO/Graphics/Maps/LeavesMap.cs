using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace Flipsider
{
    public class LeavesMap : MapPass
    {
        public override float Priority => 1;
        protected override Effect? MapEffect => EffectCache.LeavesEffect;

        internal override void OnApplyShader() 
        {
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560/2, 1440/2);
        }
    }
}