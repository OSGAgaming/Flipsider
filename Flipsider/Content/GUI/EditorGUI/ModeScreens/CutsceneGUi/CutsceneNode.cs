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
    internal class CutsceneNode : PreviewElement
    {
        public string? sID;
        public int nID;
        public bool isHovering;
        public CutsceneElement CutseneParent;
        protected int Lerp;
        public List<InterpolaterController> Nodes;

        public static InterpolaterController? controller;
        public static bool IsHoveringOverThingy;

        public virtual Cutscene Serialize(Cutscene scene) 
        {
            return new Cutscene();
        }

        public virtual void Deserialize(Cutscene scene) { }


        public Point DialogueBoxDimensions = new Point(200, 50);
        public CutsceneNode(ScrollPanel p, CutsceneElement c) : base(p)
        {
            CutseneParent = c;
            Nodes = new List<InterpolaterController>();

            PreviewPanel = p;
        }

        public virtual void DrawDialogueBox(SpriteBatch sb) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle((int)CutseneParent.Position.X, (int)CutseneParent.Position.Y - nID * 10 - 4, CutseneParent.GraphDimensions.X, 8);

            if (PreviewPanel != null) Lerp =
                    (int)((Mouse.GetState().Position.X - (PreviewPanel.dimensions.X + CutseneParent.Position.X)) / CutseneParent.GraphDimensions.X * CutseneParent.LengthOfCutscene);

            Utils.DrawLine(spriteBatch,
                new Vector2((int)CutseneParent.Position.X, (int)CutseneParent.Position.Y - nID * 10),
                new Vector2((int)CutseneParent.Position.X + CutseneParent.GraphDimensions.X, (int)CutseneParent.Position.Y - nID * 10),
                Color.White);

            foreach (InterpolaterController i in Nodes.ToArray())
            {
                i.Draw(spriteBatch);
                i.Update();
            }

            if (isHovering) DrawDialogueBox(spriteBatch);
            isHovering = false;


            if (GameInput.Instance["LC"].IsJustPressed())
            {
                if (controller != null && controller is VectorController c && !IsHoveringOverThingy)
                {
                    c.CameraStamp = Main.MouseToDestination().ToVector2();
                }
            }

            IsHoveringOverThingy = false;
        }

        protected override void OnHover()
        {
            isHovering = true;
        }
    }
}


