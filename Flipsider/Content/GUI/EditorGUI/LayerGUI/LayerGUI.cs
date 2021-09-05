using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Flipsider.TileManager;

namespace Flipsider.GUI
{
    internal class PlayerLayerTextBox : NumberBoxScalable
    {
        private float SetAlpha;
        protected override void CustomDraw(SpriteBatch spriteBatch)
        {
            if (Main.Editor.IsActive)
            {
                Alpha = Alpha.ReciprocateTo(1f);
            }
            else
            {
                Alpha = Alpha.ReciprocateTo(0f);
            }

            if (Main.Editor.IsActive)
            {
                string text = "Set Player Layer:";
                Utils.DrawTextToLeft(text, Color.Black, dimensions.Location.ToVector2() - new Vector2(Main.font.MeasureString(text).X, 0));
            }
            else
            {
            }
            KeyboardState keyboard = Keyboard.GetState();
            if (inputText != "" && !inputText.EndsWith('.') && float.TryParse(inputText, out _))
            {
                Utils.DrawTextToLeft("Player Layer is set to " + Number, Color.Black * (float)Math.Sin(SetAlpha * 3.14f / 60f), dimensions.Location.ToVector2() + new Vector2(20, 2));
                if (hasCursor && keyboard.IsKeyDown(Keys.Enter))
                {
                    SetAlpha = 60;
                    Main.player.Layer = (int)Number;
                }
            }
        }
        protected override void PostUpdate()
        {
            if (hasCursor)
            {
                Main.Editor.CanSwitch = false;
            }
            if (SetAlpha > 0)
            { SetAlpha--; }

        }
        protected override void OnHover()
        {
            CanPlace = false;
        }
    }
    
}
