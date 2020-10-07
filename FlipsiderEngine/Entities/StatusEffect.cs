using Flipsider.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Entities
{
    public abstract class StatusEffect
    {
        protected StatusEffect(WorldEntity afflicted, double time)
        {
            TimeMax = Time = time;
            Afflicted = afflicted;
            Afflicted.Entity.OnUpdate += UpdateAffliction;
        }

        private void UpdateAffliction()
        {
            if (Time == TimeMax)
            {
                OnInflict();
            }
            else
            {
                OnUpdate();
            }

            if (Time <= TimeMax)
            {
                OnEnd();
                End();
            }
            Time -= Core.Time.DeltaD;
        }

        public double Time { get; protected set; }
        public double TimeMax { get; }
        public WorldEntity Afflicted { get; }

        public void End()
        {
            Afflicted.Entity.OnUpdate -= UpdateAffliction;
        }

        protected virtual void OnInflict() { }
        protected virtual void OnEnd() { }
        protected virtual void OnUpdate() { }
    }
}
