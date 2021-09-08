using Microsoft.Xna.Framework;

namespace FlipEngine
{
    public interface IUpdate
    {
        public void Update();
    }

    public interface IUpdateGT
    {
        public void Update(GameTime gameTime);
    }
}
