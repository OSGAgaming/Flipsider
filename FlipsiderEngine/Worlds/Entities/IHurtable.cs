using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Worlds.Entities
{
    public interface IHurtable
    {
        void Damage(DamageSource source);
    }
}
