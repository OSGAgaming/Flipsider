
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using Flipsider.Engine.Interfaces;
using static Flipsider.PropManager;
using System.IO;

namespace Flipsider
{
    public partial class Prop : Entity
    {
        public string prop => Encoding.UTF8.GetString(propEncode, 0, propEncode.Length);
        public Vector2 ParallaxedCenter => Center.AddParallaxAcrossX(-Main.layerHandler.Layers[Layer].parallax);
    }
}
