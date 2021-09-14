using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider
{
    public class FlipsiderWorld : World
    {
        public Player? MainPlayer;

        public Player ReplacePlayer(Player player)
        {
            if (player != null)
            {
                MainPlayer?.Dispose();
                MainPlayer = player;
                return player;
            }
            return MainPlayer ?? new Player();
        }
        public bool AppendPlayer(Player player)
        {
            if (player != null)
            {
                MainPlayer = player;
                return true;
            }
            return false;
        }
        public FlipsiderWorld(int Width, int Height) : base(Width, Height)
        {

        }
    }
}
