
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;


namespace FlipEngine
{
    internal class FillScreen : ModeScreen
    {
        public override Mode Mode => Mode.Redo;

        private TilePreviewPanel[]? tilePanel;

        public override int PreviewHeight
        {
            get
            {
                if (tilePanel != null)
                    return tilePanel.Length * TilePreviewPanel.Seperation + 30;
                else return 0;
            }
        }

        public override void CustomDrawToScreen()
        {
            var world = FlipGame.World;
            var tileDict = TileManager.tileDict;
            int tileRes = TileManager.tileRes;

            int modifiedRes = (int)(tileRes * FlipGame.Camera.Scale);

            Vector2 mousePos = FlipGame.MouseToDestination().ToVector2();
            Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);

            float sine = Time.SineTime(2f);
            Vector2 offsetSnap = new Vector2((int)FlipGame.Camera.Offset.X, (int)FlipGame.Camera.Offset.Y);

            Rectangle TileFrame = TileScreen.AutoFrame ? Framing.GetTileFrame(world,
                (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes) : TileScreen.currentFrame;

            if (TileScreen.currentType == -1)
            {
                Utils.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
            }
            else
            {
                if (tileDict[TileScreen.currentType] != null)
                    FlipGame.spriteBatch.Draw(tileDict[TileScreen.currentType], tilePoint + new Vector2(tileRes / 2), TileFrame, Color.White *
                        Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
            }

            //Utils.DrawRectangle(new Rectangle(Main.MouseToDestination(), new Point(10, 10)), Color.White);
        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            if (tilePanel != null)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    tilePanel[i].Draw(sb);
                    tilePanel[i].Update();
                }
            }
        }

        public override void CustomUpdate()
        {
            MouseState state = Mouse.GetState();
            if (GameInput.Instance.IsClicking)
            {
                FlipGame.tileManager.AddTile(FlipGame.World, new Tile(TileScreen.currentType, TileScreen.currentFrame, FlipGame.MouseTile));
            }

            if (GameInput.Instance.IsRightClicking)
            {
                FlipGame.World.tileManager.RemoveTile(FlipGame.World, FlipGame.MouseTile.ToPoint());
            }
        }
        protected override void OnLoad()
        {
            tilePanel = new TilePreviewPanel[TileManager.tileTypes.Count];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    tilePanel[i] = new TilePreviewPanel(EditorModeGUI.ModePreview);
                    tilePanel[i].Type = i;
                }
            }
        }

        internal override void OnDrawToScreenDirect() { }
    }
}


