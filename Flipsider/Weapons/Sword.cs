using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons
{
    abstract class Sword : Weapon
    {

        protected int state;
        public int useTime;
        public abstract Texture2D swordSheet
        {
            get;
            set;
        }
        public Sword(int damage, int delay, int maxCombo) : base(damage, delay, maxCombo)
        {
            
        }

        public sealed override void Activate(Player player)
        {
            state++;
            if(state > 3)
            {
                state = 0;
            }
            OnActivate();
        }
    }
}
