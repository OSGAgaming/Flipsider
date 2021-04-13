using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    internal delegate void LightTargetEvent();
    public class Lighting
    {
        public Map Maps;
        public Lighting(ContentManager Content)
        {
            Maps = new Map();

            Maps.AddMap("Leaves", 0, new LeavesPass());
            Maps.AddMap("Bloom", 1, new BloomMap());
            Maps.AddMap("CanLightMap", 2, new LightingOcclusionMap());
            Maps.AddMap("Lighting", 3, new LightingMap());
            Maps.AddMap("FGWater", 4, new FgWaterPass());
            Maps.AddMap("GodRay", 5, new GodrayMap());
        }

    }
}