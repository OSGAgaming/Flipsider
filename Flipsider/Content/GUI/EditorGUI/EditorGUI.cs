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
    public enum Mode
    {
        None,
        Tile,
        Water,
        Prop,
        NPC,
        Pencil,
        Select,
        Lasso
    }

    //Hello Im doing work. THis is work. I am working. This is a work thingie. Give me raise lmfao xd
    internal class EditorModeGUI : UIScreen
    {
        List<EditorModeButton> Buttons = new List<EditorModeButton>();

        public static Dictionary<Mode, ModeScreen> ModeScreens = new Dictionary<Mode, ModeScreen>();

        Vector2 v => new Vector2(Main.ActualScreenSize.X - IconDimensions, 0);

        public static Mode mode;

        public static bool Active;

        public static int IconDimensions => 32;

        public static bool CanZoom = true;

        public void AddMode(Mode mode)
        {
            EditorModeButton e = new EditorModeButton(Textures._GUI_flip_icons, mode, v);
            Buttons.Add(e);
            elements.Add(Buttons[Buttons.Count - 1]);
        }

        public static ModeScreen GetActiveScreen() => ModeScreens[mode];

        public static void AddScreen(ModeScreen Screen) => ModeScreens.Add(Screen.Mode, Screen);

        public static ActiveModeSelectPreview? B;

        protected override void OnLoad()
        {
            AddMode(Mode.Tile);
            AddMode(Mode.Water);
            AddMode(Mode.Prop);
            AddMode(Mode.NPC);
            AddMode(Mode.Pencil);
            AddMode(Mode.Select);
            AddMode(Mode.Lasso);

            ActiveButton A = new ActiveButton();
            elements.Add(A);

            B = new ActiveModeSelectPreview();
            elements.Add(B);
        }

        protected override void OnUpdate()
        {
            if (Active)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].v.Y = Buttons[i].v.Y.ReciprocateTo(v.Y + (i + 1) * IconDimensions, 16f);
                    Buttons[i].v.X = v.X;
                }

                float scrollSpeed = 0.02f;
                float camMoveSpeed = 5f;

                if (CanZoom)
                {
                    if (GameInput.Instance["EditorZoomIn"].IsDown())
                    {
                        Main.Camera.targetScale += scrollSpeed;
                    }
                    if (GameInput.Instance["EditorZoomOut"].IsDown())
                    {
                        Main.Camera.targetScale -= scrollSpeed;
                    }
                }

                if (GameInput.Instance["MoveRight"].IsDown())
                {
                    Main.Camera.Offset.X += camMoveSpeed;
                }
                if (GameInput.Instance["MoveLeft"].IsDown())
                {
                    Main.Camera.Offset.X -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveUp"].IsDown())
                {
                    Main.Camera.Offset.Y -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveDown"].IsDown())
                {
                    Main.Camera.Offset.Y += camMoveSpeed;
                }
            }
            else
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].v.Y = Buttons[i].v.Y.ReciprocateTo(v.Y, 8f);
                    Buttons[i].v.X = v.X;
                }

                Main.Camera.Offset -= Main.Camera.Offset / 16f;
            }

            CanZoom = true;
        }
        internal override void OnDrawToScreenDirect()
        {
            Rectangle d = Main.renderer.Destination;

            Utils.DrawBoxFill(new Rectangle((int)v.X, 0, IconDimensions, (int)Main.ActualScreenSize.Y), new Color(30, 30, 30));
            Utils.DrawBoxFill(new Rectangle(0, 0, d.X, (int)Main.ActualScreenSize.Y), new Color(30, 30, 30));

            Utils.DrawBoxFill(new Rectangle(d.X, d.Bottom, d.Width, (int)Main.ActualScreenSize.Y - d.Height - d.Y), new Color(20, 20, 20));

            Utils.DrawRectangle(d, Color.White);
        }
    }

    internal class EditorModeButton : UIElement
    {
        Texture2D tex;
        Mode mode;

        public Vector2 v;
        public EditorModeButton(Texture2D tex, Mode mode, Vector2 v)
        {
            this.tex = tex;
            this.mode = mode;
            this.v = v;
        }
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            int Dims = EditorModeGUI.IconDimensions;

            dimensions = new Rectangle(v.ToPoint(), new Point(Dims, Dims));

            Rectangle source;

            if (!IsBeingClicked)
                source = new Rectangle(new Point(0, (int)(mode - 1) * Dims * 2), new Point(Dims * 2, Dims * 2));
            else
                source = new Rectangle(new Point(Dims * 2, (int)(mode - 1) * Dims * 2), new Point(Dims * 2, Dims * 2));

            spriteBatch.Draw(tex, dimensions, source, Color.White);
            spriteBatch.Draw(tex, dimensions, source, Color.White);
        }
        protected override void OnLeftClick()
        {
            EditorModeGUI.mode = mode;
        }
    }

    internal class ActiveButton : UIElement
    {
        Vector2 v => new Vector2(Main.ActualScreenSize.X - EditorModeGUI.IconDimensions, 0);
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            dimensions = new Rectangle(v.ToPoint(), new Point(EditorModeGUI.IconDimensions, EditorModeGUI.IconDimensions));
            spriteBatch.Draw(Textures._PointLight, dimensions, Color.White);
        }
        protected override void OnLeftClick()
        {
            EditorModeGUI.Active = !EditorModeGUI.Active;
            EditorModeGUI.mode = Mode.None;

            if(EditorModeGUI.Active) Main.Camera.targetScale = 0.8f;
            else Main.Camera.targetScale = 2f;

        }
    }

    internal class ActiveModeSelectPreview : UIElement
    {
        Point v => new Point(20, 32);

        Point PreivewDimensions => new Point(160, Main.renderer.Destination.Height);

        RenderTarget2D PreviewTarget { get; set; }

        public float ScrollValue;
        int ScrollRoom;
        bool Active;
        float BarAlpha;

        float PreviewAlpha;
        public ActiveModeSelectPreview()
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
                Utils.DrawBoxFill(dimensions, Color.Black);

                if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode))
                {
                    PreviewAlpha = PreviewAlpha.ReciprocateTo(1);

                    dimensions = new Rectangle(v, PreivewDimensions);
                    int Height = EditorModeGUI.GetActiveScreen().PreviewHeight;

                    if (Height != 0)
                        ScrollRoom = Math.Max(Height - PreivewDimensions.Y, 0);
                    else ScrollRoom = 0;

                    int Room = (int)(Math.Max(Height - PreivewDimensions.Y, 0) / (float)Height * PreivewDimensions.Y);

                    Main.renderer.AddTargetCall(PreviewTarget, EditorModeGUI.GetActiveScreen().DrawToSelector);

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


