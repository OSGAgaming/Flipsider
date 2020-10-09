using Flipsider.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Flipsider.GUI
{
    public class GuiBuilder
    {
        protected IGuiState state = new GuiState();

        /// <summary>
        /// Enter a GUI element builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type of GUI builder.</typeparam>
        /// <param name="instance">An instance to work with.</param>
        /// <returns>The builder.</returns>
        public TBuilder Enter<TBuilder>(TBuilder instance) where TBuilder : IGuiElementBuilder<TBuilder>
        {
            return instance.Enter(this, state);
        }

        /// <summary>
        /// Gets the current GUI state.
        /// </summary>
        public virtual IGuiState Build() => state;
    }

    /// <summary>
    /// A general-purpose UI element.
    /// </summary>
    public class GuiElement
    {
        public GuiElement(GuiBounds bounds)
        {
            this.bounds = bounds;
        }

        /// <summary>
        /// What GUI elements parent this one.
        /// </summary>
        public IEnumerable<GuiElement> GetChildren() => children.ToList();

        protected internal readonly LinkedList<GuiElement> children = new LinkedList<GuiElement>();

        /// <summary>
        /// What GUI element this one is relative to.
        /// </summary>
        public GuiElement? Parent { get; private set; }

        /// <summary>
        /// The GUI bounds of this element, independent of others.
        /// </summary>
        public GuiBounds Bounds => bounds;

        private bool recursive;
        private GuiBounds bounds;

        public void AppendLast(GuiElement child)
        {
            child.Parent = this;
            children.Append(child);
        }

        public void RemoveFirst(GuiElement child)
        {
            child.Parent = null;
            children.Remove(child);
        }

        protected void UpdateBounds(GuiBounds value)
        {
            if (Parent is object && !recursive)
            {
                recursive = true;
                bounds = value;
                foreach (var item in children)
                {
                    item.bounds = new GuiBounds(bounds.PositionScreen + item.bounds.PixelOffset, item.bounds.PercentOffset, item.bounds.Width, item.bounds.Height);
                }
                recursive = false;
            }
            bounds = value;
        }

        public virtual void Update(IGuiState state)
        {

        }

        public virtual void Draw(IGuiState state, SafeSpriteBatch spriteBatch)
        {

        }
    }
}
