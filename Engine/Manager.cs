﻿using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace Flipsider
{
    public class Manager<T> : IComponent where T : IComponent
    {
        public Manager(bool ingame = true)
        {
            if (ingame)
                Main.Updateables.Add(this);
            else
            {
                Main.UpdateablesOffScreen.Add(this);
            }
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
