using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;

namespace Flipsider
{
    public class Projectile : Entity
    {
        public bool pickable;
        public bool hostile;
        public float alpha;
        public float rotation;
        protected virtual void SetDefaults()
        {

        }
    }
}
