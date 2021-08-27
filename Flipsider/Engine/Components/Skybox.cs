using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public class Skybox : Engine.Interfaces.IDrawable
    {
        internal List<ParalaxLayer> Layers = new List<ParalaxLayer>();

        public Skybox()
        {
            LoadSkyboxes();
        }

        public virtual void LoadSkyboxes() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(ParalaxLayer Layer in Layers)
            {
                if (Layer.Path != null)
                {
                    Texture2D tex = Assets.GetTexture(Layer.Path);
                    Utils.RenderBG(spriteBatch, Color.White, tex, Layer.Parallax, Layer.Scale, Layer.Offset);
                }
            }
        }
    }
}
