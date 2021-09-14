using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Flipsider.Content.IO.Graphics;
using System.IO;

namespace Flipsider.Engine
{
    public class CameraInterpolater : CutsceneController<GameCamera, Vector2>
    {
        public override GameCamera receiver => Main.Camera;

        public CameraInterpolater(Vector2 startValue, Vector2 endValue) : base(startValue, endValue) { }

        public CameraInterpolater() : base(Vector2.Zero, Vector2.Zero) { }

        public override void Send(float progress) => receiver.Offset = Vector2.Lerp(startValue - receiver.Position, endValue - receiver.Position, progress);

        public override void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(startValue);
            writer.Write(endValue);
        }
        public override ICutsceneControl Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            return new CameraInterpolater(reader.ReadVector2(), reader.ReadVector2());
        }
    }

    public class ZoomInterpolater : CutsceneController<GameCamera, float>
    {
        public override GameCamera receiver => Main.Camera;

        public ZoomInterpolater(float Value) : base(Value, Value) { }

        public ZoomInterpolater() : base(0, 0) { }

        public override void Send(float progress) => receiver.targetScale = startValue;
    }
}
