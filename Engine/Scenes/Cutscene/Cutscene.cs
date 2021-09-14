﻿using Flipsider.Engine.Interfaces;
using Flipsider.GUI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Flipsider.Engine
{
    public class Cutscene : IComponent, ISerializable<Cutscene>
    {
        internal HashSet<TimeStamp> TimeStamps = new HashSet<TimeStamp>();
        private int TimeAnchor;

        public void AddStamp(int Time, params ICutsceneControl[]? control)
        {
            foreach(TimeStamp stamp in TimeStamps)
            {
                if(stamp.Time == Time)
                {
                    List<ICutsceneControl> newControl = new List<ICutsceneControl>();

                    if (stamp.Info != null)
                    {
                        foreach (ICutsceneControl c in stamp.Info)
                        {
                            newControl.Add(c);
                        }
                    }

                    if (control != null)
                    {
                        foreach (ICutsceneControl c in control)
                        {
                            newControl.Add(c);
                        }
                    }
                    return;
                }
            }

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
        public virtual string? ID { get; set; }

        /// <summary>
        /// Length of cutscene
        /// </summary>
        public virtual int Length { get; set; }

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

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Length);
            writer.Write(ID ?? "NaN");

            writer.Write(TimeStamps.Count);

            foreach (TimeStamp stamp in TimeStamps)
            {
                stamp.Serialize(stream);
            }
        }

        public Cutscene Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            Cutscene scene = new Cutscene();
            List<TimeStamp> stamps = new List<TimeStamp>();

            TimeStamp bufferStamp = new TimeStamp();

            scene.Length = reader.ReadInt32();
            scene.ID = reader.ReadString();

            int length = reader.ReadInt32();

            for (int i = 0; i < length; i++)
            {
                stamps.Add(bufferStamp.Deserialize(stream));
            }

            foreach (TimeStamp stamp in stamps)
            {
                scene.AddStamp(stamp.Time, stamp.Info);
            }

            return scene;
        }
    }
}