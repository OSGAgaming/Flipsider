namespace Flipsider.Graphics
{
    public struct ParallaxDrawData
    {
        public ParallaxDrawData(float parallaxFactor, DrawData data)
        {
            Data = data;
            ParallaxFactor = parallaxFactor;
        }

        internal DrawData Data;
        internal float ParallaxFactor;
    }
}
