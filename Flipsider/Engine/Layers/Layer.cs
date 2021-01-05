
using Flipsider;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public class Layer : IComponent
    {
        public List<ILayeredComponent> Drawables = new List<ILayeredComponent>();
        public int LayerDepth;
        public float paralax;
        public bool visible = true;
        public Layer(int ld, float paralax = 0)
        {
            LayerDepth = ld;
            this.paralax = paralax;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Main.mainCamera.ParalaxedTransform(paralax), samplerState: SamplerState.PointClamp);
            if (visible)
            {
                foreach (ILayeredComponent draw in Drawables)
                {
                    draw.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

        }
        public void Update()
        {

        }
    }
}
