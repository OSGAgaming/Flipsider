using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace FlipEngine
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
            Offsets = (GW ?? GuassianWeights.DefaultGuassianWeights).Offsets;
            Weights = (GW ?? GuassianWeights.DefaultGuassianWeights).GuassianWeight;
        }
    }
}