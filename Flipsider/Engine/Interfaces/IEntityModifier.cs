using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Flipsider.Engine.Interfaces
{
    public interface IEntityModifier
    {
        public void Update(in Entity entity);
    }
}
