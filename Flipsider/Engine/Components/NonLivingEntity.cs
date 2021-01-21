using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public abstract class NonLivingEntity : Entity
    {
        protected NonLivingEntity() : base()
        {
            AddModule(new Collideable(this, true));
        }
    }
}
