using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Flipsider
{
    public static class BFMExtensions
    {
        public static void Write(this BinaryWriter binaryWriter, Point point)
        {
            binaryWriter.Write(point.X);
            binaryWriter.Write(point.Y);
        }
        public static void Write(this BinaryWriter binaryWriter, Vector2 point)
        {
            binaryWriter.Write(point.X);
            binaryWriter.Write(point.Y);
        }
        public static void Write(this BinaryWriter binaryWriter, Rectangle rect)
        {
            binaryWriter.Write(rect.Location);
            binaryWriter.Write(rect.Size);
        }
        public static void Write(this BinaryWriter binaryWriter, Type type)
        {
            binaryWriter.Write(type?.FullName ?? "throw");
        }
        public static Point ReadPoint(this BinaryReader binaryReader)
        {
            return new Point(binaryReader.ReadInt32(), binaryReader.ReadInt32());
        }
        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
        public static Rectangle ReadRect(this BinaryReader binaryReader)
        {
            return new Rectangle(binaryReader.ReadPoint(), binaryReader.ReadPoint());
        }
        public static Type? ReadType(this BinaryReader binaryReader)
        {
            string TypeName = binaryReader.ReadString();

            if (TypeName == "throw") throw new TypeAccessException();
            else return Type.GetType(TypeName);
        }
    }
}
