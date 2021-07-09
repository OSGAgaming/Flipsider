using Flipsider.Engine.Input;
using Flipsider.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class SettingsGUI : UIScreen
    {
        public static bool On;
        public float Out;

        public Button? GoToEditor;
        protected override void OnLoad()
        {
            GoToEditor = new Button();
            GoToEditor.OptionalText = "Go To Editor";
            GoToEditor.OnClick += EditorAction;
            GoToEditor.Texture = Textures._GUI_HudSlot;
            elements.Add(GoToEditor);
        }

        protected override void OnUpdate()
        {
            if (On) Out = Out.ReciprocateTo(100);
            else Out = Out.ReciprocateTo(0);

            if(GoToEditor != null) GoToEditor.dimensions = new Rectangle((int)Out - 90, 10, 80, 20);

            if (GameInput.Instance["Esc"].IsJustPressed()) On = !On;
        }

        protected override void OnDraw()
        {
            Utils.DrawBoxFill(new Rectangle((int)Out - 100, 0, 100, (int)Main.ActualScreenSize.Y), Color.Black, 0);
        }

        public void EditorAction()
        {
            Main.instance.sceneManager.SetNextScene(new EditorScene(), null);
            On = false;
        }
    }
}


