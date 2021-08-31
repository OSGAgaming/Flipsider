using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;


namespace Flipsider
{
    public class LeftArm : Arm
    {
        public override string ID => "L_Arm";

        public override string OtherArm => "R_Arm";

        public override int StaticSide => -1;
    }
}