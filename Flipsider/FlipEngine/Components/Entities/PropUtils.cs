

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace FlipEngine
{
    public partial class Prop : Entity
    {
        public string prop => Encoding.UTF8.GetString(PropEncode, 0, PropEncode.Length);
        public Vector2 ParallaxedCenter => Center.AddParallaxAcrossX(-FlipGame.layerHandler.Layers[Layer].parallax);
    }
}
