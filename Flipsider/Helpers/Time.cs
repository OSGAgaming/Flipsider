using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Flipsider
{
    public static class Time
    {
        public static double DeltaTime(this GameTime gt) => gt.ElapsedGameTime.TotalSeconds;

        public static float DeltaT => (float)Main.gameTime.DeltaTime();

        public static float DeltaVar(float mult) => (float)Main.gameTime.DeltaTime() * mult;

        public static float DeltaTimeRoundedVar(float mult, int nearest) => NumericalHelpers.Round(mult, nearest);
        public static float TotalTimeMil => (float)Main.gameTime.TotalGameTime.TotalMilliseconds;
        public static float TotalTimeSec => (float)Main.gameTime.TotalGameTime.TotalSeconds;

        public static float QuickDelta => (float)Main.gameTime.DeltaTime() * 60;
    }
}
