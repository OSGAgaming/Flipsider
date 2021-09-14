using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider.Engine
{
    public abstract class CutsceneController<T,TOut> : ICutsceneControlGeneric<T>, ICutsceneControl
    {
        public abstract T receiver { get; }

        public TOut startValue, endValue;

        public CutsceneController(TOut startValue, TOut endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;
        }

        public virtual void Send(float progress) { }

        public virtual void Serialize(Stream stream) { }

        public virtual ICutsceneControl Deserialize(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
