using Flipsider.Engine.Maths;

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
