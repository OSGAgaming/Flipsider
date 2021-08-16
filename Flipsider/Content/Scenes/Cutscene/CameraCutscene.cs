using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flipsider.Engine
{
    public class CameraCutscene : Cutscene
    {
        public override int Length => 600;

        public override void OnActivate()
        {
            // AddStamp(100, new CameraInterpolater(Main.Camera.Offset, Main.Camera.Offset + new Vector2(50,50)));

            //AddStamp(200, new CameraInterpolater(Main.Camera.Offset + new Vector2(50, 50), Main.Camera.Offset + new Vector2(-50, 50)),
            //              new ZoomInterpolater(1f));
        }
    }
}
