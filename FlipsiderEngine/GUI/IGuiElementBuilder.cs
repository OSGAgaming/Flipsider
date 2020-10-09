namespace Flipsider.GUI
{
    /// <summary>
    /// Represents a builder for use in <see cref="GuiBuilder.Enter{TBuilder}(TBuilder)"/>.
    /// </summary>
    /// <typeparam name="TSelf">This type of builder.</typeparam>
    public interface IGuiElementBuilder<TSelf>
    {
        TSelf Enter(GuiBuilder builder, IGuiState state);
        GuiBuilder End();
    }
}
