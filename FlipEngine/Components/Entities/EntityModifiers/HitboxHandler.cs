

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine.Engine.Maths
{
    public class HitBoxHandler
    {
        public HashSet<HitBox> HitBoxes = new HashSet<HitBox>();
        public void RemoveThroughEntity(Entity entity)
        {
            HitBox[] Buffer = new HitBox[HitBoxes.Count];
            HitBoxes.CopyTo(Buffer);
            foreach (HitBox hitbox in Buffer)
            {
                if (ReferenceEquals(entity, hitbox.LE))
                {
                    hitbox.Dispose();
                }
            }
        }
    }
}
