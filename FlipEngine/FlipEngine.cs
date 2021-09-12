using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FlipEngine
{
    public static class FlipE
    {
        public static Random rand;
        public static GameTime gameTime;
        public static SpriteFont font;

        public static void Load(ContentManager Content)
        {
            rand = new Random();
            font = Content.Load<SpriteFont>("FlipFont");
        }

        public static void Update(GameTime gameTime)
        {
            FlipE.gameTime = gameTime;
        }
    }
}
