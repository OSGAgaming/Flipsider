
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class PropScreen : ModeScreen
    {
        private PropPreviewPanel[]? propPanel;
        public static string? CurrentProp;
        public override Mode Mode => Mode.Prop;

        public override int PreviewHeight
        {
            get
            {
                if (propPanel != null)
                    return (propPanel.Length / 3) * 70 + 70;
                else return 0;
            }
        }
        public override void CustomDrawToScreen()
        {
            int alteredRes = FlipGame.World.TileRes / 4;
            Vector2 tilePoint2 = FlipGame.MouseToDestination().ToVector2().Snap(alteredRes);

            if (CurrentProp != null)
            {
                Rectangle altFrame = PropManager.PropTypes[CurrentProp].Bounds;
                FlipGame.spriteBatch.Draw(PropManager.PropTypes[CurrentProp], 
                    tilePoint2 + new Vector2(alteredRes / 2), altFrame, Color.White * Math.Abs(Time.SineTime(6)), 
                    0f, altFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void DrawToSelector(SpriteBatch sb)
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
        public override void CustomUpdate()
        {
            Vector2 mousePos = FlipGame.MouseToDestination().ToVector2().Snap(2);

            if (GameInput.Instance.JustClickingLeft && Utils.MouseInBounds)
            {
                FlipGame.World.propManager.AddProp(CurrentProp ?? "", mousePos);
            }
        }

        protected override void OnLoad()
        {
            propPanel = new PropPreviewPanel[PropManager.PropTypes.Count];
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
                RelativeDimensions = new Rectangle(10 + (Type % 3) * 55, 20 + (Type / 3) * 70, 32, 32);

                if (PropManager.PropTypes.Keys.ToArray()[Type] == PropScreen.CurrentProp)
                    Utils.DrawRectangle(RelativeDimensions, Color.Yellow * Time.SineTime(6));
                spriteBatch.Draw(PropManager.PropTypes.Values.ToArray()[Type], RelativeDimensions, PropManager.PropTypes.Values.ToArray()[Type].Bounds, Color.White);
            }
        }

        protected override void OnLeftClick()
        {
            PropScreen.CurrentProp = PropManager.PropTypes.Keys.ToArray()[Type];
        }
    }
}


