using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{

    class WorldCreationGUI : UIScreen
    {
        public int chosen = -1;
        public WorldCreationGUI()
        {
            Main.UIScreens.Add(this);
            UIStringInput textBox = new UIStringInput();
            textBox.dimensions = new Rectangle((int)Main.ScreenSize.X - 150, 40, 150, 26);
            elements.Add(textBox);
        }

        protected override void OnUpdate()
        {
        }
        protected override void OnDraw()
        {

        }
    }
    class UIStringInput : UIElement
    {
        string inputText = "";
        float alpha = 0f;
        int delay = 0;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (delay > 0)
                delay--;
            KeyboardState keyboard = Keyboard.GetState();
            Main.spriteBatch.Draw(TextureCache.WorldSavePanel, new Rectangle(dimensions.X - 10, dimensions.Y - 20,180, 90), Color.White * alpha);
            Main.spriteBatch.Draw(TextureCache.Textbox, dimensions, Color.White * alpha);
            Main.spriteBatch.Draw(TextureCache.SaveTex, new Rectangle(dimensions.X + dimensions.Width / 4, dimensions.Y - 20 + (int)(Math.Sin(Main.gameTime.TotalGameTime.TotalMilliseconds / 120f) * 3), dimensions.Width / 2, dimensions.Height / 2), Color.White * alpha);
            if (EditorModes.CurrentState == EditorUIState.WorldSaverMode)
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
                    Keys firstKey = keyboard.GetPressedKeys()[0];
                    if (firstKey != Keys.Enter && inputText.Length < 15)
                    {
                        if (firstKey == Keys.LeftShift)
                        {
                            for (int i = 0; i < keyboard.GetPressedKeys().Length; i++)
                            {
                                if (keyboard.GetPressedKeys()[i] != Keys.LeftShift && keyboard.GetPressedKeys()[i] != Keys.OemSemicolon && keyboard.GetPressedKeys()[i] != Keys.Back)
                                    inputText += keyboard.GetPressedKeys()[i].ToString();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < keyboard.GetPressedKeys().Length; i++)
                            {
                                if (keyboard.GetPressedKeys()[i] != Keys.LeftShift && keyboard.GetPressedKeys()[i] != Keys.OemSemicolon && keyboard.GetPressedKeys()[i] != Keys.Back)
                                    inputText += keyboard.GetPressedKeys()[i].ToString().ToLower();
                            }
                        }
                        
                    }
                    
                    if (firstKey == Keys.Back && delay == 0)
                    {
                        if(inputText.Length > 0)
                        inputText = inputText.Remove(inputText.Length - 1);
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
