using System;
using System.Collections.Generic;
using System.Text;

using FMOD;
using FMOD.Studio;

namespace Flipsider.Engine.Audio
{
    public class GameAudio
    {
        public static GameAudio Instance;

        static GameAudio()
        {
            Instance = new GameAudio();
        }

        private FMOD.Studio.System _audioSystem;
        private Dictionary<string, AudioBank> _banks;

        private VCA _musicVCA;
        private VCA _sfxVCA;

        public float MusicVolume
        {
            get
            {
                _musicVCA.getVolume(out float vol);
                return vol;
            }
            set
            {
                _musicVCA.setVolume(value);
            }
        }
        public float SFXVolume
        {
            get
            {
                _sfxVCA.getVolume(out float vol);
                return vol;
            }
            set
            {
                _sfxVCA.setVolume(value);
            }
        }

        public GameAudio()
        {
            FMOD.Studio.System.create(out _audioSystem);
            _audioSystem.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero).CheckOK();

            _audioSystem.getVCA("vca:/music", out _musicVCA);
            _audioSystem.getVCA("vca:/sfx", out _sfxVCA); //TODO: Add .CheckOK() to these just to be safe, at time of writing no bank exists yet.
        }

        public void LoadBank(string internalName, string dirInContent)
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\Content\\" + dirInContent;

            _banks[internalName] = new AudioBank(_audioSystem, file);
        }

        public void Unload()
        {
            _audioSystem.release();
            foreach(AudioBank bank in _banks.Values)
            {
                bank.Unload();
            }
        }

        public AudioBank this[string name] => _banks[name];
    }

    public class AudioBank
    {
        private Bank _bank;
        private EventDescription[] _events;
        private Dictionary<string, EventDescription> _eventDict;

        public AudioBank(FMOD.Studio.System system, string file)
        {
            system.loadBankFile(file, LOAD_BANK_FLAGS.NORMAL, out _bank).CheckOK();

            _bank.getEventList(out _events);
            _eventDict = new Dictionary<string, EventDescription>();
            for (int i = 0; i < _events.Length; i++)
            {
                if (_events[i].isValid())
                {
                    _events[i].getPath(out string path);
                    _eventDict[path] = _events[i];
                }
            }
        }

        public void Unload()
        {
            _bank.unload();
        }

        public void PlayOneShot(string path)
        {
            GetEventInstance(path).StartOneShot();
        }

        public EventInstance GetEventInstance(string path)
        {
            _eventDict[path].createInstance(out var instance).CheckOK();
            return instance;
        }

        public EventDescription this[string name] => _eventDict[name];
    }
}
