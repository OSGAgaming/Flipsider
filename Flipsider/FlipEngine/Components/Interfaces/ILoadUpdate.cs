using Microsoft.Xna.Framework;

namespace FlipEngine
{
    public interface ILoadUpdate : IUpdate { }

    public interface ILoadUpdateGT : IUpdateGT { }

    public interface ILoadAlwaysUpdate : IAlwaysUpdate { }

    public interface IAlwaysUpdate : IUpdate { }

    //Non Loaded
    public interface IUpdate
    {
        public void Update();
    }

    public interface IUpdateGT
    {
        public void Update(GameTime gameTime);
    }

}
