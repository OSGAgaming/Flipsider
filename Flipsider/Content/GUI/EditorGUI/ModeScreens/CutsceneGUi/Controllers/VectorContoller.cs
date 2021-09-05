using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Flipsider.TileManager;

namespace Flipsider.GUI
{
    internal class VectorController : InterpolaterController
    {
        public Vector2 CameraStamp;

        public VectorController(ScrollPanel p, CutsceneNode c) : base(p, c) { }

        public override void DrawToActive()
        {
            Utils.DrawTextToLeft("Time Stamp: " + Lerp * (c.CutseneParent.LengthOfCutscene / (float)c.CutseneParent.GraphDimensions.X), Color.White, new Point(5, 15).ToVector2(), 0, 0.5f);
            Utils.DrawTextToLeft("Camera Position: " + CameraStamp, Color.White, new Point(5, 25).ToVector2(), 0, 0.5f);
        }
    }

    internal class VectorCutsceneNode : CutsceneNode
    {
        public VectorCutsceneNode(ScrollPanel p, CutsceneElement c) : base(p, c) { }

        public override Cutscene Serialize(Cutscene scene)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                VectorController controller = (VectorController)Nodes[i];
                if (i == 0)
                {
                    scene.AddStamp(controller.Lerp, new CameraInterpolater(Main.Camera.Position, controller.CameraStamp));
                }
                else
                {
                    VectorController prevController = (VectorController)Nodes[i - 1];
                    scene.AddStamp(controller.Lerp, new CameraInterpolater(prevController.CameraStamp, controller.CameraStamp));
                }
            }

            return scene;
        }

        public override void Deserialize(Cutscene scene)
        {
            foreach (TimeStamp stamp in scene.TimeStamps)
            {
                if (stamp.Info != null)
                {
                    foreach (ICutsceneControl icc in stamp.Info)
                    {
                        if (icc is CameraInterpolater ci)
                        {
                            if (PreviewPanel != null)
                            {
                                VectorController IC = new VectorController(PreviewPanel, this);
                                IC.Lerp = stamp.Time;
                                IC.CameraStamp = ci.endValue;

                                Nodes.Add(IC);
                            }
                        }
                    }
                }
            }
        }

        public override void DrawDialogueBox(SpriteBatch sb)
        {
            if (PreviewPanel != null)
            {
                Point p = Mouse.GetState().Position.Sub(new Point(PreviewPanel.dimensions.X - 10, DialogueBoxDimensions.Y + PreviewPanel.dimensions.Y + 20));
                Utils.DrawBoxFill(new Rectangle(p, DialogueBoxDimensions), Color.Gray, 0.1f);

                Utils.DrawTextToLeft("Camera Pos: " + Vector2.Zero, Color.Black, p.Add(new Point(5, 5)).ToVector2(), 0, 0.5f);
                Utils.DrawTextToLeft("Time Stamp: " + Lerp, Color.Black, p.Add(new Point(5, 15)).ToVector2(), 0, 0.5f);

                Utils.DrawLine(sb, Mouse.GetState().Position.Sub(new Point(PreviewPanel.dimensions.X, PreviewPanel.dimensions.Y)).ToVector2(), p.Add(new Point(2, DialogueBoxDimensions.Y)).ToVector2(), Color.White, 1);

                if (GameInput.Instance["LC"].IsJustPressed() && !IsHoveringOverThingy)
                {
                    if (PreviewPanel != null)
                    {
                        int GraphLerp = (int)(Mouse.GetState().Position.X - (PreviewPanel.dimensions.X + CutseneParent.Position.X));

                        InterpolaterController IC = new VectorController(PreviewPanel, this);
                        IC.Lerp = GraphLerp;

                        Nodes.Add(IC);
                    }
                }
            }
        }
    }
}


