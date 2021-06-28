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
    internal class BottomModeSelectPreview : UIElement
    {
        Point v => new Point(Main.renderer.Destination.Left, Main.renderer.Destination.Bottom);

        Point PreivewDimensions => new Point(Main.renderer.Destination.Width, (int)Main.ActualScreenSize.Y - Main.renderer.Destination.Bottom);

        RenderTarget2D PreviewTarget { get; set; }

        public float ScrollValue;
        int ScrollRoom;
        bool Active;
        float BarAlpha;

        float PreviewAlpha;
        public BottomModeSelectPreview()
        {
            PreviewTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2000, 2000);
        }
        protected override void OnUpdate()
        {
        }
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            if (Main.CurrentScene.Name != "Main Menu")
            {
                Utils.DrawBoxFill(dimensions, new Color(20, 20, 20));

                if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode))
                {
                    PreviewAlpha = PreviewAlpha.ReciprocateTo(1);

                    dimensions = new Rectangle(v, PreivewDimensions);
                    int Height = EditorModeGUI.GetActiveScreen().PreviewHeight;

                    if (Height != 0)
                        ScrollRoom = Math.Max(Height - PreivewDimensions.Y, 0);
                    else ScrollRoom = 0;

                    int Room = (int)(Math.Max(Height - PreivewDimensions.Y, 0) / (float)Height * PreivewDimensions.Y);

                    Main.renderer.AddTargetCall(PreviewTarget, EditorModeGUI.GetActiveScreen().DrawToBottomPanel);

                    spriteBatch.Draw(PreviewTarget, new Rectangle(v, PreivewDimensions), new Rectangle(new Point(0, (int)ScrollValue), PreivewDimensions), Color.White * PreviewAlpha);

                    if (Active)
                    {
                        Utils.DrawRectangle(dimensions, Color.White * Time.SineTime(4f) * PreviewAlpha);
                        int BarWidth = 20;
                        int Padding = 10;
                        Utils.DrawRectangle(new Rectangle(
                            dimensions.Right - BarWidth - Padding,
                            dimensions.Y + Padding + (int)(ScrollValue * (PreivewDimensions.Y / (float)Height)),
                            BarWidth,
                            dimensions.Height - Padding * 2 - Room),
                            Color.White * BarAlpha);
                    }
                }
                else
                {
                    PreviewAlpha = PreviewAlpha.ReciprocateTo(0);
                }
            }
        }
        protected override void OnLeftClick()
        {
            Active = true;
        }
        float ScrollSpeed;
        protected override void OnHover()
        {
            if (Active)
            {
                BarAlpha += (1 - BarAlpha) / 16f;

                if (GameInput.Instance["EditorZoomIn"].IsDown())
                {
                    ScrollSpeed -= 3;
                }
                if (GameInput.Instance["EditorZoomOut"].IsDown())
                {
                    ScrollSpeed += 3;
                }

                ScrollSpeed *= 0.8f;
                ScrollValue += ScrollSpeed;

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


