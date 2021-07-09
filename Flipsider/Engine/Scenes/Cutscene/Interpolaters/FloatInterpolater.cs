using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Flipsider.Engine
{
    public class FloatInterpolater : CutsceneController<CameraTransform, Vector2>
    {
        public FloatInterpolater(Vector2 startValue, Vector2 endValue) : base(startValue, endValue, Main.Camera)
        {
            this.startValue = startValue;
            this.endValue = endValue;
        }

        public override void Send(float progress) => receiver.Position = Vector2.Lerp(startValue, endValue, progress);
    }
}
