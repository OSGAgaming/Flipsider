using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flipsider.Core;
using Flipsider.Graphics;
using Flipsider.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider
{
    public class SceneManager : IUpdated, IDrawn
    {
        private Scene? _currentScene;
        private Scene? _nextScene;
        private SceneTransition? _transitionToUse;
        private float _transitionProgress;
        private bool _transitioning;
        private bool _transitionSwitchedScene;

        public Scene? Scene
        {
            get
            {
                return _currentScene;
            }
            set
            {
                if (_currentScene != value)
                {
                    SetNextScene(value, null, true);
                }
            }
        }

        public void SetNextScene(Scene? scene, SceneTransition? transition = null, bool startTransition = true)
        {
            _nextScene = scene;
            _transitionToUse = transition;
            if (startTransition)
            {
                StartTransition();
            }
        }

        void IUpdated.Update()
        {
            if (_transitioning)
            {
                if (!_transitionSwitchedScene && _transitionProgress > _transitionToUse?.SwitchPoint)
                {
                    _transitionSwitchedScene = true;
                    SwitchScene();
                }
                _transitionProgress += Time.DeltaF;
            }

            _currentScene?.Update();
        }

        public void StartTransition()
        {
            if (_nextScene == null || _transitioning)
            {
                return;
            }

            if (_transitionToUse == null)
            {
                SwitchScene();
                return;
            }

            _transitionProgress = 0f;
            _transitioning = true;
            _transitionSwitchedScene = false;
        }

        private void SwitchScene()
        {
            //deactivate current scene (if not null), set next scene, then activate it
            _currentScene?.OnDeactivate();
            _currentScene = _nextScene;
            _currentScene?.OnActivate();
            _nextScene = null;
        }

        void IDrawn.Draw(SafeSpriteBatch sb)
        {
            _currentScene?.Draw(sb);

            if (_transitioning && _transitionToUse != null)
            {
                //the progress is from 0-1 so we need to get it via that.
                float progress = _transitionProgress / _transitionToUse.Length;
                _transitionToUse.Draw(sb, progress);
            }
        }
    }
}
