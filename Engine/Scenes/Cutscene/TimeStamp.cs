using Flipsider.Engine.Interfaces;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider.Engine
{
    public class TimeStamp : ISerializable<TimeStamp>
    {
        public int Time;

        public ICutsceneControl[]? Info;

        public TimeStamp Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            List<ICutsceneControl> controllers = new List<ICutsceneControl>();

            int Time = reader.ReadInt32();
            bool nullInfo = reader.ReadBoolean();

            if (nullInfo)
            {
                int length = reader.ReadInt32();

                for (int i = 0; i < length; i++)
                {
                    Type? type = Type.GetType(reader.ReadString());
                    if (type != null)
                    {
                        ICutsceneControl? icc = Activator.CreateInstance(type) as ICutsceneControl;

                        if (icc != null) controllers.Add(icc.Deserialize(stream));
                    }
                }
            }

            TimeStamp stamp = new TimeStamp();

            stamp.Time = Time;
            stamp.Info = controllers.ToArray();

            return stamp;
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Time);
            writer.Write(Info != null);

            if (Info != null)
            {
                writer.Write(Info.Length);

                foreach (ICutsceneControl icc in Info)
                {
                    writer.Write(icc.GetType().FullName ?? "Empty");
                    icc.Serialize(stream);
                }
            }
        }
    }
}
