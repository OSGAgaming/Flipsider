using FlipEngine;
using Microsoft.Xna.Framework;
using System;

namespace Flipsider
{
    public static class Time
    {
        public static double DeltaTime(this GameTime gt) => gt.ElapsedGameTime.TotalSeconds;
        public static float SineTime(float period, float displacement = 0) => (float)Math.Sin(FlipE.gameTime.TotalGameTime.TotalSeconds * period + displacement);
        public static float DeltaT => (float)FlipE.gameTime.DeltaTime();

        public static float DeltaVar(float mult) => (float)FlipE.gameTime.DeltaTime() * mult;

        public static float DeltaTimeRoundedVar(float mult, int nearest) => Utils.Round(mult, nearest);
        public static float TotalTimeMil => (float)FlipE.gameTime.TotalGameTime.TotalMilliseconds;
        public static float TotalTimeSec => (float)FlipE.gameTime.TotalGameTime.TotalSeconds;

        public static float QuickDelta => (float)FlipE.gameTime.DeltaTime() * 60;
    }
}
