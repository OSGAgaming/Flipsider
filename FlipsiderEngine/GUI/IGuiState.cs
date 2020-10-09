using Flipsider.Core;

namespace Flipsider.GUI
{
    /// <summary>
    /// Represents a set of UI elements. Use <see cref="GuiBuilder"/> or derive from this class to create instances of it.
    /// </summary>
    public interface IGuiState : IUpdated, IDrawn
    {
        /// <summary>
        /// Appends an element to this GUI state.
        /// </summary>
        void AddElement(GuiElement element);
    }
}
