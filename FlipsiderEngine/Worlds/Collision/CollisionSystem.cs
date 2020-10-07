using Flipsider.Core;
using Flipsider.Extensions;
using Flipsider.Worlds.Entities;
using Flipsider.Worlds.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flipsider.Worlds.Collision
{
    public delegate void OnCheckCollideableDelegate(ICollideable collideable);
    public sealed class CollisionSystem : IUpdated
    {
        // Many-to-Many relationship. Pretty complex to maintain. But doable! :)
        private readonly HashSet<ICollideable> collideables = new HashSet<ICollideable>();

        // Used to cache collideables that will be added/removed (true/false) next update.
        private readonly Dictionary<ICollideable, bool> additions = new Dictionary<ICollideable, bool>();

        internal CollisionSystem()
        {
            OnCheck += TileCollision;
        }

        private void TileCollision(ICollideable collideable)
        {
            if (collideable is ITileCollideable obj)
            {
                // TODO reimplement tile collision
            }
        }

        /// <summary>
        /// Fired when a collideable is iterated (one time for each collideable per collision update).
        /// </summary>
        public event OnCheckCollideableDelegate? OnCheck;

        /// <summary>
        /// Registers a collideable and begins checking it for collisions.
        /// </summary>
        public void Add(ICollideable collideable)
        {
            if (collideables.Contains(collideable))
            {
                throw new InvalidOperationException("Collideable already added.");
            }
            additions[collideable] = true;
        }

        /// <summary>
        /// Removes a collideable and stops checking it for collisions.
        /// </summary>
        /// <param name="collideable"></param>
        public void Remove(ICollideable collideable)
        {
            if (!collideables.Contains(collideable))
            {
                throw new InvalidOperationException("Collideable is not present.");
            }
            additions[collideable] = false;
        }

        void IUpdated.Update()
        {
            // Update new/old
            foreach (var item in additions)
            {
                if (item.Value) 
                    collideables.Add(item.Key);
                else 
                    collideables.Remove(item.Key);
            }

            additions.Clear();

            // Do le collisions.
            // TODO: use grid-based or quadtree optimizations
            foreach (var one in collideables)
            {
                foreach (var two in collideables)
                {
                    if (ReferenceEquals(one, two))
                        continue;

                    try
                    {
                        one.Intersect(two);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn($"Intersection of collidedables {one} and {two} threw an exception. {e}");
                    }
                }
                if (OnCheck != null)
                    foreach (OnCheckCollideableDelegate del in OnCheck.GetInvocationList())
                    {
                        try
                        {
                            del(one);
                        }
                        catch (Exception e)
                        {
                            Logger.Warn(del.Method + " in the CollisionSystem.OnCheck event threw an exception. " + e);
                            OnCheck -= del;
                        }
                    }
            }
        }
    }
}
