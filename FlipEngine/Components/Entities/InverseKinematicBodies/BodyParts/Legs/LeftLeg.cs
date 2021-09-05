using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlipEngine
{
    public class LeftLeg : Leg
    {
        public override string ID => "L_Leg";

        public override string OtherLeg => "R_Leg";
    }
}