
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public class Camera
    {
       public Matrix Transform { get; set; }
       public float scale { get; set; }
       public float rotation { get; set; }

        public void FixateOnPlayer(Player player)
        {
            Transform = 
                 Matrix.CreateTranslation(new Vector3(-player.position.X - player.width, -player.position.Y - player.height, 0))*
                 Matrix.CreateScale(scale) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Main.screenSize.X/2,Main.screenSize.Y/2,0);
        }

    }
}
