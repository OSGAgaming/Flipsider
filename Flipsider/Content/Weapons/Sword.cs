﻿using Microsoft.Xna.Framework.Graphics;

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
        protected override void OnActivate()
        {
            OnActivation();
            state++;
            if (state > 3)
            {
                state = 0;
            }
        }
        protected virtual void OnActivation() { }
    }

}
