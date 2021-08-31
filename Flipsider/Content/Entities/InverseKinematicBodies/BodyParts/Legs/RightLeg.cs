using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public class RightLeg : Leg
    {
        public override string ID => "R_Leg";

        public override string OtherLeg => "L_Leg";
    }
}