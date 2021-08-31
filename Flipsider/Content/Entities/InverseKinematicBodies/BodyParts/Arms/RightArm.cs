
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public class RightArm : Arm
    {
        public override string ID => "R_Arm";

        public override string OtherArm => "L_Arm";

        public override int StaticSide => 1;
    }
}