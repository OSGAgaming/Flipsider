using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    //OBSOLETE
    public struct BloomSettings
    {
        public float NoOfFramesX;
        public float NoOfFramesY;
        public float FrameX;
        public float FrameY;
        public float Intensity;
        public float Saturation;
        public float[] Offsets;
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
        //except this part
        public LivingEntity BindableEntity;
        public static GuassianWeights DefaultGuassianWeights = new GuassianWeights(20, 1, 4f);
        public EntityBloom(LivingEntity entity, Texture2D BloomMap, float str, Vector2 pos = default, Color col = default) : base(str, pos, col)
        {
            DefaultGuassianWeights = new GuassianWeights(10, 1, 4f);
            BindableEntity = entity;
            Main.AutoAppendToLayer(this);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}