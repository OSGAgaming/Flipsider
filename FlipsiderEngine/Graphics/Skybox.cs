using Flipsider.Assets;
using Flipsider.Core;
using System.Text;

namespace Flipsider.Graphics
{
    public abstract class Skybox : IUpdated, IDrawn
    {
        public abstract void Update();

        public abstract void Draw(SafeSpriteBatch spriteBatch);
    }
}
