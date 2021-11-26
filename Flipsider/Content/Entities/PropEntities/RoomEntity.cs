
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;

using System.IO;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.Engine.Maths;

namespace Flipsider
{
    public class RoomEntity : PropEntity
    {
        public override string Prop => "Room";

        public Vector2 Size => Texture.Bounds.Size.ToVector2();
        public int Girth { get; set; } = 32;
        public override void Update(Prop prop)
        {
            if (Main.player.CollisionFrame.Intersects(new Rectangle(prop.Position.ToPoint(), Size.ToPoint())))
            {
                CutawayMap.Outdoors = false;
            }
            Main.lighting.Maps.DrawToMap("CutawayMap", (SpriteBatch sb) =>
            {
                sb.Draw(Textures._GUI_MagicPixel, prop.Position, new Rectangle(0, 0, (int)Size.X, (int)Size.Y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            });

            prop.Width = (int)Size.X;
            prop.Height = (int)Size.Y;

            Center = prop.Center;
        }

        public override void PostLoad(FlipEngine.Prop prop)
        {
            Chunk chunk = Main.tileManager.GetChunkToWorldCoords(prop.Position);

            Active = true;
            Center = prop.Center;
        }
        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            spriteBatch.Draw(Texture, prop.Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

