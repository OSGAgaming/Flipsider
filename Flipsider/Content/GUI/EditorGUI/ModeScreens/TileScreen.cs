using Flipsider.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using static Flipsider.TileManager;

namespace Flipsider.GUI
{
    internal class TileScreen : ModeScreen
    {
        public static int currentType;
        public static Rectangle currentFrame;
        public static bool AutoFrame = true;
        public override Mode Mode => Mode.Tile;

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
            var world = Main.World;
            var tileDict = Main.tileManager.tileDict;

            int modifiedRes = (int)(tileRes * Main.Camera.Scale);

            Vector2 mousePos = Main.MouseToDestination().ToVector2();
            Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);

            float sine = Time.SineTime(2f);
            Vector2 offsetSnap = new Vector2((int)Main.Camera.Offset.X, (int)Main.Camera.Offset.Y);

            Rectangle TileFrame = Main.Editor.AutoFrame ? Framing.GetTileFrame(world, 
                (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes) : currentFrame;

            if (Main.Editor.currentType == -1)
            {
                Utils.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
            }
            else
            {
                if (tileDict[currentType] != null)
                    Main.spriteBatch.Draw(tileDict[currentType], tilePoint + new Vector2(tileRes / 2), TileFrame, Color.White * 
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
            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                Main.tileManager.AddTile(Main.World, new Tile(currentType, currentFrame, Main.MouseTile));
            }

            if (GameInput.Instance["EdtiorRemoveTile"].IsDown())
            {
                Main.World.tileManager.RemoveTile(Main.World, Main.MouseTile.ToPoint());
            }
        }
        protected override void OnLoad()
        {
            tilePanel = new TilePreviewPanel[Main.tileManager.tileTypes.Count];
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

    internal class TilePreviewPanel : PreviewElement
    {
        public int Type;
        private Point AtlasDimensions = new Point(128, 272);
        private float ColorLerp;
        internal static int Seperation => 280;

        public TilePreviewPanel(ScrollPanel p) : base(p) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PreviewPanel != null)
            {
                RelativeDimensions = new Rectangle(20, 20 + Type * Seperation, AtlasDimensions.X, AtlasDimensions.Y);

                if (Type == TileScreen.currentType) ColorLerp = ColorLerp.ReciprocateTo(1);
                else ColorLerp = ColorLerp.ReciprocateTo(0);

                if (Main.tileManager.tileDict[Type] != null)
                {
                    spriteBatch.Draw(Main.tileManager.tileDict[Type], RelativeDimensions, Color.Lerp(Color.Gray, Color.White, ColorLerp));
                    Utils.DrawRectangle(RelativeDimensions, Color.Lerp(Color.Gray, Color.White, ColorLerp), 1);
                }
            }
        }

        protected override void OnLeftClick()
        {
            TileScreen.currentType = Type;
            Rectangle chooseArea = new Rectangle(dimensions.X, dimensions.Y, AtlasDimensions.X, AtlasDimensions.Y);
            int DimTileRes = tileRes / 2;
            if (chooseArea.Contains(Mouse.GetState().Position))
            {
                Vector2 tilePoint = Mouse.GetState().Position.ToVector2().Snap(DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                TileScreen.currentFrame = new Rectangle((int)(tilePoint.X - chooseArea.X) * 2, (int)(tilePoint.Y - chooseArea.Y) * 2, tileRes, tileRes);
            }
        }

        protected override void CustomUpdate() { }
    }
}


