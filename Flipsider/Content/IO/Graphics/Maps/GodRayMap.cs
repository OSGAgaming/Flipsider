using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace Flipsider
{
    public class GodrayMap : MapPass
    {
        public override float Priority => 5;
        protected override Effect? MapEffect => EffectCache.GodrayEffect;

        internal override void OnApplyShader() 
        {
            //MapEffect?.CurrentTechnique.Passes[0].Apply();
        }

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }
    }
}