using FMOD;
using FMOD.Studio;
using System;

namespace FlipEngine
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
