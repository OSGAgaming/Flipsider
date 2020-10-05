using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace Flipsider.Core
{
    public static class Time
    {
        public static double TimeScale { get; set; } = 1;

        public static TimeSpan Total => FlipsiderGame.time.TotalGameTime;

        /// <summary>
        /// Delta time.
        /// </summary>
        public static TimeSpan DeltaT => FlipsiderGame.time.ElapsedGameTime;

        /// <summary>
        /// Delta time as a double.
        /// </summary>
        public static double DeltaD => DeltaT.TotalSeconds * TimeScale;

        /// <summary>
        /// Delta time as a float.
        /// </summary>
        public static float DeltaF => (float)DeltaD;
    }
}
