
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
        public List<IMeshComponent> PrimitiveDrawables = new List<IMeshComponent>();
        //need to abstract all of these lists
        public List<IPixelated> PixelatedDrawables = new List<IPixelated>();

        public int LayerDepth;
        public float parallax;
        public bool visible = true;

        public float SaturationValue = 1f;
        public Vector4 ColorModification = new Vector4(1, 1, 1, 1);

        public string? Name { get; set; }

        public RenderTarget2D PixelationTarget { get; set; }
        public RenderTarget2D LayerTarget { get; set; }

        public RenderTarget2D BPixelationTarget { get; set; }
        public RenderTarget2D BLayerTarget { get; set; }

        public RenderTarget2D APixelationTarget { get; set; }
        public RenderTarget2D ALayerTarget { get; set; }


        public Layer(int ld, float paralax = 0)
        {
            LayerDepth = ld;
            parallax = paralax;
            Name = "Layer " + ld;

            PixelationTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);
            LayerTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);

            BPixelationTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);
            BLayerTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);

            APixelationTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);
            ALayerTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, FlipGame.Renderer.MaxResolution.X, FlipGame.Renderer.MaxResolution.Y);
        }

        public void RenderTargets(SpriteBatch sb)
        {
            EffectParameter? pFactor = EffectCache.LayerColorModification?.Parameters["pixelationFactor"];
            EffectCache.LayerColorModification?.Parameters["saturationValue"]?.SetValue(SaturationValue);
            EffectCache.LayerColorModification?.Parameters["colorModification"]?.SetValue(ColorModification);
            EffectCache.LayerColorModification?.Parameters["resolution"]?.SetValue(FlipGame.Renderer.MaxResolution.ToVector2() / 2);
            EffectCache.LayerColorModification?.Parameters["transform"]?.SetValue(FlipGame.Camera.Position);

            int PixelationAmount = 4;

            void RenderInPixelationClause(float pixelationFactor, Action<SpriteBatch> action, SamplerState? sampler = default, float scale = 1f)
            {
                sampler = sampler == default ? SamplerState.PointClamp : sampler;

                sb.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, transformMatrix: Matrix.CreateScale(scale), samplerState: sampler);

                pFactor?.SetValue(pixelationFactor);

                //EffectCache.LayerColorModification?.CurrentTechnique.Passes[0].Apply();

                action?.Invoke(sb);

                sb.End();
            }

            RenderInPixelationClause(0f, sb =>
            {
                sb.Draw(BLayerTarget, Vector2.Zero, LayerTarget.Bounds, Color.White);
            });

            RenderInPixelationClause(1f, sb =>
            {
                sb.Draw(BPixelationTarget, new Rectangle(0, 0, BLayerTarget.Width * PixelationAmount, BLayerTarget.Height * PixelationAmount), BPixelationTarget.Bounds, Color.White);
            }, SamplerState.PointClamp, 1f / PixelationAmount);

            RenderInPixelationClause(0f, sb =>
            {
                sb.Draw(LayerTarget, Vector2.Zero, LayerTarget.Bounds, Color.White);
            });

            RenderInPixelationClause(1f, sb =>
            {
                sb.Draw(PixelationTarget, new Rectangle(0, 0, BLayerTarget.Width * PixelationAmount, BLayerTarget.Height * PixelationAmount), BPixelationTarget.Bounds, Color.White);
            }, SamplerState.PointClamp, 1f / PixelationAmount);

            RenderInPixelationClause(0f, sb =>
            {
                sb.Draw(ALayerTarget, Vector2.Zero, LayerTarget.Bounds, Color.White);
            });

            RenderInPixelationClause(1f, sb =>
            {
                sb.Draw(APixelationTarget, new Rectangle(0, 0, BLayerTarget.Width * PixelationAmount, BLayerTarget.Height * PixelationAmount), BPixelationTarget.Bounds, Color.White);
            }, SamplerState.PointClamp, 1f / PixelationAmount);

        }

        public void Draw(SpriteBatch sb)
        {
            DrawBefore(sb, true, BPixelationTarget, SamplerState.PointClamp);
            DrawBefore(sb, false, BLayerTarget, SamplerState.LinearClamp);

            OnDraw(sb, true, PixelationTarget, SamplerState.PointClamp);
            OnDraw(sb, false, LayerTarget, SamplerState.PointClamp);

            DrawAfter(sb, true, APixelationTarget, SamplerState.PointClamp);
            DrawAfter(sb, false, ALayerTarget, SamplerState.LinearClamp);
        }

        public void DrawBefore(SpriteBatch spriteBatch, bool Pixelated, RenderTarget2D target, SamplerState? sampler = default)
        {
            FlipGame.Renderer.Graphics?.GraphicsDevice.SetRenderTarget(target);
            FlipGame.Renderer.Graphics?.GraphicsDevice.Clear(Color.Transparent);

            Matrix PixelationTransform = Pixelated ? Matrix.CreateScale(0.5f) : Matrix.CreateScale(1f);

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, transformMatrix: null, samplerState: sampler);

            foreach (IMeshComponent layeredComponent in PrimitiveDrawables)
            {
                if (
                    Pixelated ? (layeredComponent is IPixelated || layeredComponent is IPixelatedMeshComponent) :
                    !(layeredComponent is IPixelated) && !(layeredComponent is IPixelatedMeshComponent))
                    layeredComponent.DrawPrimtiivesBefore(spriteBatch);
            }

            spriteBatch.End();
        }

        public void DrawAfter(SpriteBatch spriteBatch, bool Pixelated, RenderTarget2D target, SamplerState? sampler = default)
        {
            FlipGame.Renderer.Graphics?.GraphicsDevice.SetRenderTarget(target);
            FlipGame.Renderer.Graphics?.GraphicsDevice.Clear(Color.Transparent);

            Matrix PixelationTransform = Pixelated ? Matrix.CreateScale(0.5f) : Matrix.CreateScale(1f);

            sampler = sampler == default ? SamplerState.PointClamp : sampler;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, transformMatrix: FlipGame.Camera.ParallaxedTransform(parallax) * PixelationTransform, samplerState: sampler);

            foreach (IMeshComponent layeredComponent in PrimitiveDrawables)
            {
                if (
                    Pixelated ? (layeredComponent is IPixelated || layeredComponent is IPixelatedMeshComponent) :
                    !(layeredComponent is IPixelated) && !(layeredComponent is IPixelatedMeshComponent))
                    layeredComponent.DrawPrimtiivesAfter(spriteBatch);
            }

            spriteBatch.End();
        }

        public void OnDraw(SpriteBatch spriteBatch, bool Pixelated, RenderTarget2D target, SamplerState? sampler = default)
        {
            FlipGame.Renderer.Graphics?.GraphicsDevice.SetRenderTarget(target);
            FlipGame.Renderer.Graphics?.GraphicsDevice.Clear(Color.Transparent);

            Matrix PixelationTransform = Pixelated ? Matrix.CreateScale(0.5f) : Matrix.CreateScale(1f);

            sampler = sampler == default ? SamplerState.PointClamp : sampler;
            spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, transformMatrix: FlipGame.Camera.ParallaxedTransform(parallax) * PixelationTransform, samplerState: sampler);

            foreach (ILayeredComponent draw in Drawables.ToArray())
            {
                if (Pixelated ? (draw is IPixelated) : !(draw is IPixelated))
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
