using Flipsider.Core;
using Flipsider.Extensions;
using Flipsider.Entities;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flipsider.Collision
{
    public sealed class CollisionSystem : IUpdated
    {
        private readonly HashSet<ICollideable> collideables = new HashSet<ICollideable>();
        private readonly HashSet<ICollisionObserver> observers = new HashSet<ICollisionObserver>();

        // Used to cache collideables that will be added/removed (true/false) next update.
        private readonly Dictionary<ICollideable, bool> additions = new Dictionary<ICollideable, bool>();
        private readonly Dictionary<ICollisionObserver, bool> additionsObservers = new Dictionary<ICollisionObserver, bool>();

        internal CollisionSystem()
        {
        }

        /// <summary>
        /// Registers a collideable and begins checking it for collisions.
        /// </summary>
        public void AddCollideable(ICollideable collideable) => additions[collideable] = true;
        /// <summary>
        /// Registers a collision observer and begins looking for collisions.
        /// </summary>
        public void AddObserver(ICollisionObserver collideable) => additionsObservers[collideable] = true;

        /// <summary>
        /// Removes a collideable and stops checking it for collisions.
        /// </summary>
        public void RemoveCollideable(ICollideable collideable) => additions[collideable] = false;
        /// <summary>
        /// Removes a collision observer and stops looking for collisions.
        /// </summary>
        /// <param name="collideable"></param>
        public void RemoveObserver(ICollisionObserver collideable) => additionsObservers[collideable] = false;

        void IUpdated.Update()
        {
            RefreshCollections();

            // Do le collisions.
            // TODO: use grid-based or quadtree optimizations
            foreach (var one in observers)
            {
                foreach (var two in collideables)
                {
                    if (ReferenceEquals(one, two))
                        continue;

                    try
                    {
                        if (one.Bounds.Intersects(two.Bounds))
                            one.Intersect(two);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn($"Intersection of collidedables {one} and {two} threw an exception. {e}");
                    }
                }
            }

            RefreshCollections();
        }

        private void RefreshCollections()
        {
            // Update new/old
            foreach (var item in additions)
            {
                if (item.Value)
                    collideables.Add(item.Key);
                else
                    collideables.Remove(item.Key);
            }
            foreach (var item in additionsObservers)
            {
                if (item.Value)
                    observers.Add(item.Key);
                else
                    observers.Remove(item.Key);
            }

            additions.Clear();
            additionsObservers.Clear();
        }
    }
}
