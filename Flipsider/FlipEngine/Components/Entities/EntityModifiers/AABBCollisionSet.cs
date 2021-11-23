using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public class AABBCollisionSet : ISerializable<AABBCollisionSet>
    {
        public HashSet<RectangleF> AABBs = new HashSet<RectangleF>();

        public AABBCollisionSet Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            HashSet<RectangleF> temp = new HashSet<RectangleF>();

            for (int i = 0; i < binaryReader.ReadInt32(); i++)
            {
                temp.Add(binaryReader.ReadRectF());
            }

            AABBCollisionSet set = new AABBCollisionSet()
            { 
                AABBs = temp
            };

            return set;
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            binaryWriter.Write(AABBs.Count);

            foreach (RectangleF r in AABBs)
            {
                binaryWriter.Write(r);
            }
        }
    }
}