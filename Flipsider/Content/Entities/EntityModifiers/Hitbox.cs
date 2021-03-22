
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public partial class HitBox : IEntityModifier
    {
        public int HitBoxGrouping { get; }

        LivingEntity? LE;
        public void Update(in Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollide()
        {
            
        }
    }
}
