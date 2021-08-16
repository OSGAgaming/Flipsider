using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public interface ICutsceneControl : ISerializable<ICutsceneControl>
    {
        public void Send(float progress);
    }
}
