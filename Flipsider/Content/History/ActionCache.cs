using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Flipsider.Engine;
using System.Collections.Generic;
using Flipsider.Engine.Input;
using Microsoft.Xna.Framework.Input;
using Flipsider.GUI.TilePlacementGUI;

namespace Flipsider
{
    public class ActionCache : IUpdate
    {
        private const int MaxUndoHistory = 50;

        public List<ActionSet> Actions = new List<ActionSet>();

        public HashSet<Entity> EntityQueue = new HashSet<Entity>();

        private int delay = 0;

        private int delayTime = 60;
        private int ctrlZTime;
        private int ctrlYTime;
        public bool CanAddToCache { get; set; } = true;

        public void Load()
        {
            for (int i = 0; i < MaxUndoHistory; i++) { Actions.Add(new ActionSet()); }
        }

        public void Clear()
        {
            for (int i = 0; i < MaxUndoHistory; i++) { Actions[i].Clear(); }
        }

        void UpdateCache()
        {
            CurrentPosition++;
            if (CanAddToCache)
            {
                for (int i = CurrentPosition; i < MaxUndoHistory; i++)
                {
                    Actions[i].Clear();
                }
            }
        }
        public int CurrentPosition { get; set; } = 0;

        public void AddQueueToCache(int index = -1)
        {
            if (index == -1) index = CurrentPosition;

            if (index >= MaxUndoHistory) return;

            Actions[CurrentPosition].EntityList.Clear();

            foreach (Entity entity in EntityQueue)
            {
                Actions[index].EntityList.Add(entity);
            }

            if (index == MaxUndoHistory - 1)
            {
                Actions.RemoveAt(0);
                Actions.Add(new ActionSet());
                return;
            }
        }

        public void UndoCurrent()
        {
            if (CurrentPosition != 0)
            {
                if (Actions[CurrentPosition - 1].EntityList.Count != 0)
                {
                    if (delay == 0)
                    {
                        Actions[CurrentPosition - 1].Undo();
                        CurrentPosition--;
                        delay = delayTime;
                    }
                }
            }
        }

        public void RedoCurrent()
        {
            if (CurrentPosition != MaxUndoHistory + 1 && delay == 0)
            {
                if (Actions[CurrentPosition].EntityList.Count != 0)
                {
                    Actions[CurrentPosition].Redo();
                    if (EntityQueue.Count > 0) AddQueueToCache(CurrentPosition);
                    CurrentPosition++;
                    delay = delayTime;
                }
            }

        }

        int[] ECBuffer = new int[2];

        bool CacheHasChanged;

        List<bool> CacheChanges = new List<bool>();
        public void Update()
        {
            CacheHasChanged = false;

            if (ECBuffer[0] != EntityQueue.Count)
                CacheHasChanged = true;

            CacheChanges.Add(CacheHasChanged);

            if (CacheChanges.Count > 1)
            {
                bool C1 = CacheChanges[CacheChanges.Count - 1];
                bool C2 = CacheChanges[CacheChanges.Count - 2];
                if (!C1 && C2)
                {
                    UpdateCache();
                    EntityQueue.Clear();
                }
            }

            if (EntityQueue.Count > 0 && CanAddToCache) AddQueueToCache();
            CanAddToCache = true;

            if (!GameInput.Instance.IsClicking && !GameInput.Instance.PreviousIsClicking)
            {
                EntityQueue.Clear();
            }

            if (delay > 0) delay--;

            if (ctrlZTime >= 60 || ctrlYTime >= 60) delayTime = 15;
            else delayTime = 60;

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.LeftControl) && state.IsKeyDown(Keys.Z)) { UndoCurrent(); ctrlZTime++; }
            else ctrlZTime = 0;



            if (state.IsKeyDown(Keys.LeftControl) && state.IsKeyDown(Keys.Y)) { RedoCurrent(); ctrlYTime++; }
            else ctrlYTime = 0;

            ECBuffer[0] = EntityQueue.Count;
            //for (int i = 0; i < MaxUndoHistory; i++) { Logger.NewText($"{i}: " + Actions[i].EntityList.Count); }
        }

        public int GetLastIndex() => Actions.Count;

        public static ActionCache Instance;

        public ActionCache()
        {
            Load();
            Main.UpdateablesOffScreen.Add(this);
        }
        static ActionCache()
        {
            Instance = new ActionCache();
        }
    }
}
