using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Entities
{
    public interface IHurtable
    {
        void Damage(DamageSource source);
    }
}
