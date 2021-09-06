
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public interface ICutsceneControlGeneric<T>
    {
        T receiver { get; }
    }
}
