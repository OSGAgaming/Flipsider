using System;
using System.Collections.Generic;
using System.Text;

using FMOD;
using FMOD.Studio;

namespace Flipsider.Engine.Audio
{
    /// <summary>
    /// This object is used to store information about an audio track, like music or ambience.
    /// </summary>
    public class AudioTrack
    {
        private EventInstance _instance;
        private string _bank;
        private string _track;

        public AudioTrack(string bank, string track)
        {
            _bank = bank;
            _track = track;
        }

        public void Play()
        {
            GameAudio.Instance[_bank][_track].createInstance(out _instance).CheckOK();
            _instance.start();
        }

        public void Pause()
        {
            _instance.setPaused(true).CheckOK();
        }

        public void SetParameter(string paramName, float value, bool immediate = false)
        {
            _instance.setParameterByName(paramName, value, immediate).CheckOK();
        }

        public void Stop(bool fadeOut = true)
        {
            _instance.stop(fadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE).CheckOK();
            _instance.release();
        }
    }
}
