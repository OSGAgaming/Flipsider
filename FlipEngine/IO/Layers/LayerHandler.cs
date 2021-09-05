using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlipEngine
{
    public struct LayerManagerInfo : ISerializable<LayerManagerInfo>
    {
        public int NumberOfLayers;
        public float[] LayerParalax;
        public Vector4[] LayerColor;
        public float[] Saturation;

        public LayerManagerInfo(int N, float[] L, Vector4[] C, float[] S)
        {
            NumberOfLayers = N;
            LayerParalax = L;
            LayerColor = C;
            Saturation = S;
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            binaryWriter.Write(NumberOfLayers);

            for (int i = 0; i < LayerParalax.Length; i++)
            {
                binaryWriter.Write(LayerParalax[i]);
                binaryWriter.Write(LayerColor[i]);
                binaryWriter.Write(Saturation[i]);

            }
        }
        public LayerManagerInfo Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            int N = binaryReader.ReadInt32();

            float[] L = new float[N];
            Vector4[] C = new Vector4[N];
            float[] S = new float[N];

            for (int i = 0; i < N; i++)
            {
                L[i] = binaryReader.ReadSingle();
                if (FormatVersion.Version > 1)
                {
                    C[i] = binaryReader.ReadVector4();
                    S[i] = binaryReader.ReadSingle();
                }
            }

            return new LayerManagerInfo(N, L, C, S);
        }

        public LayerHandler Load()
        {
            LayerHandler layerHandler = new LayerHandler();
            for (int i = 1; i < NumberOfLayers; i++)
            {
                layerHandler.AddLayer();
            }
            for (int i = 0; i < NumberOfLayers; i++)
            {
                layerHandler.SetLayerParallax(i, LayerParalax[i]);
                layerHandler.Layers[i].SaturationValue = Saturation[i];
                layerHandler.Layers[i].ColorModification = LayerColor[i];
            }
            return layerHandler;
        }
    }
    public class LayerHandler
    {
        public LayerHandler()
        {
            AddLayer();
            Target = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        }

        public List<Layer> Layers = new List<Layer>();
        public static int CurrentLayer = 0;
        public static int[] LayerCache = { -1, -1 };
        public RenderTarget2D Target;
        internal LayerManagerInfo Info
        {
            get
            {
                float[] paralaxLayer = new float[Layers.Count];
                Vector4[] paralaxColor = new Vector4[Layers.Count];
                float[] paralaxSaturation = new float[Layers.Count];

                for (int i = 0; i < Layers.Count; i++)
                {
                    paralaxLayer[i] = Layers[i].parallax;
                    paralaxColor[i] = Layers[i].ColorModification;
                    paralaxSaturation[i] = Layers[i].SaturationValue;
                }
                return new LayerManagerInfo(Layers.Count, paralaxLayer, paralaxColor, paralaxSaturation);
            }
        }
        public LayerManagerInfo InfoCache;
        public void DrawLayers(SpriteBatch spriteBatch)
        {
            InfoCache = Info;
            Main.graphics.GraphicsDevice.SetRenderTarget(Target);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.End();

            for (int i = 0; i < Layers.Count; i++)
            {
                for (int j = 0; j < Layers.Count; j++)
                {
                    if (Layers[j].LayerDepth == i)
                    {
                        Layers[j].Draw(spriteBatch);
                    }
                }
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(Main.renderer.RenderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.Camera.Transform, samplerState: SamplerState.PointClamp);

        }
        public void AppendMethodToLayer(ILayeredComponent Method)
        {
            GetLayer(Method.Layer).Drawables.Add(Method);
        }
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
        public void SwitchLayerVisibility(int Layer)
        {
            if (Layer < Layers.Count)
                GetLayer(Layer).visible = !GetLayer(Layer).visible;
        }
        public Layer GetLayer(int Layer)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].LayerDepth == Layer)
                {
                    return Layers[i];
                }
            }

            return Layers[0];
        }

        public void RemoveLayer(int Layer)
        {
            if (Layers.Count != 1)
            {
                if (Main.player.Layer == Layer)
                {
                    Logger.NewText("Cannot remove layer with player in it");
                    return;
                }

                Layers.RemoveAt(Layer);

                for (int i = Layer; i < Layers.Count; i++)
                {
                    Layers[i].LayerDepth--;
                }
            }
        }

        public void SwitchLayers(int Layer1, int Layer2)
        {
            Layer L1 = GetLayer(Layer1);
            Layer L2 = GetLayer(Layer2);

            int D1 = L1.LayerDepth;
            int D2 = L2.LayerDepth;

            L1.LayerDepth = D2;
            L2.LayerDepth = D1;
        }
        public void SetLayerParallax(int Layer, float paralax)
        {
            if (Layer < Layers.Count)
                GetLayer(Layer).parallax = paralax;
        }
        public int GetLayerCount() => Layers.Count;
    }
}
