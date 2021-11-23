using FlipEngine;
using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class CitySkybox : Skybox
    {
        public override void LoadSkyboxes()
        {
            Layers.Add(new ParalaxLayer(@"Backgrounds\CityBackground1", -0.9f, 0, new Vector2(0, Utils.BOTTOM), 3f));
            Layers.Add(new ParalaxLayer(@"Backgrounds\CityBackground2", -0.8f, 0, new Vector2(0, Utils.BOTTOM), 3f));
        }
    }
}
