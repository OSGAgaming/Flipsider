using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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

            foreach (Type t in Utils.GetInheritedClasses(typeof(MapPass)))
            {
                MapPass? state = (MapPass?)Activator.CreateInstance(t);

                if (state != null) Maps?.AddMap(t.Name, 0, state);
            }
        }
    }
}