


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class CutsceneScreen : ModeScreen
    {
        public override Mode Mode => Mode.Cutscene;

        public static CutsceneElement? cutsceneElement;
        CutsceneLoadScroll[]? Load;

        public override void DrawToBottomPanel(SpriteBatch sb)
        {
            cutsceneElement?.Draw(sb);
            cutsceneElement?.Update();
        }

        public override void CustomDrawToScreen()
        {
            if (CutsceneNode.controller != null && CutsceneNode.controller is VectorController c)
            {
                Utils.DrawBoxFill(new Rectangle((int)c.CameraStamp.X, (int)c.CameraStamp.Y, 20, 20), Color.Blue * Math.Abs(Time.SineTime(3f)));
            }
        }

        public override void DrawToSelector(SpriteBatch sb)
        {
            CutsceneNode.controller?.DrawToActive();

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

        }

        protected override void OnLoad()
        {
            cutsceneElement = new CutsceneElement(EditorModeGUI.BottomPreview);

            cutsceneElement.OnSave = () =>
            {
                string[] files = Directory.GetFiles(Utils.CutscenePath.Remove(Utils.CutscenePath.Length - 1), "*.fctsn");
                Load = new CutsceneLoadScroll[files.Length];

                for (int i = 0; i < files.Length; i++)
                {
                    Load[i] = new CutsceneLoadScroll(EditorModeGUI.ModePreview);
                    Load[i].index = i;
                    Load[i].path = files[i];
                }
            };

            string[] files = Directory.GetFiles(Utils.CutscenePath.Remove(Utils.CutscenePath.Length - 1), "*.fctsn");
            Load = new CutsceneLoadScroll[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Load[i] = new CutsceneLoadScroll(EditorModeGUI.ModePreview);
                Load[i].index = i;
                Load[i].path = files[i];
            }
        }

        internal override void OnDrawToScreenDirect() { }
    }

    internal class CutsceneLoadScroll : PreviewElement
    {
        public string path = "";
        public int index;

        public Action? OnClick;

        public CutsceneLoadScroll(ScrollPanel p) : base(p) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(10,60 + 20 * index, 120, 30);

            Utils.DrawTextToLeft(Path.GetFileName(path), Color.White, new Vector2(RelativeDimensions.X, RelativeDimensions.Y), 0.1f);

        }
        protected override void OnLeftClick()
        {
            if (CutsceneScreen.cutsceneElement != null)
            {
                Cutscene scene = new Cutscene();

                Stream writeStream = File.OpenRead(path);

                Cutscene newScene = scene.Deserialize(writeStream);
                CutsceneElement element = CutsceneScreen.cutsceneElement;

                VectorCutsceneNode vController = new VectorCutsceneNode(EditorModeGUI.BottomPreview, element);
                vController.Deserialize(newScene);

                element.LengthOfCutscene = newScene.Length;
                element.NameOfCutscene = newScene.ID ?? "";
                element.Name.inputText = element.NameOfCutscene;
                element.NodeLines.Clear();

                element.AddNodeLine(vController);

                OnClick?.Invoke();
            }
        }
    }

    internal class CutsceneElement : PreviewElement
    {
        public int LengthOfCutscene = 100;
        public string NameOfCutscene = "MyCutscene";
        public TextBoxScalableScroll Name;
        public List<CutsceneNode> NodeLines = new List<CutsceneNode>();
        public Action? OnSave;

        public Cutscene ToCutscene()
        {
            Cutscene scene = new Cutscene();
            scene.Length = LengthOfCutscene;
            scene.ID = NameOfCutscene;

            for (int i = 0; i < NodeLines.Count; i++)
            {
                scene = NodeLines[i].Serialize(scene);
            }

            return scene;
        }

        public void AddNodeLine(CutsceneNode line)
        {
            line.nID = NodeLines.Count + 1;
            NodeLines.Add(line);
        }

        public CutsceneElement(ScrollPanel p) : base(p)
        {
            AddNodeLine(new VectorCutsceneNode(p, this));

            Name = new TextBoxScalableScroll(EditorModeGUI.BottomPreview);

            Name.RelativeDimensions.Location = new Point(10, 32);
            Name.RelativeDimensions.Height = 14;
            Name.inputText = NameOfCutscene;
            Name.BorderWidth = 0;

            Name.OnEnterEvent = (string s) =>
            {
                NameOfCutscene = s;
            };
        }

        public Vector2 Position = new Vector2(10, 100);

        public Point GraphDimensions = new Point(300, 50);

        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawLine(spriteBatch, Position, Position + new Vector2(GraphDimensions.X, 0), Color.White);
            Utils.DrawLine(spriteBatch, Position, Position + new Vector2(0, -GraphDimensions.Y), Color.White);

            for (int i = 0; i < NodeLines.Count; i++)
            {
                NodeLines[i].Draw(spriteBatch);
            }

            Name.Draw(spriteBatch);
        }

        int CoolDown;

        protected override void CustomUpdate()
        {
            if (CoolDown > 0) CoolDown--;

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter)) 
                CutsceneManager.Instance?.StartCutscene(ToCutscene());

            if (state.IsKeyDown(Keys.LeftControl) && state.IsKeyDown(Keys.S) && CoolDown == 0)
            {
                Stream stream = File.OpenWrite(Utils.CutscenePath + NameOfCutscene + ".fctsn");
                ToCutscene().Serialize(stream);
                OnSave?.Invoke();

                CoolDown = 60;
            }

            for (int i = 0; i < NodeLines.Count; i++) 
                NodeLines[i].Update();

            Name.Update();
        }
    }
}


