using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons
{
    internal abstract class Sword : Weapon
    {

        protected int state;
        public abstract Texture2D swordSheet
        {
            get;
        }
        public Sword(int damage, int delay, int maxCombo) : base(damage, delay, maxCombo)
        {

        }
        public sealed override void UpdateActive()
        {
        }
        protected override void OnActivate()
        {
            state++;
            if (state > 3)
            {
                state = 0;
            }
        }
    }

}
