using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Weapons
{
    public class Weapon
    {
        Texture2D inventoryIcon;

        public int damage;
        public int delay;
        protected int activeTimeLeft;

        public int comboState;
        protected readonly int maxCombo;

        public bool active => activeTimeLeft > 0;
        public int activeTime => delay = activeTimeLeft;

        public Weapon(int damage, int delay, int maxCombo)
        {
            this.damage = damage;
            this.delay = delay;
            this.maxCombo = maxCombo;
        }

        protected virtual void OnActivate() { }

        public virtual void UpdateActive() { }

        public virtual bool CanUse() => !active;

        public virtual void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(inventoryIcon, pos, Color.White);
        }

        public virtual void Activate()
        {
            if (!CanUse()) return;

            activeTimeLeft = delay;
            comboState++;

            if (comboState > maxCombo)
                comboState = 0;

            OnActivate();
        }

        public virtual void Update() { }

        public virtual void UpdatePassive()
        {
            Update();

            if (active)
            {
                activeTimeLeft--;
                UpdateActive();
            }
        }
    }
}
