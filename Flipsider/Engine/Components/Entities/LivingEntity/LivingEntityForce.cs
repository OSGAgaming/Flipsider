using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider
{
    public abstract partial class LivingEntity : Entity
    {
        public virtual void ApplyForces() { ; }

        public void UpdatePosition() => position += velocity * Time.DeltaVar(120);
    }
}
