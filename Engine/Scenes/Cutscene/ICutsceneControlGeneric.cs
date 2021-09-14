using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public interface ICutsceneControlGeneric<T>
    {
        T receiver { get; }
    }
}
