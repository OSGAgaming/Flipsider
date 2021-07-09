using Flipsider.Engine.Interfaces;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flipsider.Engine
{
    public class Cutscene : IComponent
    {
        private HashSet<TimeStamp> TimeStamps = new HashSet<TimeStamp>();
        private int CurrentStamp;
        private int TimeAnchor;

        public void AddStamp(int Time, params ICutsceneControl[]? control)
        {
            TimeStamp t = new TimeStamp();
            t.Time = Time;
            t.Info = control;

            TimeStamps.Add(t);
        }

        private TimeStamp? FindStamp()
        {
            TimeStamp? stamp = null;
            int lowest = Length + 1;

            foreach(TimeStamp TS in TimeStamps)
            {
                int difference = TS.Time - Time;

                if (difference < lowest && difference >= 0)
                {
                    lowest = difference;
                    stamp = TS;
                }
            }

            return stamp;
        }
        /// <summary>
        /// Name of the scene for easy Accessibility
        /// </summary>
        public virtual string? ID { get; }

        /// <summary>
        /// Length of cutscene
        /// </summary>
        public virtual int Length { get; }

        /// <summary>
        /// Progress of cutscene
        /// </summary>
        public int Time { get; private set; }

        /// <summary>
        /// Method called when the scene has just been activated.
        /// </summary>
        public virtual void OnActivate() { }

        /// <summary>
        /// Method called when the scene has just been deactivated and is no longer the active scene.
        /// </summary>
        public virtual void OnDeactivate() { }

        /// <summary>
        /// Method called to update the scene.
        /// </summary>
        public virtual void Update()
        {
            TimeStamp? stamp = FindStamp();

            if (stamp != null)
            {
                CurrentStamp = stamp.Time;

                if (Time == stamp.Time) TimeAnchor = stamp.Time;

                if (stamp.Info != null)
                {
                    foreach (ICutsceneControl icc in stamp.Info)
                    {
                        if ((stamp.Time - TimeAnchor) != 0)
                        {
                            float interpValue = (Time - TimeAnchor) / (float)(stamp.Time - TimeAnchor);
                            icc.Send(interpValue);
                        }
                    }
                }
            }

            Time++;
        }

        /// <summary>
        /// Method called to draw the scene.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
