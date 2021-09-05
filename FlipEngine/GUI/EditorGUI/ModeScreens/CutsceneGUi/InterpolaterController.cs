


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class InterpolaterController : PreviewElement
    {
        public int Lerp;
        protected CutsceneNode c;

        public InterpolaterController(ScrollPanel p, CutsceneNode c) : base(p)
        {
            this.c = c;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(c.RelativeDimensions.X + Lerp, c.RelativeDimensions.Y, 8, 8);
            Utils.DrawRectangle(RelativeDimensions, Color.White, 1);

            InterpolaterController? controller = CutsceneNode.controller;

            if (controller != null)
            {
                if (controller.Equals(this))
                {
                    Utils.DrawRectangle(RelativeDimensions.Inf(1,1), Color.Yellow * Time.SineTime(6), 1);

                    if(Keyboard.GetState().IsKeyDown(Keys.Delete))
                    {
                        c.Nodes.Remove(this);
                    }
                }
            }
        }

        protected override void OnLeftClick()
        {
            CutsceneNode.controller = this;
        }
        protected override void OnHover()
        {
            CutsceneNode.IsHoveringOverThingy = true;
        }
        public virtual void DrawToActive() { }
    }
}


