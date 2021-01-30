using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    [Serializable]
    public struct LayerManagerInfo
    {
        public int NumberOfLayers;
        public float[] LayerParalax;
        public LayerManagerInfo(int N, float[] L)
        {
            NumberOfLayers = N;
            LayerParalax = L;
        }
        public LayerHandler Load()
        {
            LayerHandler layerHandler = new LayerHandler();
            for(int i = 1; i<NumberOfLayers; i++)
            {
                layerHandler.AddLayer();
            }
            for (int i = 0; i < NumberOfLayers; i++)
            {
                layerHandler.SetLayerParallax(i,LayerParalax[i]);
            }
            return layerHandler;
        }
    }
    [Serializable]
    public class LayerHandler
    {
        public LayerHandler()
        {
            AddLayer();
            RTGaming = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }
        [NonSerialized]
        public List<Layer> Layers = new List<Layer>();
        public static int CurrentLayer = 0;
        public static int[] LayerCache = { 1, 1 };
        public RenderTarget2D RTGaming;
        public int PlayerLayer => Main.player.Layer;
        internal LayerManagerInfo Info
        {
            get
            {
                float[] paralaxLayer = new float[Layers.Count];
                for (int i = 0; i < Layers.Count; i++)
                {
                    paralaxLayer[i] = Layers[i].parallax;
                }
                return new LayerManagerInfo(Layers.Count, paralaxLayer);
            }
        }
        public LayerManagerInfo InfoCache;
        public void DrawLayers(SpriteBatch spriteBatch)
        {

            InfoCache = Info;
            Main.graphics.GraphicsDevice.SetRenderTarget(RTGaming);
            Main.graphics.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.End();
            foreach (Layer layer in Layers)
            {
                layer.Draw(spriteBatch);
            }
            Main.graphics.GraphicsDevice.SetRenderTarget(Main.renderer.renderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);

        }
        public void AppendMethodToLayer(ILayeredComponent Method)
        =>
        Layers[Method.Layer].Drawables.Add(Method);
        public void AutoAppendMethodToLayer(ref ILayeredComponent Method)
        {
            if (Layers.Count > 0)
            {
                Method.Layer = CurrentLayer;
                Layers[Method.Layer].Drawables.Add(Method);
            }
        }
        public void AppendPrimitiveToLayer(ILayeredComponent Method)
        =>
        Layers[Method.Layer].PrimitiveDrawables.Add(Method);
        public void AddLayer()
        {
            Layers.Add(new Layer(Layers.Count));
        }
        public void AddLayer(float paralax)
        {
            Layer layer = new Layer(Layers.Count);
            layer.parallax = paralax;
            Layers.Add(layer);
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
            for (int i = 0; i < Layers[Layer1].PrimitiveDrawables.Count; i++)
            {
                Layers[Layer1].PrimitiveDrawables[i].Layer = Layer2;
                L1.Add(Layers[Layer1].PrimitiveDrawables[i]);
            }
            for (int i = 0; i < Layers[Layer2].PrimitiveDrawables.Count; i++)
            {
                Layers[Layer2].PrimitiveDrawables[i].Layer = Layer1;
                L2.Add(Layers[Layer2].PrimitiveDrawables[i]);
            }
            Layers[Layer1].Drawables.Clear();
            Layers[Layer2].Drawables.Clear();
            Layers[Layer1].PrimitiveDrawables.Clear();
            Layers[Layer2].PrimitiveDrawables.Clear();
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
