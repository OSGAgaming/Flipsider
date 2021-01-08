using System;
using System.Collections.Generic;
using System.Text;

using Flipsider.Engine;
using Flipsider.Engine.Interfaces;

namespace Flipsider.Scenes
{
    public class ForestArea : Scene
    {
        public ForestArea()
        {

        }

        public override void Update()
        {
            foreach (IUpdate updateable in Main.UpdateablesOffScreen.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
            foreach (IUpdate updateable in Main.Updateables.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
