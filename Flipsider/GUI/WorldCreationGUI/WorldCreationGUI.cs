using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;
using System.Windows.Input;
using Flipsider.Engine.Input;
using System.IO;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class WorldCreationGUI : UIScreen
    {
        public int chosen = -1;
        protected override void OnLoad()
        {
            UIStringInput textBox = new UIStringInput();
            textBox.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 150, 40, 16, 16);
            elements.Add(textBox);
            string[] files = Directory.GetFiles(Main.MainPath.Remove(Main.MainPath.Length - 1), "*.flip");
            for (int i = 0; i < files.Length; i++)
            {
                WorldLoad wlpanel = new WorldLoad();
                wlpanel.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 160, 150 + i * 40, 180, 30);
                wlpanel.path = Path.GetFileName(files[i]);
                wlpanel.index = i;
                elements.Add(wlpanel);
            }
            Save Save = new Save();
            Save.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 150, 100, TextureCache.SaveTex.Width, TextureCache.SaveTex.Height);
            elements.Add(Save);
        }

        protected override void OnUpdate()
        {
        }
        protected override void OnDraw()
        {

        }
    }
    internal class TextBox : UIElement
    {
        public string inputText = "";
        public float alpha = 1f;
        private int delay = 0;
        public bool isActive;
        Texture2D? Texture;
        KeyboardState oldKeyboardState = Keyboard.GetState();
        KeyboardState currentKeyboardState = Keyboard.GetState();
        private void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyUp(key) && key != Keys.OemSemicolon)
                {
                    KeyboardInput.Instance?.InputKey(key, ref inputText);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D GottenTexture = Texture ?? TextureCache.magicPixel;
            Main.spriteBatch.Draw(GottenTexture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), Color.Black * alpha);
            DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(GottenTexture.Height / 2, 7));
            UpdateInput();
        }
        protected override void OnLeftClick()
        {
            isActive = true;
        }
        protected override void OnLeftClickAway()
        {
            isActive = false;
        }
    }
    internal class NumberBox : UIElement
    {
        public string inputText = "";
        public float alpha = 1f;
        public bool isActive;
        public float Number => float.Parse(inputText);
        Texture2D? Texture;
        protected virtual void CustomDraw(SpriteBatch spriteBatch)
        {

        }
        KeyboardState oldKeyboardState = Keyboard.GetState();
        KeyboardState currentKeyboardState = Keyboard.GetState();
        private void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                bool IsNumber = (key >= Keys.D0
                                         &&
                                        key <= Keys.D9 || key == Keys.Back || key == Keys.OemPeriod);
                if (oldKeyboardState.IsKeyUp(key) && IsNumber)
                {
                    KeyboardInput.Instance?.InputKey(key, ref inputText);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            dimensions.Width = 16 + (inputText.Length - 1) * 7;
            Texture2D GottenTexture = Texture ?? TextureCache.magicPixel;
            Main.spriteBatch.Draw(GottenTexture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), Color.Black * alpha);
            DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(2, 2));
            UpdateInput();
            CustomDraw(spriteBatch);
        }
        protected override void OnLeftClick()
        {
            isActive = true;
        }
        protected override void OnLeftClickAway()
        {
            isActive = false;
        }
    }
    internal class UIStringInput : UIElement
    {
        private string inputText = "";
        private float alpha = 0f;
        private int delay = 0;
        KeyboardState oldKeyboardState = Keyboard.GetState();
        KeyboardState currentKeyboardState = Keyboard.GetState();
        private void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyUp(key) && key != Keys.OemSemicolon)
                {
                    KeyboardInput.Instance?.InputKey(key, ref inputText);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Main.spriteBatch.Draw(TextureCache.WorldSavePanel, new Rectangle(dimensions.X - 10, dimensions.Y - 20, 180, 90), Color.White * alpha);
            Main.spriteBatch.Draw(TextureCache.SaveTex, new Rectangle(dimensions.X + dimensions.Width / 4, dimensions.Y - 20 + (int)(Math.Sin(Main.gameTime.TotalGameTime.TotalMilliseconds / 120f) * 3), TextureCache.SaveTex.Width, TextureCache.SaveTex.Height), Color.White * alpha);
            if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
            {
                UpdateInput();
                alpha += (1 - alpha) / 16f;
                dimensions.X += (int)(Main.ActualScreenSize.X - 150 - dimensions.X) / 16;
                if (keyboard.IsKeyDown(Keys.Enter) && inputText != "")
                {
                    SaveCurrentWorldAs(inputText);
                    inputText = "";
                }
                else
                {
                    DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(26, 20));
                }
            }
            else
            {
                alpha += (-1 - alpha) / 16f;
            }
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnLeftClick()
        {

        }
        protected override void OnHover()
        {

        }

        protected override void NotOnHover()
        {
        }
    }
    internal class WorldLoad : UIElement
    {
        private float alpha = 0f;
        private int delay = 0;
        public string path = "";
        public int index;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(TextureCache.Textbox, new Rectangle(dimensions.X, dimensions.Y, 180, 30), Color.White * alpha);
            DrawMethods.DrawTextToLeft(path, Color.White * alpha, new Vector2(dimensions.X + 30, dimensions.Y + 10));

        }
        protected override void OnUpdate()
        {
            if (delay > 0)
                delay--;
            if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
            {
                alpha += (1 - alpha) / 16f;
                dimensions.X += (int)(Main.ActualScreenSize.X - 180 - dimensions.X) / 16;
            }
            else
            {
                alpha += (-1 - alpha) / 16f;
            }
        }
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
            {
                Main.CurrentWorld.RetreiveLevelInfo(path);
            }
        }
    }
    internal class Save : UIElement
    {
        private float alpha = 0f;
        private int delay = 0;
        public string path = "";
        public int index;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(TextureCache.SaveTex, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), Color.White * alpha);
        }
        protected override void OnUpdate()
        {
            if (delay > 0)
                delay--;
            if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
            {
                alpha += (1 - alpha) / 16f;
                dimensions.X += (int)(Main.ActualScreenSize.X - 180 - dimensions.X) / 16;
            }
            else
            {
                alpha += (-1 - alpha) / 16f;
            }
        }
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
            {
                int i = 1;
                while (File.Exists(Main.MainPath + "FlipWorld" + i + ".flip"))
                {
                    i++;
                }
                SaveCurrentWorldAsWithExtension(Main.Editor.CurrentSaveFile ?? "FlipWorld" + i + ".flip");
            }
        }
    }
}


