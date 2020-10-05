using System;
using System.Collections.Generic;
using System.Text;

using FMOD;
using FMOD.Studio;

namespace Flipsider.Audio
{
    public static class AudioExtensions
    {
        public static void CheckOK(this RESULT result)
        {
            if (result != RESULT.OK) throw new Exception(result.ToString());
        }

        public static void StartOneShot(this EventInstance ev)
        {
            ev.start();
            ev.release();
        }
    }
}
