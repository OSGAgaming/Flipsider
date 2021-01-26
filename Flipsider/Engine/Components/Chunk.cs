
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class Chunk : IUpdate
    {
        public static int ChunkSize => 1000;

        public List<Entity> Entities = new List<Entity>();

        public int I;

        public int J;

        public bool Active;

        public Chunk(int I, int J)
        {
            this.I = I;
            this.J = J;
        }
        public void Update()
        {
            if (Active)
            {
                foreach (Entity entity in Entities)
                {
                    entity.Update();
                }
            }
        }
    }
}
