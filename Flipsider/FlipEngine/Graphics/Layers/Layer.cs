
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public class Layer : IComponent, ISerializable<Layer>
    {
        public List<ILayeredComponent> Drawables = new List<ILayeredComponent>();
        public List<ILayeredComponent> PrimitiveDrawables = new List<ILayeredComponent>();

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
                foreach (ILayeredComponent layeredComponent in PrimitiveDrawables)
                {
                    layeredComponent.Draw(spriteBatch);
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
