
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FlipEngine
{
    public class Layer : IComponent, ISerializable<Layer>
    {
        public HashSet<ILayeredComponent> Drawables = new HashSet<ILayeredComponent>();
        public List<IPrimitiveLayeredComponent> PrimitiveDrawables = new List<IPrimitiveLayeredComponent>();

        public int LayerDepth;
        public float parallax;
        public bool visible = true;

        public float SaturationValue = 1f;
        public Vector4 ColorModification = new Vector4(1, 1, 1, 1);

        public string? Name { get; set; }

        public Layer(int ld, float paralax = 0)
        {
            LayerDepth = ld;
            parallax = paralax;
            Name = "Layer " + ld;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            { 
                foreach (IPrimitiveLayeredComponent layeredComponent in PrimitiveDrawables)
                {
                    layeredComponent.DrawPrimtiivesBefore(spriteBatch);
                }
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, transformMatrix: FlipGame.Camera.ParallaxedTransform(parallax), samplerState: SamplerState.PointClamp);

            EffectCache.LayerColorModification?.Parameters["saturationValue"]?.SetValue(SaturationValue);
            EffectCache.LayerColorModification?.Parameters["colorModification"]?.SetValue(ColorModification);
            EffectCache.LayerColorModification?.CurrentTechnique.Passes[0].Apply();

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
            }

            spriteBatch.End();

            if (visible)
            {
                foreach (IPrimitiveLayeredComponent layeredComponent in PrimitiveDrawables)
                {
                    layeredComponent.DrawPrimtiivesAfter(spriteBatch);
                }
            }
        }
        public void Update() { }

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
        }

        public Layer Deserialize(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
