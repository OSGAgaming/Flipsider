using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
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
        private Entity? ChosenEntity { get; set; }

        List<EditorModeButton> Buttons = new List<EditorModeButton>();

        public static Dictionary<Mode, ModeScreen> ModeScreens = new Dictionary<Mode, ModeScreen>();
        Vector2 v => new Vector2(FlipGame.ActualScreenSize.X - IconDimensions, 0);

        public static Mode mode;

        public static bool Active;

        public static int IconDimensions => 32;

        public static bool CanZoom = true;

        public void AddMode(Mode mode)
        {
            EditorModeButton e = new EditorModeButton(FlipTextureCache.Icons, mode, v);
            Buttons.Add(e);
            elements.Add(Buttons[Buttons.Count - 1]);
        }

        public static ModeScreen GetActiveScreen() => ModeScreens[mode];

        public static void AddScreen(ModeScreen Screen) => ModeScreens.Add(Screen.Mode, Screen);

        public static ActiveModeSelectPreview ModePreview = new ActiveModeSelectPreview();
        public static BottomModeSelectPreview BottomPreview = new BottomModeSelectPreview();
        public static BottomLeftModeSelectPreview BottomLeftPreview = new BottomLeftModeSelectPreview();

        protected override void OnLoad()
        {
            AddMode(Mode.Tile);
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
            elements.Add(BottomLeftPreview);

            active = false;
        }
        internal override void DrawToScreen()
        {
            if (ChosenEntity != null) Utils.DrawRectangle(ChosenEntity.CollisionFrame, Color.White, 3);
        }
        public bool IsDraggingEntity;
        private void ManageEntities()
        {
            Vector2 mousePos = FlipGame.MouseToDestination().ToVector2().Snap(2);

            if (Mouse.GetState().RightButton != ButtonState.Pressed)
            {
                ChosenEntity = null;

                var Chunks = FlipGame.GetActiveChunks();
                foreach (Chunk chunk in Chunks)
                {
                    foreach (Entity entity in chunk.Entities)
                    {
                        if (entity.CollisionFrame.Contains(mousePos))
                        {
                            ChosenEntity = entity;
                        }
                    }
                }
            }

            if (ChosenEntity != null)
            {
                Rectangle rect = ChosenEntity.CollisionFrame;
                if (rect.Contains(mousePos))
                {
                    if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                    {
                        ChosenEntity.Dispose();
                    }
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        IsDraggingEntity = true;
                    }
                    else
                    {
                        IsDraggingEntity = false;
                    }
                }

                if (IsDraggingEntity)
                {
                    ChosenEntity.Position +=
                                FlipGame.MouseToDestination().ToVector2() -
                                FlipGame.PreviousMouseToDestination().ToVector2();
                }
            }

            
        }

        protected override void OnUpdate()
        {
            if (Active)
            {
                KeyboardState state = Keyboard.GetState();

                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].v.Y = Buttons[i].v.Y.ReciprocateTo(v.Y + (i + 1) * IconDimensions, 16f);
                    Buttons[i].v.X = v.X;
                }

                float scrollSpeed = 0.0002f;
                float camMoveSpeed = 5f;

                if (CanZoom)
                {
                    FlipGame.Camera.targetScale += GameInput.Instance.DeltaScroll * scrollSpeed;
                }

                if (state.IsKeyDown(Keys.D))
                {
                    FlipGame.Camera.Offset.X += camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    FlipGame.Camera.Offset.X -= camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    FlipGame.Camera.Offset.Y -= camMoveSpeed;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    FlipGame.Camera.Offset.Y += camMoveSpeed;
                }
            }
            else
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].v.Y = Buttons[i].v.Y.ReciprocateTo(v.Y, 8f);
                    Buttons[i].v.X = v.X;
                }
            }

            CanZoom = true;

            ManageEntities();
        }

        protected override void OnUpdatePassive() => active = FlipGame.CurrentScene.Name == "Editor";

        internal override void OnDrawToScreenDirect()
        {
            Rectangle d = FlipGame.Renderer.Destination;

            if (FlipGame.CurrentScene.Name == "Editor")
            {
                Utils.DrawBoxFill(new Rectangle((int)v.X, 0, IconDimensions, (int)FlipGame.ActualScreenSize.Y), new Color(30, 30, 30));
                Utils.DrawBoxFill(new Rectangle(0, 0, d.X, (int)FlipGame.ActualScreenSize.Y), new Color(30, 30, 30));

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
        Vector2 v => new Vector2(FlipGame.ActualScreenSize.X - EditorModeGUI.IconDimensions, 0);
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            dimensions = new Rectangle(v.ToPoint(), new Point(EditorModeGUI.IconDimensions, EditorModeGUI.IconDimensions));
            Utils.DrawBoxFill(dimensions, new Color(30, 30, 30));
            spriteBatch.Draw(FlipTextureCache.Icons, dimensions, new Rectangle(EditorModeGUI.Active ? 32 : 0, 0, 32, 32), Color.White);
        }
        protected override void OnLeftClick()
        {
            EditorModeGUI.Active = !EditorModeGUI.Active;
            EditorModeGUI.mode = Mode.None;

            if (EditorModeGUI.Active) FlipGame.Camera.targetScale = 2f;
            else FlipGame.Camera.targetScale = 2f;
        }
    }

    internal class GamePlayButton : UIElement
    {
        Vector2 v => new Vector2(0, 0);
        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            dimensions = new Rectangle(v.ToPoint(), new Point(EditorModeGUI.IconDimensions, EditorModeGUI.IconDimensions));
            spriteBatch.Draw(FlipTextureCache.PointLight, dimensions, Color.White);
        }
        protected override void OnLeftClick()
        {
            SceneManager.Instance.SetNextScene(EditorScene.DisplayScene, null);
        }
    }
}


