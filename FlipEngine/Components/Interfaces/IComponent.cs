namespace FlipEngine
{
    public interface IComponent : IUpdate, IDrawable { }

    public interface ILoadComponent : ILoadUpdate, IDrawable { }
}
