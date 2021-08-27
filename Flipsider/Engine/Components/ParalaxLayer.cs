using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider
{
    public struct ParalaxLayer
    {
        public int Priority;
        public float Parallax;
        public float Scale;
        public string? Path;
        public Vector2 Offset;

        public ParalaxLayer(string? Path, float Parallax = 1, int Priority = 0, Vector2 Offset = default, float Scale = 1)
        {
            this.Path = Path;
            this.Priority = Priority;
            this.Parallax = Parallax;
            this.Scale = Scale;
            this.Offset = Offset;
        }
    }
}
