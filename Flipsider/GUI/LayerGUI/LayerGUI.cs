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
    internal class LayerGUI : UIScreen
    {
        public int chosen = -1;
        protected override void OnLoad()
        {
            for (int i = 0; i < Main.renderer.layerHandler.GetLayerCount(); i++)
            {
                LayerGUIElement textBox = new LayerGUIElement(i);
                textBox.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 150, 40 + i * 20, 150, 10);
                elements.Add(textBox);
                LayerGUIElementHide Hide = new LayerGUIElementHide(i);
                Hide.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 100, 40 + i * 20, 32, 64);
                elements.Add(Hide);
                LayerTextBox Box = new LayerTextBox(i);
                Box.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 50, 40 + i * 20, 16, 16);
                elements.Add(Box);
            }

        }

        protected override void OnUpdate()
        {
        }
        protected override void OnDraw()
        {

        }
    }
    internal class LayerTextBox : NumberBox
    {
        private int Layer;
        float lerp;
        float SetAlpha;
        float buffer;
        public LayerTextBox(int Layer)
        {
            this.Layer = Layer;
        }
        protected override void CustomDraw(SpriteBatch spriteBatch)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (inputText != "" && !inputText.EndsWith('.') && float.TryParse(inputText, out buffer))
            {
                DrawMethods.DrawTextToLeft("Paralax in Layer " + Layer + " is set to: " + Number, Color.Black * (float)Math.Sin(SetAlpha * 3.14f / 60f), dimensions.Location.ToVector2() + new Vector2(20, 2));
                if (isActive && keyboard.IsKeyDown(Keys.Enter))
                {
                    SetAlpha = 60;
                    Main.layerHandler.SetLayerParalax(Layer, Number);
                }
            }
        }
        protected override void OnUpdate()
        {
            if (isActive)
            {
                Main.Editor.CanSwitch = false;
            }
            if (SetAlpha > 0)
            { SetAlpha--; }

            if (LayerHandler.CurrentLayer == Layer)
            {
                dimensions.X += (150 - dimensions.X) / 16;
            }
            else
            {
                dimensions.X += (-50 - dimensions.X) / 16;
            }
            if (Main.Editor.IsActive)
            {
                dimensions.Y += (40 + Layer * 20 - dimensions.Y) / 16;
            }
            else
            {
                dimensions.Y += (-100 - dimensions.Y) / 16;
            }
        }
    }
    internal class LayerGUIElement : UIElement
    {
        private int Layer;
        float lerp;

        public LayerGUIElement(int Layer)
        {
            this.Layer = Layer;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawMethods.DrawTextToLeft("Layer " + (Layer + 1), Color.Lerp(Color.Black, Color.BlanchedAlmond, lerp), dimensions.Location.ToVector2());
        }
        protected override void OnUpdate()
        {
            if (LayerHandler.CurrentLayer == Layer)
            {
                dimensions.X += (50 - dimensions.X) / 16;
            }
            else
            {
                dimensions.X += (-dimensions.X) / 16;
            }
            if (Main.Editor.IsActive)
            {
                dimensions.Y += (40 + Layer * 20 - dimensions.Y) / 16;
            }
            else
            {
                dimensions.Y += (-100 - dimensions.Y) / 16;
            }
        }
        protected override void OnLeftClick()
        {
            CanPlace = false;
            LayerHandler.CurrentLayer = Layer;
        }
        protected override void OnHover()
        {
            lerp += (1 - lerp) / 16f;
        }

        protected override void NotOnHover()
        {
            lerp += (0 - lerp) / 16f;
        }
    }
    internal class LayerGUIElementHide : UIElement
    {
        private int Layer;
        float lerp;
        bool Hide;
        public LayerGUIElementHide(int Layer)
        {
            this.Layer = Layer;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.LayerHide, dimensions.Location.ToVector2(), new Rectangle(0, Hide ? 32 : 0, 32, 32), Color.White);
        }
        protected override void OnUpdate()
        {
            if (LayerHandler.CurrentLayer == Layer)
            {
                dimensions.X += (100 - dimensions.X) / 16;
            }
            else
            {
                dimensions.X += (-50 - dimensions.X) / 16;
            }
            if (Main.Editor.IsActive)
            {
                dimensions.Y += (32 + Layer * 20 - dimensions.Y) / 16;
            }
            else
            {
                dimensions.Y += (-100 - dimensions.Y) / 16;
            }
        }
        protected override void OnLeftClick()
        {
            CanPlace = false;
            Main.layerHandler.SwitchLayerVisibility(Layer);
            Hide = !Hide;
        }
        protected override void OnHover()
        {
            lerp += (1 - lerp) / 16f;
        }

        protected override void NotOnHover()
        {
            lerp += (0 - lerp) / 16f;
        }
    }
}
