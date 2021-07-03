using Flipsider.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class ScrollPanel : UIElement
    {
        public virtual Point v { get; }

        public virtual Point PreivewDimensions { get; }

        public virtual Color Color { get; }

        RenderTarget2D PreviewTarget { get; set; }

        public float ScrollValue;
        public int ScrollRoom;
        public bool Active;
        private float BarAlpha;

        public float PreviewAlpha;
        public int PreviewHeight;
        private float ScrollVelocity;

        private float ScrollSpeed = 3;
        private int BarWidth = 20;
        private int Padding = 10;
        public ScrollPanel()
        {
            PreviewTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2000, 2000);
        }
        protected virtual void CustomDrawDirect(SpriteBatch sb) { }
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            if (Main.CurrentScene.Name != "Main Menu")
            {
                Utils.DrawBoxFill(dimensions, Color);
                PreviewAlpha = PreviewAlpha.ReciprocateTo(1);

                dimensions = new Rectangle(v, PreivewDimensions);

                Main.renderer.AddTargetCall(PreviewTarget, CustomDrawDirect);
                spriteBatch.Draw(PreviewTarget, new Rectangle(v, PreivewDimensions), new Rectangle(new Point(0, (int)ScrollValue), PreivewDimensions), Color.White * PreviewAlpha);

                PreviewAlpha = PreviewAlpha.ReciprocateTo(0);

                if (Active)
                {
                    int Height = PreviewHeight;

                    if (Height != 0)
                        ScrollRoom = Math.Max(Height - PreivewDimensions.Y, 0);
                    else ScrollRoom = 0;

                    int Room = (int)(Math.Max(Height - PreivewDimensions.Y, 0) / (float)Height * PreivewDimensions.Y);

                    Utils.DrawRectangle(dimensions, Color.White * Time.SineTime(4f) * PreviewAlpha);

                    Utils.DrawRectangle(new Rectangle(
                        dimensions.Right - BarWidth - Padding,
                        dimensions.Y + Padding + (int)(ScrollValue * (PreivewDimensions.Y / (float)Height)),
                        BarWidth,
                        dimensions.Height - Padding * 2 - Room),
                        Color.White * BarAlpha);
                }
            }
        }
        protected override void OnLeftClick()
        {
            Active = true;
        }
        protected override void OnHover()
        {
            if (Active)
            {
                BarAlpha += (1 - BarAlpha) / 16f;

                if (GameInput.Instance["EditorZoomIn"].IsDown())
                {
                    ScrollVelocity -= ScrollSpeed;
                }
                if (GameInput.Instance["EditorZoomOut"].IsDown())
                {
                    ScrollVelocity += ScrollSpeed;
                }

                ScrollVelocity *= 0.8f;
                ScrollValue += ScrollVelocity;

                ScrollValue = Math.Clamp(ScrollValue, 0, ScrollRoom);
            }

            EditorModeGUI.CanZoom = false;
        }

        protected override void OnLeftClickAway()
        {
            Active = false;
        }

        protected override void NotOnHover()
        {
            BarAlpha += (0 - BarAlpha) / 16f;
        }
    }
}


