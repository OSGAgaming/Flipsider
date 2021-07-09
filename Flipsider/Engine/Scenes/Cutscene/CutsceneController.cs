using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public abstract class CutsceneController<T,TOut> : ICutsceneControlGeneric<T>, ICutsceneControl
    {
        public T receiver { get; set; }

        public TOut startValue, endValue;

        public CutsceneController(TOut startValue, TOut endValue, T receiver)
        {
            this.startValue = startValue;
            this.endValue = endValue;

            this.receiver = receiver;
        }

        public abstract void Send(float progress);
    }
}
