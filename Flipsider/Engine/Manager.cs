using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.GUI.HUD;
using Flipsider.Scenes;
using Flipsider.Engine.Particles;
using Flipsider.Engine;
using Flipsider.Engine.Audio;
using Flipsider.Engine.Input;
using Flipsider.GUI.TilePlacementGUI;
using static Flipsider.TileManager;
using System.Reflection;
using System.Linq;
using System.Threading;
using Flipsider.Engine.Interfaces;

#nullable disable
// TODO fix this..
namespace Flipsider
{
    public class Manager<T> : IComponent where T : IComponent
    {
        public Manager()
        {
            Main.Updateables.Add(this);
        }
        internal List<T> Components = new List<T>();
        public virtual void Update()
        {
            foreach (T foo in Components)
            {
                if (foo != null)
                    foo.Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (T foo in Components)
            {
                if (foo != null)
                    foo.Draw(spriteBatch);
            }
        }

        public void AddComponent(T Component)
        => Components.Add(Component);

        public void RemoveComponent(int index)
        => Components.RemoveAt(index);
        public void RemoveComponent(T instance)
        => Components.Remove(instance);
    }
}
