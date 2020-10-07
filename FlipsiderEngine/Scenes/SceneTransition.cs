using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flipsider.Assets;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider
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
        public virtual void Draw(SafeSpriteBatch spriteBatch, float transitionProgress)
        {

        }
    }
}
