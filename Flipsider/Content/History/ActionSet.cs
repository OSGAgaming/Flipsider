using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Flipsider.Engine;
using System.Collections.Generic;
using Flipsider.GUI.TilePlacementGUI;
using MonoGame.Extended.BitmapFonts;

namespace Flipsider
{
    public class ActionSet
    {
        internal int Order { get; }

        internal List<Entity> EntityList = new List<Entity>();

        //TODO Add more Misc Things
        public void Clear() => EntityList.Clear();

        public void Undo()
        {
            foreach(Entity entity in EntityList)
                entity.Dispose();
        }

        public void Redo()
        {
            foreach (Entity entity in EntityList)
            {
                if (entity is Tile tile)
                {
                    tile.Active = true;
                    Vector2 pos = new Vector2(tile.i, tile.j);
                    Tile tileI = new Tile(tile.type, tile.frame, pos);
                    Main.tileManager.AddTile(Main.World, tileI);
                }
                if (entity is Prop prop)
                {
                    Main.World.propManager.AddProp(prop.prop, prop.Center);
                }
            }

            ActionCache.Instance.CanAddToCache = false;
        }
    }
}
