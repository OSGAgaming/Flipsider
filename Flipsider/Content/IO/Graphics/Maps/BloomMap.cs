using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class BloomMap : MapPass
    {
        protected override Effect? MapEffect => EffectCache.BloomEffect;

        public RenderTarget2D HorizontalBuffer;
        public RenderTarget2D VerticalBuffer;
        public RenderTarget2D CombineBuffer;

        BloomSettings BS;

        public void RenderBuffer(int PassIndex, RenderTarget2D currentTarget, RenderTarget2D previousTarget)
        {
            Main.graphics?.GraphicsDevice.SetRenderTarget(currentTarget);
            Main.graphics?.GraphicsDevice.Clear(Color.Transparent);

            MapEffect?.CurrentTechnique.Passes[PassIndex].Apply();

            Rectangle frame = new Rectangle(0, 0, 2560, 1440);
            Main.spriteBatch.Draw(previousTarget, Main.mainCamera.CamPos, frame, Color.White, 0f, Vector2.Zero, new Vector2(1 / Main.ScreenScale, 1 / Main.ScreenScale), SpriteEffects.None, 0f);
        }

        internal override void OnApplyShader() 
        {
            SetGuassianParameters();
            MapEffect?.Parameters["Dims"]?.SetValue(new Vector2(2560/2, 1440 / 2));

            MapEffect?.Parameters["Map"]?.SetValue(MapTarget);
            RenderBuffer(0, HorizontalBuffer, Main.lighting.Maps.Buffers[Index]);

            MapEffect?.Parameters["Map"]?.SetValue(HorizontalBuffer);
            RenderBuffer(1, VerticalBuffer, HorizontalBuffer);

            Main.graphics?.GraphicsDevice.SetRenderTarget(Main.lighting.Maps.Buffers[Index]);
            Main.graphics?.GraphicsDevice.Clear(Color.Transparent);

            MapEffect?.Parameters["Map"]?.SetValue(VerticalBuffer);
            MapEffect?.CurrentTechnique.Passes[2].Apply();
        }

        public void SetGuassianParameters()
        {
            //literally to declutter that mess of a method, lazy to add params
            MapEffect?.Parameters["BloomIntensity"]?.SetValue(BS.Intensity);
            MapEffect?.Parameters["BloomSaturation"]?.SetValue(BS.Saturation);
            MapEffect?.Parameters["BaseIntensity"]?.SetValue(BS.Intensity);
            MapEffect?.Parameters["BaseSaturation"]?.SetValue(BS.Saturation);
            MapEffect?.Parameters["Offsets"]?.SetValue(BS.Offsets);
            MapEffect?.Parameters["Weights"]?.SetValue(BS.Weights);
        }
        public override void Load()
        {
            BS = new BloomSettings(1,1,0,0,1,1);
        }
        public BloomMap()
        {
            MapTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
            HorizontalBuffer = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
            VerticalBuffer = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
            CombineBuffer = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }
    }
    //Too lazy to inherit. Just want it to fucking work
}