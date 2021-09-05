
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlipEngine
{
    public abstract partial class LivingEntity : Entity
    {
        public virtual void ApplyForces() { ; }

        public void UpdatePosition() => Position += velocity * Time.DeltaVar(120);
    }
}
