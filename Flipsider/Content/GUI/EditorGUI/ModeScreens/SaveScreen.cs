using Flipsider.Engine.Input;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class SaveScreen : ModeScreen
    {
        TextBoxScalableScroll? TextBox;
        WorldLoadScroll[]? Load;
        public override Mode Mode => Mode.Select;

        public override int PreviewHeight
        {
            get
            {
                if (Load != null)
                    return Load.Length * 50 + 100;
                else return 0;
            }
        }
        public override void CustomDrawToScreen()
        {

        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            TextBox?.Draw(sb);

            Utils.DrawTextToLeft("Save World As:", Color.Yellow, new Vector2(10, 10), 0.1f);

            Utils.DrawTextToLeft("Load World", Color.Yellow, new Vector2(10, 75), 0.1f);

            if (Load != null)
            {
                for (int i = 0; i < Load.Length; i++)
                {
                    Load[i].Draw(sb);
                    Load[i].Update();
                }
            }
        }

        public override void CustomUpdate()
        {
            if (TextBox != null)
            {
                TextBox.RelativeDimensions.Location = new Point(20, 40);
                TextBox.RelativeDimensions.Height = 20;

                TextBox?.Update();
            }
        }
        protected override void OnLoad()
        {
            TextBox = new TextBoxScalableScroll();
            TextBox.OnEnterEvent = (string text) =>
            {
                Utils.SaveCurrentWorldAs(TextBox.inputText);
                TextBox.inputText = "";

                Load = null;

                string[] files = Directory.GetFiles(Main.MainPath.Remove(Main.MainPath.Length - 1), "*.flip");
                Load = new WorldLoadScroll[files.Length];

                for (int i = 0; i < files.Length; i++)
                {
                    Load[i] = new WorldLoadScroll(EditorModeGUI.ModePreview);
                    Load[i].index = i;
                    Load[i].path = files[i];
                }
            };

            string[] files = Directory.GetFiles(Main.MainPath.Remove(Main.MainPath.Length - 1), "*.flip");
            Load = new WorldLoadScroll[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Load[i] = new WorldLoadScroll(EditorModeGUI.ModePreview);
                Load[i].index = i;
                Load[i].path = files[i];
            }
        }

        internal override void OnDrawToScreenDirect() { }
    }

    internal class WorldLoadScroll : PreviewElement
    {
        public string path = "";
        public int index;

        public WorldLoadScroll(ScrollPanel p) : base(p) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(20, 100 + 50*index, 120, 30);

            Utils.DrawTextToLeft(Path.GetFileName(path), Color.White, new Vector2(RelativeDimensions.X, RelativeDimensions.Y), 0.1f);

        }
        protected override void OnLeftClick()
        {
            Main.CurrentWorld.RetreiveLevelInfo(Path.GetFileName(path));
        }
    }

}


