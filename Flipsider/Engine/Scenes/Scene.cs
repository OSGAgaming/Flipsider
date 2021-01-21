using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public class Scene
    {
        /// <summary>
        /// Name of the scene for easy Accessibility
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// Method called when the scene has just been activated.
        /// </summary>
        public virtual void OnActivate()
        {
        }

        /// <summary>
        /// Method called when the scene has just been deactivated and is no longer the active scene.
        /// </summary>
        public virtual void OnDeactivate()
        {
        }

        /// <summary>
        /// Method called to update the scene.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Method called to draw the scene.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
