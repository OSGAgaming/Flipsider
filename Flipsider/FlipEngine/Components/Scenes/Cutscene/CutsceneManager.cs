using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlipEngine
{
    public class CutsceneManager : ILoadUpdate
    {
        private Cutscene? currentCutscene;

        public static CutsceneManager Instance;

        static CutsceneManager() 
        { 
            Instance = new CutsceneManager();
        }

        public CutsceneManager() {  }

        public bool IsPlayingCutscene => currentCutscene != null;

        public void StartCutscene(Cutscene scene) 
        {
            if (scene == null) return;

            currentCutscene = scene;
            currentCutscene.OnActivate();
        }

        public void Update()
        {
            if(currentCutscene != null)
            {
                currentCutscene.Update();

                if(currentCutscene.Time >= currentCutscene.Length)
                {
                    currentCutscene.OnDeactivate();
                    currentCutscene = null;
                }
            }
        }
    }
}
