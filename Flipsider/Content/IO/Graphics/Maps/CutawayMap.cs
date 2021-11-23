using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace Flipsider
{
    public class CutawayMap : MapPass
    {
        public override int Priority => 8;
        protected override Effect? MapEffect => EffectCache.CutawayEffect;

        public static bool Outdoors { get; set; } = true;
        public static float CutawayProgression;

        public float CutawayLatency => 4f;

        internal override void OnApplyShader()
        {
            if (Outdoors) { CutawayProgression = CutawayProgression.ReciprocateTo(0, CutawayLatency); }
            else { CutawayProgression = CutawayProgression.ReciprocateTo(1, CutawayLatency); }

            Main.lighting.Maps.DrawToMap("CutawayMap", (SpriteBatch sb) =>
            {
                sb.Draw(TextureCache.magicPixel, new Rectangle(Main.Camera.Position.ToPoint().Sub(new Point(1000, 1000)), (Main.ActualScreenSize / FlipGame.ScreenScale).ToPoint().Add(new Point(2000, 2000))), Color.White * (1 - CutawayProgression));
            });

            MapEffect?.CurrentTechnique.Passes[0].Apply();

            Outdoors = true;
        }

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }
    }
}