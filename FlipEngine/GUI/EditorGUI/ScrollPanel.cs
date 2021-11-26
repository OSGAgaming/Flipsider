
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;


namespace FlipEngine
{
    internal class ScrollPanel : UIElement
    {
        public virtual Point v { get; }

        public virtual Point PreivewDimensions { get; }

        public virtual Color Color { get; }

        RenderTarget2D PreviewTarget { get; set; }

        public float ScrollValueY;
        public int ScrollRoomY;

        public float ScrollValueX;
        public int ScrollRoomX;

        public bool Active;
        private float BarAlpha;

        public float PreviewAlpha;

        public int PreviewHeight;
        public int PreviewWidth;

        private float ScrollVelocityY;

        private float ScrollVelocityX;

        private float ScrollSpeed = 1;
        private int BarWidth = 20;
        private int Padding = 10;
        public ScrollPanel()
        {
            PreviewTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, 2000, 2000);
        }
        protected virtual void CustomDrawDirect(SpriteBatch sb) { }
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            if (FlipGame.CurrentScene.Name != "Main Menu")
            {
                Utils.DrawBoxFill(dimensions, Color);
                PreviewAlpha = PreviewAlpha.ReciprocateTo(1);

                dimensions = new Rectangle(v, PreivewDimensions);

                FlipGame.Renderer.AddTargetCall(PreviewTarget, CustomDrawDirect);
                spriteBatch.Draw(PreviewTarget, new Rectangle(v, PreivewDimensions), new Rectangle(new Point((int)ScrollValueX, (int)ScrollValueY), PreivewDimensions), Color.White * PreviewAlpha);

                PreviewAlpha = PreviewAlpha.ReciprocateTo(0);

                if (Active)
                {
                    int Height = PreviewHeight;
                    int Width = PreviewWidth;

                    if (Height != 0)
                        ScrollRoomY = Math.Max(Height - PreivewDimensions.Y, 0);
                    else ScrollRoomY = 0;

                    if (Width != 0)
                        ScrollRoomX = Math.Max(Width - PreivewDimensions.X, 0);
                    else ScrollRoomX = 0;

                    int RoomY = (int)(Math.Max(Height - PreivewDimensions.Y, 0) / (float)Height * PreivewDimensions.Y);

                    int RoomX = (int)(Math.Max(Width - PreivewDimensions.X, 0) / (float)Width * PreivewDimensions.X);

                    Utils.DrawRectangle(dimensions, Color.White * Time.SineTime(4f) * PreviewAlpha);

                    Utils.DrawRectangle(new Rectangle(
                        dimensions.Right - BarWidth - Padding,
                        dimensions.Y + Padding + (int)(ScrollValueY * (PreivewDimensions.Y / (float)Height)),
                        BarWidth,
                        dimensions.Height - Padding * 2 - RoomY),
                        Color.White * BarAlpha);

                    Utils.DrawRectangle(new Rectangle(
                        dimensions.X + Padding + (int)(ScrollValueX * (PreivewDimensions.X / (float)Width)),
                        dimensions.Bottom - BarWidth - Padding,
                        dimensions.Width - Padding * 2 - RoomX,
                        BarWidth),
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

                ScrollVelocityY -= GameInput.Instance.DeltaScroll * ScrollSpeed * 0.01f;

                ScrollVelocityY *= 0.8f;
                ScrollValueY += ScrollVelocityY;

                ScrollValueY = Math.Clamp(ScrollValueY, 0, ScrollRoomY);

                if (Keyboard.GetState().IsKeyDown(Keys.Right)) ScrollVelocityX += ScrollSpeed;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) ScrollVelocityX -= ScrollSpeed;

                ScrollVelocityX *= 0.8f;
                ScrollValueX += ScrollVelocityX;

                ScrollValueX = Math.Clamp(ScrollValueX, 0, ScrollRoomX);
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


