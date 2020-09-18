using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine
{
    public class Scene
    {
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
        public virtual void Draw()
        {

        }
    }
}
