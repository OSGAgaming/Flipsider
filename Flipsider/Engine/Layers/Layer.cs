using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider
{
    [Serializable]
    public class Layer : IComponent
    {
        [NonSerialized]
        public List<ILayeredComponent> Drawables = new List<ILayeredComponent>();
        [NonSerialized]
        public List<ILayeredComponent> PrimitiveDrawables = new List<ILayeredComponent>();
        public int LayerDepth;
        public float parallax;
        public bool visible = true;

        public Layer(int ld, float paralax = 0)
        {
            LayerDepth = ld;
            parallax = paralax;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                foreach (ILayeredComponent layeredComponent in PrimitiveDrawables)
                {
                    layeredComponent.Draw(spriteBatch);
                }
            }
            spriteBatch.Begin(transformMatrix: Main.mainCamera.ParallaxedTransform(parallax), samplerState: SamplerState.PointClamp);
            if (visible)
            {
                    foreach (ILayeredComponent draw in Drawables.ToArray())
                    {
                        if (draw is IDrawData)
                        {
                            var Drawable = draw as IDrawData;
                            if (Drawable != null)
                            {
                                if (Drawable.InFrame)
                                {
                                    Vector2 dist = Main.player.Center - Drawable.drawData.position;
                                    DrawData d = Drawable.drawData;
                                    spriteBatch.DrawOffset(d, Vector2.Zero);
                                }
                            }
                        }
                        else
                        {
                            draw.Draw(spriteBatch);
                        }
                    }
                if (Main.player.Layer == LayerDepth + 1)
                {
                    Main.player.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
        public void Update()
        {

        }
    }
}
