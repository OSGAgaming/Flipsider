using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Flipsider.Content.IO.Graphics;

namespace Flipsider.Engine
{
    public class CameraInterpolater : CutsceneController<GameCamera, Vector2>
    {
        public CameraInterpolater(Vector2 startValue, Vector2 endValue) : base(startValue, endValue, Main.Camera) { }

        public override void Send(float progress) => receiver.Offset = Vector2.Lerp(startValue, endValue, progress);
    }

    public class ZoomInterpolater : CutsceneController<GameCamera, float>
    {
        public ZoomInterpolater(float Value) : base(Value, Value, Main.Camera) { }

        public override void Send(float progress) => receiver.targetScale = startValue;
    }
}
