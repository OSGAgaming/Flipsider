using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Scenes;
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
        Lasso,
        Undo,
        Redo,
        Move,
        Fill,
        Camera,
        Cutscene,
        Collideables
    }

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
            EditorModeButton e = new EditorModeButton(Textures._GUI_FlipIcons, mode, v);
            Buttons.Add(e);
            elements.Add(Buttons[Buttons.Count - 1]);
        }

        public static ModeScreen GetActiveScreen() => ModeScreens[mode];

        public static void AddScreen(ModeScreen Screen) => ModeScreens.Add(Screen.Mode, Screen);

        public static ActiveModeSelectPreview ModePreview = new ActiveModeSelectPreview();
        public static BottomModeSelectPreview BottomPreview = new BottomModeSelectPreview();

        protected override void OnLoad()
        {
            AddMode(Mode.Tile);
            AddMode(Mode.Water);
            AddMode(Mode.Prop);
            AddMode(Mode.NPC);
            AddMode(Mode.Pencil);
            AddMode(Mode.Select);
            AddMode(Mode.Lasso);
            AddMode(Mode.Undo);
            AddMode(Mode.Redo);
            AddMode(Mode.Move);
            AddMode(Mode.Fill);
            AddMode(Mode.Camera);
            AddMode(Mode.Cutscene);
            AddMode(Mode.Collideables);

            ActiveButton A = new ActiveButton();
            elements.Add(A);

            GamePlayButton Play = new GamePlayButton();
            elements.Add(Play);

            elements.Add(ModePreview);
            elements.Add(BottomPreview);

            active = false;
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

                if (!CutsceneManager.Instance.IsPlayingCutscene) Main.Camera.Offset -= Main.Camera.Offset / 16f;
            }

            CanZoom = true;
        }

        protected override void OnUpdatePassive() => active = Main.CurrentScene.Name == "Editor";

        internal override void OnDrawToScreenDirect()
        {
            Rectangle d = Main.renderer.Destination;

            if (Main.CurrentScene.Name == "Editor")
            {
                Utils.DrawBoxFill(new Rectangle((int)v.X, 0, IconDimensions, (int)Main.ActualScreenSize.Y), new Color(30, 30, 30));
                Utils.DrawBoxFill(new Rectangle(0, 0, d.X, (int)Main.ActualScreenSize.Y), new Color(30, 30, 30));

                Utils.DrawRectangle(d, Color.White);
            }
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
                source = new Rectangle(new Point(0, (int)mode * Dims), new Point(Dims, Dims));
            else
                source = new Rectangle(new Point(Dims * 2, (int)mode * Dims), new Point(Dims, Dims));

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
            Utils.DrawBoxFill(dimensions, new Color(30, 30, 30));
            spriteBatch.Draw(Textures._GUI_FlipIcons, dimensions,new Rectangle(EditorModeGUI.Active ? 32 : 0,0,32,32), Color.White);
        }
        protected override void OnLeftClick()
        {
            EditorModeGUI.Active = !EditorModeGUI.Active;
            EditorModeGUI.mode = Mode.None;

            if(EditorModeGUI.Active) Main.Camera.targetScale = 0.8f;
            else Main.Camera.targetScale = 2f;

        }
    }

    internal class GamePlayButton : UIElement
    {
        Vector2 v => new Vector2(0, 0);
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            dimensions = new Rectangle(v.ToPoint(), new Point(EditorModeGUI.IconDimensions, EditorModeGUI.IconDimensions));
            spriteBatch.Draw(Textures._PointLight, dimensions, Color.White);
        }
        protected override void OnLeftClick()
        {
            Main.instance.sceneManager.SetNextScene(EditorScene.DisplayScene, null);
        }
    }
}


