using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons.Ranged.Pistol
{
    class TestGun : RangedWeapon
    {
        public TestGun() : base(5, 30, 3) { }

        protected override void OnActivate()
        {
            Camera.screenShake += 10;
        }
    }
}
