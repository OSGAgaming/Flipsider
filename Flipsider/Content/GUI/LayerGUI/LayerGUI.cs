using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class LayerGUI : UIScreen
    {
        public int chosen = -1;
        public int LayerCount => Main.CurrentWorld.layerHandler.GetLayerCount();
        protected override void OnLoad()
        {

            for (int i = 0; i < Main.CurrentWorld.layerHandler.GetLayerCount(); i++)
            {
                LayerGUIElement textBox = new LayerGUIElement(i)
                {
                    dimensions = new Rectangle((int)Main.ActualScreenSize.X - 150, 40 + i * 20, 150, 10)
                };
                elements.Add(textBox);
                LayerGUIElementHide Hide = new LayerGUIElementHide(i)
                {
                    dimensions = new Rectangle((int)Main.ActualScreenSize.X - 100, 40 + i * 20, 32, 32)
                };
                elements.Add(Hide);
                LayerElementSwitch Switch = new LayerElementSwitch(i)
                {
                    dimensions = new Rectangle((int)Main.ActualScreenSize.X - 100, 40 + i * 20, 30, 16)
                };
                elements.Add(Switch);
                LayerTextBox Box = new LayerTextBox(i)
                {
                    dimensions = new Rectangle((int)Main.ActualScreenSize.X - 50, 40 + i * 20, 16, 16)
                };
                elements.Add(Box);
            }
            LayerAddition LayerAddition = new LayerAddition(this)
            {
                dimensions = new Rectangle(20, 10, 16, 16)
            };
            elements.Add(LayerAddition);
            LayerAddition LayerAddition2 = new LayerAddition(this)
            {
                dimensions = new Rectangle(40, 10, 16, 16),
                isAdding = true
            };
            elements.Add(LayerAddition2);
            PlayerLayerTextBox PLTB = new PlayerLayerTextBox
            {
                dimensions = new Rectangle(200, 10, 16, 16)
            };
            elements.Add(PLTB);
        }
        private int Buffer;
        protected override void OnUpdate()
        {
            if(Buffer != LayerCount)
            {
                elements.Clear();
                for (int i = 0; i < Main.CurrentWorld.layerHandler.GetLayerCount(); i++)
                {
                    LayerGUIElement textBox = new LayerGUIElement(i)
                    {
                        dimensions = new Rectangle( - 150, 40 + i * 20, 150, 10)
                    };
                    elements.Add(textBox);
                    LayerGUIElementHide Hide = new LayerGUIElementHide(i)
                    {
                        dimensions = new Rectangle(- 100, 40 + i * 20, 32, 32)
                    };
                    elements.Add(Hide);
                    LayerElementSwitch Switch = new LayerElementSwitch(i)
                    {
                        dimensions = new Rectangle(- 100, 40 + i * 20, 30, 16)
                    };
                    elements.Add(Switch);
                    LayerTextBox Box = new LayerTextBox(i)
                    {
                        dimensions = new Rectangle( - 50, 40 + i * 20, 16, 16)
                    };
                    elements.Add(Box);
                }
                LayerAddition LayerAddition = new LayerAddition(this)
                {
                    dimensions = new Rectangle(20, 10, 16, 16)
                };
                elements.Add(LayerAddition);
                LayerAddition LayerAddition2 = new LayerAddition(this)
                {
                    dimensions = new Rectangle(40, 10, 16, 16),
                    isAdding = true
                };
                elements.Add(LayerAddition2);
                PlayerLayerTextBox PLTB = new PlayerLayerTextBox
                {
                    dimensions = new Rectangle(200, 10, 16, 16)
                };
                elements.Add(PLTB);
            }
            Buffer = LayerCount;
        }
        protected override void OnDraw()
        {

        }
    }
    internal class PlayerLayerTextBox : NumberBoxScalable
    {
        private float SetAlpha;
        protected override void CustomDraw(SpriteBatch spriteBatch)
        {
            if (Main.Editor.IsActive)
            {
                alpha = 1;
                string text = "Set Player Layer:";
                Utils.DrawTextToLeft(text, Color.Black, dimensions.Location.ToVector2() - new Vector2(Main.font.MeasureString(text).X, 0));
            }
            else
            {
                alpha = 0;
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
    internal class LayerTextBox : NumberBoxScalable
    {
        private int Layer;
        private float SetAlpha;
        private float buffer;
        public LayerTextBox(int Layer)
        {
            this.Layer = Layer;
        }
        protected override void CustomDraw(SpriteBatch spriteBatch)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (inputText != "" && !inputText.EndsWith('.') && float.TryParse(inputText, out buffer))
            {
                Utils.DrawTextToLeft("Paralax in Layer " + Layer + " is set to: " + Number, Color.Black * (float)Math.Sin(SetAlpha * 3.14f / 60f), dimensions.Location.ToVector2() + new Vector2(20, 2));
                if (hasCursor && keyboard.IsKeyDown(Keys.Enter))
                {
                    SetAlpha = 60;
                    Main.layerHandler.SetLayerParallax(Layer, Number);
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
        protected override void OnHover()
        {
            CanPlace = false;
        }
    }
    internal class LayerGUIElement : UIElement
    {
        private int Layer;
        private float lerp;

        public LayerGUIElement(int Layer)
        {
            this.Layer = Layer;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawTextToLeft("Layer " + (Layer + 1), Color.Lerp(Color.Black, Color.BlanchedAlmond, lerp), dimensions.Location.ToVector2());
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
            CanPlace = false;
            lerp += (1 - lerp) / 16f;
        }

        protected override void NotOnHover()
        {
            lerp += (0 - lerp) / 16f;
        }
    }
    internal class LayerAddition : UIElement
    {
        private float lerp;
        public bool isAdding;
        public UIScreen parent;
        public LayerAddition(UIScreen parent)
        {
            this.parent = parent;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAdding)
            {
                spriteBatch.Draw(TextureCache.AddLayer, dimensions, new Rectangle(0, 0, 32, 32), Color.White * lerp);
            }
            else
            {
                spriteBatch.Draw(TextureCache.RemoveLayer, dimensions, new Rectangle(0, 0, 32, 32), Color.White * lerp);
            }
        }
        protected override void OnUpdate()
        {
            if (Main.Editor.IsActive)
            {
                lerp = lerp.ReciprocateTo(1);
            }
            else
            {
                lerp = lerp.ReciprocateTo(0);
            }
        }
        protected override void OnLeftClick()
        {
            if (isAdding)
            {
                Main.layerHandler.AddLayer();
            }
        }
        protected override void OnHover()
        {
            CanPlace = false;
        }

        protected override void NotOnHover()
        {

        }
    }
    internal class LayerGUIElementHide : UIElement
    {
        private int Layer;
        private float lerp;
        private bool Hide;
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
    internal class LayerElementSwitch : UIElement
    {
        private int Layer;
        private float lerp;
        private bool isSelected;
        public LayerElementSwitch(int Layer)
        {
            this.Layer = Layer;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.SwitchLayer, dimensions.Location.ToVector2(), new Rectangle(0, 0, 30, 16), isSelected ? Color.Yellow * 0.5f : Color.White);
        }
        protected override void OnUpdate()
        {
            if (LayerHandler.CurrentLayer == Layer)
            {
                dimensions.X += (200 - dimensions.X) / 16;
            }
            else
            {
                dimensions.X += (150 - dimensions.X) / 16;
            }
            if (Main.Editor.IsActive)
            {
                dimensions.Y += (40 + Layer * 20 - dimensions.Y) / 16;
            }
            else
            {
                dimensions.Y += (-100 - dimensions.Y) / 16;
            }
            if (LayerHandler.LayerCache[1] == -1 && LayerHandler.LayerCache[0] == -1)
            {
                isSelected = false;
            }
        }
        protected override void OnLeftClick()
        {
            CanPlace = false;
            if (LayerHandler.LayerCache[0] == -1)
            {
                LayerHandler.LayerCache[0] = Layer;
                isSelected = true;
            }
            else
            {
                LayerHandler.LayerCache[1] = Layer;
                Main.layerHandler.SwitchLayers(LayerHandler.LayerCache[0], LayerHandler.LayerCache[1]);
                LayerHandler.LayerCache[0] = -1;
                LayerHandler.LayerCache[1] = -1;
            }
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
