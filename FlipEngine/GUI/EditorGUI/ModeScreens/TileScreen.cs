﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlipEngine
{
    internal class TileScreen : ModeScreen
    {
        public static int currentType;
        public static Rectangle currentFrame;
        public static bool AutoFrame = true;
        public override Mode Mode => Mode.Tile;

        private TilePreviewPanel[]? tilePanel;
        ButtonScroll? AutoFrameButton;
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
            int tileRes = TileManager.tileRes;

            var world = FlipGame.World;
            var tileDict = TileManager.tileDict;

            int modifiedRes = (int)(tileRes * FlipGame.Camera.Scale);

            Vector2 mousePos = FlipGame.MouseToDestination().ToVector2();
            Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);

            float sine = Time.SineTime(2f);
            Vector2 offsetSnap = new Vector2((int)FlipGame.Camera.Offset.X, (int)FlipGame.Camera.Offset.Y);

            Rectangle TileFrame = AutoFrame ? Framing.GetTileFrame(world, 
                (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes) : currentFrame;

            if (currentType == -1)
            {
                Utils.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
            }
            else
            {
                if (tileDict.ContainsKey(currentType))
                {
                    if (tileDict[currentType] != null)
                        FlipGame.spriteBatch.Draw(tileDict[currentType], tilePoint + new Vector2(tileRes / 2), TileFrame, Color.White *
                            Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
                }
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

            AutoFrameButton?.Draw(sb);
            AutoFrameButton?.Update();
        }

        public override void CustomUpdate()
        {
            if (Utils.MouseInBounds)
            {
                if (GameInput.Instance.IsClicking)
                {
                    Logger.NewText(FlipGame.MouseTile);
                    FlipGame.tileManager.AddTile(FlipGame.World, new Tile(currentType, currentFrame, FlipGame.MouseTile));
                }

                if (GameInput.Instance.IsRightClicking)
                {
                    FlipGame.World.tileManager.RemoveTile(FlipGame.World, FlipGame.MouseTile.ToPoint());
                }
            }
        }
        protected override void OnLoad()
        {
            tilePanel = new TilePreviewPanel[TileManager.tileTypes.Count];
            AutoFrameButton = new ButtonScroll();

            AutoFrameButton.Texture = FlipTextureCache.TileGUIPanels;
            AutoFrameButton.OptionalText = "        Auto Frame Toggle";
            AutoFrameButton.RelativeDimensions = new Rectangle(0,0,10,10);
            AutoFrameButton.OnClick = () =>
            {
                AutoFrame = !AutoFrame;
                Logger.NewText("AutoFrame set to " + AutoFrame);
            };
            AutoFrameButton.ScrollParent = EditorModeGUI.ModePreview;
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

                if (TileManager.tileDict[Type] != null)
                {
                    spriteBatch.Draw(TileManager.tileDict[Type], RelativeDimensions, Color.Lerp(Color.Gray, Color.White, ColorLerp));
                    Utils.DrawRectangle(RelativeDimensions, Color.Lerp(Color.Gray, Color.White, ColorLerp), 1);
                }

                Rectangle chooseArea = new Rectangle(dimensions.X, dimensions.Y, AtlasDimensions.X, AtlasDimensions.Y);

                if (chooseArea.Contains(Mouse.GetState().Position))
                {
                    int DimTileRes = TileManager.tileRes / 2;
                    Vector2 tilePoint = (Mouse.GetState().Position.ToVector2() - new Vector2(dimensions.X, dimensions.Y)).Snap(DimTileRes) + new Vector2(RelativeDimensions.X, RelativeDimensions.Y);
                    Utils.DrawRectangle(new Rectangle(tilePoint.ToPoint(), new Point(DimTileRes, DimTileRes)), Color.Yellow, 2);
                }
            }
        }

        protected override void OnLeftClick()
        {
            int tileRes = TileManager.tileRes;

            TileScreen.currentType = Type;
            Rectangle chooseArea = new Rectangle(dimensions.X, dimensions.Y, AtlasDimensions.X, AtlasDimensions.Y);
            int DimTileRes = TileManager.tileRes / 2;
            if (chooseArea.Contains(Mouse.GetState().Position))
            {
                Vector2 tilePoint = (Mouse.GetState().Position.ToVector2() - new Vector2(DimTileRes / 2)).Snap(DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                TileScreen.currentFrame = new Rectangle((int)(tilePoint.X - chooseArea.X) * 2, (int)(tilePoint.Y - chooseArea.Y) * 2, tileRes, tileRes);
            }
        }

        protected override void CustomUpdate() { }
    }
}


