using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;

namespace Flipsider.Engine
{
    public class FloatInterpolater<T> : CutsceneController<T, float>
    {
        public FloatInterpolater(float startValue, float endValue) : base(startValue, endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;
        }

        public override T receiver => throw new System.NotImplementedException();
    }

    public class Vector2Interpolater<T> : CutsceneController<T, Vector2>
    {
        public Vector2Interpolater(Vector2 startValue, Vector2 endValue) : base(startValue, endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;
        }

        public override T receiver => throw new System.NotImplementedException();
    }
}
