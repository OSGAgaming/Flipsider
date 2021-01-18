
using Flipsider;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public class LayerHandler
    {
        public List<Layer> Layers = new List<Layer>();
        public static int CurrentLayer = 0;
        public int PlayerLayer => Main.player.Layer;
        public void DrawLayers(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            foreach (Layer layer in Layers)
            {
                layer.Draw(spriteBatch);
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
        }
        public void AppendMethodToLayer(ILayeredComponent Method)
        =>
        Layers[Method.Layer].Drawables.Add(Method);
        public void AppendPrimitiveToLayer(ILayeredComponent Method)
        =>
        Layers[Method.Layer].PrimitiveDrawables.Add(Method);
        public void AddLayer()
        {
            Layers.Add(new Layer(Layers.Count));
        }
        public void SwitchLayerVisibility(int Layer)
        {
            if (Layer < Layers.Count)
                Layers[Layer].visible = !Layers[Layer].visible;
        }
        public void SetEntityDrawLayer()
        {

        }
        public void SetLayerParallax(int Layer, float paralax)
        {
            if (Layer < Layers.Count)
                Layers[Layer].parallax = paralax;
        }
        public int GetLayerCount() => Layers.Count;
    }
}
