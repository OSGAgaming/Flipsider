
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
  public class EnergyRocc : PropEntity
  {
      public override string Prop => "EnergyRocc";

      public override bool Draw(SpriteBatch spriteBatch, Prop prop)
      {
          Main.lighting.Maps.DrawToMap("BloomMap", (SpriteBatch sb) => { sb.Draw(TextureCache.EnergyRoccGlow, prop.Center, Texture.Bounds, Color.White*Time.SineTime(2f), 0f, Texture.TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
          spriteBatch.Draw(Texture, prop.Center,Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f);
          return false;
      }
  }
}
