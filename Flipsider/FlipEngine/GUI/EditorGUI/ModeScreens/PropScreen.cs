
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    public enum PropMode
    {
        PropSelect,
        PropCollidable
    }
    internal class PropScreen : ModeScreen
    {
        private PropPreviewPanel[]? propPanel;
        private PropCollideablePreview? propColPanel;
        public static string? CurrentProp;
        public static PropMode CurrentMode { get; set; }

        public override Mode Mode => Mode.Prop;

        public static int Rows => 4;
        public override int PreviewHeight
        {
            get
            {
                if (propPanel != null && propColPanel != null)
                {
                    if (CurrentMode == PropMode.PropCollidable) return propColPanel.RelativeDimensions.Height;
                    return (propPanel.Length / Rows) * 40 + 70;
                }
                else return 0;
            }
        }

        public override int PreviewWidth
        {
            get
            {
                if (propColPanel != null && CurrentMode == PropMode.PropCollidable)
                    return (propColPanel.RelativeDimensions.Width);
                else return 0;
            }
        }

        public override void CustomDrawToScreen()
        {
            int alteredRes = FlipGame.World.TileRes / 4;
            Vector2 tilePoint2 = FlipGame.MouseToDestination().ToVector2().Snap(alteredRes);

            if (CurrentProp != null && CurrentMode != PropMode.PropCollidable)
            {
                Rectangle altFrame = PropManager.PropTypes[CurrentProp].Bounds;
                FlipGame.spriteBatch.Draw(PropManager.PropTypes[CurrentProp],
                    tilePoint2 + new Vector2(alteredRes / 2), altFrame, Color.White * Math.Abs(Time.SineTime(6)),
                    0f, altFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            if (CurrentMode == PropMode.PropCollidable)
            {
                propColPanel?.Draw(sb);
                propColPanel?.Update();
            }
            else
            {
                if (propPanel != null)
                {
                    for (int i = 0; i < propPanel.Length; i++)
                    {
                        propPanel[i].Draw(sb);
                        propPanel[i].Update();
                    }
                }
            }
        }
        public override void CustomUpdate()
        {
            Vector2 mousePos = FlipGame.MouseToDestination().ToVector2().Snap(2);

            if (GameInput.Instance.JustClickingLeft && Utils.MouseInBounds && CurrentMode != PropMode.PropCollidable)
            {
                FlipGame.World.propManager.AddProp(CurrentProp ?? "", mousePos);
            }
        }

        protected override void OnLoad()
        {
            CurrentMode = PropMode.PropSelect;

            propPanel = new PropPreviewPanel[PropManager.PropTypes.Count];
            propColPanel = new PropCollideablePreview(EditorModeGUI.ModePreview);

            if (propPanel.Length != 0)
            {
                for (int i = 0; i < propPanel.Length; i++)
                {
                    propPanel[i] = new PropPreviewPanel(EditorModeGUI.ModePreview);
                    propPanel[i].Type = i;
                }
            }
        }
    }

    internal class PropPreviewPanel : PreviewElement
    {
        public PropPreviewPanel(ScrollPanel p) : base(p) { }

        public int Type;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PreviewPanel != null)
            {
                RelativeDimensions = new Rectangle(10 + (Type % PropScreen.Rows) * 35, 20 + (Type / PropScreen.Rows) * 35, 32, 32);

                if (PropManager.PropTypes.Keys.ToArray()[Type] == PropScreen.CurrentProp)
                    Utils.DrawRectangle(RelativeDimensions, Color.Yellow * Time.SineTime(6));

                spriteBatch.Draw(PropManager.PropTypes.Values.ToArray()[Type], RelativeDimensions, PropManager.PropTypes.Values.ToArray()[Type].Bounds, Color.White);
            }
        }

        protected override void OnLeftClick()
        {
            PropScreen.CurrentProp = PropManager.PropTypes.Keys.ToArray()[Type];
        }

        protected override void OnRightClick()
        {
            PropScreen.CurrentProp = PropManager.PropTypes.Keys.ToArray()[Type];
            PropCollideablePreview.AABBSets.Clear();

            AABBCollisionSet buffer = new AABBCollisionSet();

            string PropName = PropScreen.CurrentProp;
            string Path = Utils.CollisionSetPath + PropName + ".abst";

            if (File.Exists(Path))
            {
                Stream stream = File.OpenRead(Path);
                AABBCollisionSet set = buffer.Deserialize(stream);

                PropCollideablePreview.AABBSets = set.AABBs.ToList();
            }

            PropScreen.CurrentMode = PropMode.PropCollidable;
        }
    }

    internal class PropCollideablePreview : PreviewElement
    {
        public PropCollideablePreview(ScrollPanel p) : base(p) { }
        private readonly int GrideSize = 4;

        public static List<RectangleF> AABBSets = new List<RectangleF>();
        public Point Start;
        public Point Size;

        public void AddAABB(Rectangle r)
        {
            if (r.Width != 0 && r.Height != 0)
            {
                Rectangle rO = RelativeDimensions;
                AABBSets.Add(new RectangleF(
                    (r.X - rO.X) / (float)rO.Width,
                    (r.Y - rO.Y) / (float)rO.Height,
                    r.Width / (float)rO.Width,
                    r.Height / (float)rO.Height));
            }
        }

        public void SaveAABBSet()
        {
            AABBCollisionSet set = new AABBCollisionSet();
            set.AABBs = AABBSets.ToHashSet();
            string path = Utils.CollisionSetPath + PropScreen.CurrentProp + ".abst";
            Stream stream = File.OpenWrite(path);

            set.Serialize(stream);

            Logger.NewText("Collision Set Saved at: " + path);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PreviewPanel != null
               && PropScreen.CurrentProp != null)
            {
                Point p = Mouse.GetState().Position.Sub(
                    PreviewPanel.dimensions.Location).Add(
                    new Point((int)PreviewPanel.ScrollValueX, (int)PreviewPanel.ScrollValueY));


                if (GameInput.Instance.JustClickingLeft) Start = p;
                if (GameInput.Instance.IsClicking) Size = p.Sub(Start);

                Texture2D cPropTex = PropManager.PropTypes[PropScreen.CurrentProp];

                Rectangle rO = RelativeDimensions;
                RelativeDimensions = cPropTex.Bounds;

                Rectangle r = new Rectangle(Start, Size).Snap(4);

                spriteBatch.Draw(cPropTex, RelativeDimensions, cPropTex.Bounds, Color.White);

                Utils.DrawRectangle(RelativeDimensions);

                for (int i = RelativeDimensions.X; i < RelativeDimensions.Right; i += GrideSize)
                {
                    Utils.DrawLine(new Vector2(i, RelativeDimensions.Top), new Vector2(i, RelativeDimensions.Bottom), Color.FloralWhite * 0.2f, 1);
                }

                for (int i = RelativeDimensions.Y; i < RelativeDimensions.Bottom; i += GrideSize)
                {
                    Utils.DrawLine(new Vector2(RelativeDimensions.Left, i), new Vector2(RelativeDimensions.Right, i), Color.FloralWhite * 0.2f, 1);
                }

                if (GameInput.Instance.IsClicking) Utils.DrawRectangle(r, Color.Red, 2);

                foreach (RectangleF rf in AABBSets)
                {
                    Utils.DrawRectangle(new RectangleF(
                            rO.Location.Add(new Vector2(rf.x * rO.Width, rf.y * rO.Height)).ToVector2(),
                            rO.Size.Dot(new Vector2(rf.width, rf.height)).ToVector2()), Color.Red, 2);
                }

                if (GameInput.Instance.JustReleasedLeft) AddAABB(r);
                if (Flipsider.Utils.JustSaved) SaveAABBSet();

            }
        }

        protected override void OnRightClick()
        {
            PropScreen.CurrentMode = PropMode.PropSelect;
        }
    }
}


