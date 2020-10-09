using Flipsider.Core;
using Flipsider.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Flipsider.Tiles
{
    public sealed class TileSystem
    {
        internal TileSystem(World world)
        {
            World = world;
        }

        public IEnumerable<Chunk> ChunksEnumerable => chunks.Values;

        public World World { get; }

        private readonly Dictionary<Point, Chunk> chunks = new Dictionary<Point, Chunk>();

        /// <summary>
        /// Returns <see cref="GetChunk(int, int)"/> and loads the chunk if it isn't present.
        /// </summary>
        public Chunk GetChunkOrLoad(int x, int y) => GetChunkOrLoad(new Point(x, y));
        /// <summary>
        /// Returns <see cref="GetChunk(Point)"/> and loads the chunk if it isn't present.
        /// </summary>
        public Chunk GetChunkOrLoad(Point p)
        {
            chunks.TryGetValue(p, out var ret);
            ret ??= Chunk.Load(p);
            return ret;
        }

        /// <summary>
        /// Gets the chunk at the specified chunk coordinates.
        /// </summary>
        /// <returns>The chunk instance, or null for an unloaded chunk.</returns>
        public Chunk? GetChunk(int x, int y) => GetChunk(new Point(x, y));
        /// <summary>
        /// Gets the chunk at the specified chunk coordinates.
        /// </summary>
        /// <returns>The chunk instance, or null for an unloaded chunk.</returns>
        public Chunk? GetChunk(Point p) => chunks.GetValueOrDefault(p);

        internal void UnloadChunk(Point p) => chunks.Remove(p);

        /// <summary>
        /// Gets the tile from the specified tile coordinates.
        /// </summary>
        /// <returns>The tile instance.</returns>
        public ref Tile GetTile(int x, int y)
        {
            Chunk c = GetChunkOrLoad(x / Chunk.Width, y / Chunk.Height);
            x = Mod(x, Chunk.Width);
            y = Mod(y, Chunk.Height);
            if (x < 0)
                x -= c.Pos.X * Chunk.Width;
            if (y < 0)
                y -= c.Pos.Y * Chunk.Height;
            return ref c[x, y];

            static int Mod(int value, int length)
            {
                int r = value % length;
                return r < 0 ? r + length : r;
            }
        }
        /// <summary>
        /// Gets the tile from the specified tile coordinates.
        /// </summary>
        /// <returns>The tile instance.</returns>
        public ref Tile GetTile(Point p) => ref GetTile(p.X, p.Y);

        /// <summary>
        /// Shorthhand for <see cref="GetTile(int, int)"/>.
        /// </summary>
        public ref Tile this[int x, int y] => ref GetTile(x, y);
        /// <summary>
        /// Shorthand for <see cref="GetTile(Point)"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ref Tile this[Point p] => ref GetTile(p.X, p.Y);

        // ---- STATIC ENTITY MANAGEMENT ----

        private readonly HashSet<StaticEntity> staticEntities = new HashSet<StaticEntity>();
        private readonly Dictionary<Entity, StaticEntity> entityLinks = new Dictionary<Entity, StaticEntity>();

        /// <summary>
        /// Spawn an entity that was loaded from a chunk, if it has not been loaded already.
        /// </summary>
        /// <param name="chunk">The chunk to test for.</param>
        /// <param name="staticId">The unique, per-chunk ID of the entity.</param>
        /// <param name="e">The entity that was loaded.</param>
        public void SpawnStaticEntity(Chunk chunk, int staticId, Entity e)
        {
            StaticEntity identity = new StaticEntity { chunk = chunk.Pos, id = staticId };
            if (staticEntities.Add(identity))
            {
                entityLinks[e] = identity;
                e.OnRemove += e => E_OnRemove(e.Entity);
                e.SpawnInWorld();
            }
        }

        private void E_OnRemove(Entity e)
        {
            staticEntities.Remove(entityLinks[e]);
            entityLinks.Remove(e);
        }

        // Used for tracking entities loaded from chunks.
        private struct StaticEntity
        {
            public int id;
            public Point chunk;

            public override int GetHashCode()
            {
                return HashCode.Combine(id, chunk);
            }
        }
    }
}
