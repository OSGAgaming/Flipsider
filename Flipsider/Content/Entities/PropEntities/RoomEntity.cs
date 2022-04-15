
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
        public override string Prop => "City_Apartment_Room";
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures._Props_City_Apartment_RoomFMap, Position, Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f);

            Main.lighting.Maps.DrawToMap("PlayerObscureMap", (SpriteBatch sb) =>
            {
                sb.Draw(Textures._Props_City_Apartment_RoomFMap, Position, Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f);
            });
        }

        public override void Update(Prop prop)
        {
            if (Main.player.CollisionFrame.Intersects(prop.CollisionFrame))
            {
                CutawayMap.Outdoors = false;
            }
            Main.lighting.Maps.DrawToMap("CutawayMap", (SpriteBatch sb) =>
            {
                sb.Draw(Textures._GUI_MagicPixel, prop.Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            });

            Center = prop.Center;
        }

        public override void PostLoad(FlipEngine.Prop prop)
        {
            Layer = Main.player.Layer + 1;

            Active = true;
            Center = prop.Center;

            Main.AppendToLayer(this);
        }
        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            spriteBatch.Draw(Texture, prop.Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

