
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
        public int alteredWidth => PropTypes[prop].Width / PropEntites[prop].noOfFrames;
        public Vector2 ParallaxedCenter => Center.AddParallaxAcrossX(-Main.layerHandler.Layers[Layer].parallax);
        public int frameX => PropEntites[prop].animSpeed == -1 ? 0 : (frameCounter / PropEntites[prop].animSpeed) % PropEntites[prop].noOfFrames;
        public Rectangle alteredFrame => new Rectangle(frameX * alteredWidth, 0, alteredWidth, PropTypes[prop].Height);
    }
}
