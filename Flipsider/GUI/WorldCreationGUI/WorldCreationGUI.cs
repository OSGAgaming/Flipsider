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

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class WorldCreationGUI : UIScreen
    {
        public int chosen = -1;
        protected override void OnLoad()
        {
            UIStringInput textBox = new UIStringInput();
            textBox.dimensions = new Rectangle((int)Main.ScreenSize.X - 150, 40, 16, 16);
            elements.Add(textBox);
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (delay > 0)
                delay--;
            Texture2D GottenTexture = Texture ?? TextureCache.magicPixel;
            KeyboardState keyboard = Keyboard.GetState();
            Main.spriteBatch.Draw(GottenTexture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), Color.Black * alpha);
            DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(GottenTexture.Height / 2, 7));
            if (keyboard.GetPressedKeys().Length != 0 && delay == 0 && isActive)
            {
                for (int i = 0; i < keyboard.GetPressedKeys().Length; i++)
                {
                    bool CanType = keyboard.GetPressedKeys()[i] != Keys.LeftShift && keyboard.GetPressedKeys()[i] != Keys.OemSemicolon && keyboard.GetPressedKeys()[i] != Keys.Back &&
                                keyboard.GetPressedKeys()[i] != Keys.LeftAlt &&
                                keyboard.GetPressedKeys()[i] != Keys.RightAlt &&
                                (keyboard.GetPressedKeys()[i] < Keys.D0
                                 ||
                                keyboard.GetPressedKeys()[i] > Keys.D9);
                    bool IsNumber = (keyboard.GetPressedKeys()[i] >= Keys.D0
                                     &&
                                    keyboard.GetPressedKeys()[i] <= Keys.D9);
                    Keys firstKey = keyboard.GetPressedKeys()[0];
                    if (firstKey != Keys.Enter && inputText.Length < 15)
                    {
                        if (CanType)
                            inputText += firstKey == Keys.LeftShift ? keyboard.GetPressedKeys()[i].ToString().ToUpper() : keyboard.GetPressedKeys()[i].ToString().ToLower();
                        if (IsNumber)
                            inputText += (firstKey == Keys.LeftShift ? keyboard.GetPressedKeys()[i].ToString().ToUpper() : keyboard.GetPressedKeys()[i].ToString().ToLower())[1];
                    }
                    if (firstKey == Keys.Back && delay == 0)
                    {
                        if (inputText.Length > 0)
                            inputText = inputText.Remove(inputText.Length - 1);
                    }
                }
                delay = 10;

            }
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
            private int delay = 0;
            public bool isActive;
            public float Number => float.Parse(inputText);
            Texture2D? Texture;
        protected virtual void CustomDraw(SpriteBatch spriteBatch)
        {

        }
            public override void Draw(SpriteBatch spriteBatch)
            {
            dimensions.Width = 16 + (inputText.Length - 1) * 7;
                if (delay > 0)
                    delay--;
            Texture2D GottenTexture = Texture ?? TextureCache.magicPixel;
                KeyboardState keyboard = Keyboard.GetState();
            Main.spriteBatch.Draw(GottenTexture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), Color.Black * alpha);
            DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(2, 2));
            if (keyboard.GetPressedKeys().Length != 0 && delay == 0 && isActive)
                {
                    for (int i = 0; i < keyboard.GetPressedKeys().Length; i++)
                    {
                        bool IsNumber = (keyboard.GetPressedKeys()[i] >= Keys.D0
                                         &&
                                        keyboard.GetPressedKeys()[i] <= Keys.D9);
                        Keys firstKey = keyboard.GetPressedKeys()[0];
                        if (firstKey != Keys.Enter && inputText.Length < 15)
                        {
                            if (IsNumber)
                                inputText += (firstKey == Keys.LeftShift ? keyboard.GetPressedKeys()[i].ToString().ToUpper() : keyboard.GetPressedKeys()[i].ToString().ToLower())[1];
                        if (keyboard.GetPressedKeys()[i] == Keys.OemPeriod)
                            inputText += ".";
                        if (keyboard.GetPressedKeys()[i] == Keys.OemMinus)
                            inputText += "-";
                    }
                        if (firstKey == Keys.Back && delay == 0)
                        {
                            if (inputText.Length > 0)
                                inputText = inputText.Remove(inputText.Length - 1);
                        }
                    }
                    delay = 10;

                }
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
            public override void Draw(SpriteBatch spriteBatch)
            {
                if (delay > 0)
                    delay--;
                KeyboardState keyboard = Keyboard.GetState();
                Main.spriteBatch.Draw(TextureCache.WorldSavePanel, new Rectangle(dimensions.X - 10, dimensions.Y - 20, 180, 90), Color.White * alpha);
                Main.spriteBatch.Draw(TextureCache.Textbox, dimensions, Color.White * alpha);
                Main.spriteBatch.Draw(TextureCache.SaveTex, new Rectangle(dimensions.X + dimensions.Width / 4, dimensions.Y - 20 + (int)(Math.Sin(Main.gameTime.TotalGameTime.TotalMilliseconds / 120f) * 3), dimensions.Width / 2, dimensions.Height / 2), Color.White * alpha);
                if (Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
                {
                    alpha += (1 - alpha) / 16f;
                    dimensions.X += (int)(Main.ScreenSize.X - 150 - dimensions.X) / 16;
                    if (keyboard.IsKeyDown(Keys.Enter) && inputText != "")
                    {
                        SaveCurrentWorldAs(inputText);
                        inputText = "";
                    }
                    else
                    {
                        DrawMethods.DrawTextToLeft(inputText, Color.White * alpha, dimensions.Location.ToVector2() + new Vector2(26, 7));
                    }

                    if (keyboard.GetPressedKeys().Length != 0 && delay == 0)
                    {
                        for (int i = 0; i < keyboard.GetPressedKeys().Length; i++)
                        {
                            bool CanType = keyboard.GetPressedKeys()[i] != Keys.LeftShift && keyboard.GetPressedKeys()[i] != Keys.OemSemicolon && keyboard.GetPressedKeys()[i] != Keys.Back &&
                                        keyboard.GetPressedKeys()[i] != Keys.LeftAlt &&
                                        keyboard.GetPressedKeys()[i] != Keys.RightAlt &&
                                        (keyboard.GetPressedKeys()[i] < Keys.D0
                                         ||
                                        keyboard.GetPressedKeys()[i] > Keys.D9);
                            bool IsNumber = (keyboard.GetPressedKeys()[i] >= Keys.D0
                                             &&
                                            keyboard.GetPressedKeys()[i] <= Keys.D9);
                            Keys firstKey = keyboard.GetPressedKeys()[0];
                            if (firstKey != Keys.Enter && inputText.Length < 15)
                            {
                                if (CanType)
                                    inputText += firstKey == Keys.LeftShift ? keyboard.GetPressedKeys()[i].ToString().ToUpper() : keyboard.GetPressedKeys()[i].ToString().ToLower();
                                if (IsNumber)
                                    inputText += (firstKey == Keys.LeftShift ? keyboard.GetPressedKeys()[i].ToString().ToUpper() : keyboard.GetPressedKeys()[i].ToString().ToLower())[1];
                            }
                            if (firstKey == Keys.Back && delay == 0)
                            {
                                if (inputText.Length > 0)
                                    inputText = inputText.Remove(inputText.Length - 1);
                            }
                        }
                        delay = 10;
                    }
                }
                else
                {
                    inputText = "";
                    alpha -= alpha / 16f;
                    dimensions.X += (int)(Main.ScreenSize.X - dimensions.X) / 16;
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
    }

