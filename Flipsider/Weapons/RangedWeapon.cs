﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons
{
    class RangedWeapon : Weapon
    {
        public int ammo;
        private int reload;
        public int reloadTime;

        public bool reloading => reload > 0;

        public RangedWeapon(int damage, int delay, int maxCombo) : base(damage, delay, maxCombo) { }

        public sealed override void Activate()
        {
            if (!CanUse() || reloading) return;

            activeTimeLeft = delay;
            comboState++;
            ammo--;

            if (comboState > maxCombo)
                comboState = 0;

            if (ammo <= 0)
                reload = reloadTime;

            OnActivate();
        }

        public sealed override void UpdatePassive()
        {
            Update();

            if (active)
            {
                activeTimeLeft--;
                UpdateActive();
            }

            if (reloading)
                reload--;
        }
    }
}