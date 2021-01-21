
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
        public static int[] LayerCache = { 1, 1 };
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
        public void AutoAppendMethodToLayer(ref ILayeredComponent Method)
        {
            Method.Layer = CurrentLayer;
            Layers[Method.Layer].Drawables.Add(Method);
        }
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
        public void SwitchLayers(int Layer1, int Layer2)
        {

            List<ILayeredComponent> L1 = new List<ILayeredComponent>();
            List<ILayeredComponent> L2 = new List<ILayeredComponent>();
            for (int i = 0; i < Layers[Layer1].Drawables.Count; i++)
            {
                Layers[Layer1].Drawables[i].Layer = Layer2;
                L1.Add(Layers[Layer1].Drawables[i]);
            }
            for (int i = 0; i < Layers[Layer2].Drawables.Count; i++)
            {
                Layers[Layer2].Drawables[i].Layer = Layer1;
                L2.Add(Layers[Layer2].Drawables[i]);
            }
            Layers[Layer1].Drawables.Clear();
            Layers[Layer2].Drawables.Clear();
            for (int i = 0; i < L1.Count; i++)
            {
                AppendMethodToLayer(L1[i]);
            }
            for (int i = 0; i < L2.Count; i++)
            {
                AppendMethodToLayer(L2[i]);
            }
            float buffer1 = Layers[Layer1].parallax;
            float buffer2 = Layers[Layer2].parallax;
            SetLayerParallax(Layer1, buffer2);
            SetLayerParallax(Layer2, buffer1);
        }
        public void SetLayerParallax(int Layer, float paralax)
        {
            if (Layer < Layers.Count)
                Layers[Layer].parallax = paralax;
        }
        public int GetLayerCount() => Layers.Count;
    }
}
