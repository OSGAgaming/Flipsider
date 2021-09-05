
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public interface ICutsceneControl : ISerializable<ICutsceneControl>
    {
        public void Send(float progress);
    }
}
