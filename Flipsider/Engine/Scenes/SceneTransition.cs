
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public class SceneTransition
    {
        /// <summary>
        /// The length, in seconds, of this transition.
        /// </summary>
        public virtual float Length { get; }

        /// <summary>
        /// The time interval at which the transition is fully covering the screen.
        /// </summary>
        public virtual float SwitchPoint { get; }

        /// <summary>
        /// Draw the transition.
        /// </summary>
        /// <param name="transitionProgress">Value between 0 and 1 that represents the progress of the transition.</param>
        public virtual void Draw(SpriteBatch spriteBatch, float transitionProgress) { }

        /// <summary>
        /// Draw the transition in the UI Layer
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="transitionProgress">Value between 0 and 1 that represents the progress of the transition.</param>
        public virtual void DrawUI(SpriteBatch spriteBatch, float transitionProgress) { }

    }
}
