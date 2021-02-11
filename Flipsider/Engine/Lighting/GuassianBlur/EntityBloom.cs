using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public struct BloomSettings
    {
        public float NoOfFramesX;
        public float NoOfFramesY;
        public float FrameX;
        public float FrameY;
        public float Intensity;
        public float Saturation;
        public Vector2[] Offsets;
        public float[] Weights;
        public static BloomSettings DefaultBloomSettings => new BloomSettings(1, 1, 0, 0, 1, 1);
        public BloomSettings(
        float NoOfFramesX = 1,
        float NoOfFramesY = 1,
        float FrameX = 0,
        float FrameY = 0,
        float Intensity = 1,
        float Saturation = 1,
        GuassianWeights? GW = null)
        {
            this.NoOfFramesX = NoOfFramesX;
            this.NoOfFramesY = NoOfFramesY;
            this.FrameX = FrameX;
            this.FrameY = FrameY;
            this.Intensity = Intensity;
            this.Saturation = Saturation;
            Offsets = (GW ?? EntityBloom.DefaultGuassianWeights).Offsets;
            Weights = (GW ?? EntityBloom.DefaultGuassianWeights).GuassianWeight;
        }
    }
    public class EntityBloom : LightSource
    {
        public LivingEntity BindableEntity;
        Texture2D BloomMap;
        public static GuassianWeights DefaultGuassianWeights = new GuassianWeights(10, 10, 0.008f, 4f);
        public EntityBloom(LivingEntity entity, Texture2D BloomMap, float str, Vector2 pos = default, Color col = default) : base(str, pos, col)
        {
            BindableEntity = entity;
            this.BloomMap = BloomMap;
            Main.AutoAppendToLayer(this);
        }
        public void Dispose()
        {
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = BindableEntity.texture;
            if (BindableEntity.frame.Height != 0 && tex.Height > 3)
            {
                float NoOfFramesX = tex.Width / BindableEntity.framewidth;
                float NoOfFramesY = tex.Height / BindableEntity.frame.Height;
                float FrameX = BindableEntity.frame.Location.X / BindableEntity.frame.Width;
                float FrameY = BindableEntity.frame.Location.Y / BindableEntity.frame.Height;
                spriteBatch.End();
                Utils.BeginCameraSpritebatch();
                Utils.ApplyBloom(new BloomSettings(NoOfFramesX, NoOfFramesY,FrameX,FrameY,2f,1.2f));
                spriteBatch.Draw(tex, BindableEntity.Center - new Vector2(0,18), tex.Bounds, Color.White, 0f, tex.TextureCenter(), new Vector2(2/NoOfFramesX, 2/NoOfFramesY), BindableEntity.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.End();
                Utils.BeginCameraSpritebatch();
            }
            else
            {
               

            }
        }
    }
}