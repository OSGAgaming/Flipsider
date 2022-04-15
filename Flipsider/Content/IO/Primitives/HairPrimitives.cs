
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    public class HairPrimtives : DualTriangleQuad
    {
        private CorePart part;

        public HairPrimtives(CorePart part, Texture2D tex) : base(tex)
        {
            this.part = part;
        }

        protected override void AddPoints()
        {
            WidthFallOff = 1;
            Width = 6;
            for (int i = part.MainVerletPoint; i < part.MainVerletPoint + part.HairPoints; i++)
            {
                _points.Add(Verlet.Instance.points[i].point);
            }
        }
    }
}