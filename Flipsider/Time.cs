using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider
{
    public static class Time
    {
        public static double DeltaTime(this GameTime gt) => gt.ElapsedGameTime.TotalSeconds;
    }
}
