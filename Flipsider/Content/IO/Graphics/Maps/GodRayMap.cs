﻿using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class GodrayMap : MapPass
    {
        protected override Effect? MapEffect => EffectCache.GodrayEffect;

        internal override void OnApplyShader() 
        {
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }
    }
}