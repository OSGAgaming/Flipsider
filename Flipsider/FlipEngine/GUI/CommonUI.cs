using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    internal class Box : UIElement
    {
        protected virtual Color color => Color.Black;

        protected float Alpha = 1;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawBoxFill(dimensions.Inf(2, 2), Color.CadetBlue * Alpha);
            Utils.DrawBoxFill(dimensions,color * Alpha);
            PostDraw(spriteBatch); 
        }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
    }

    internal class Button : UIElement
    {
        public Texture2D? Texture;
        public Rectangle source;
        public Action? OnClick;

        public string? OptionalText = " ";

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                if (source == default) source = Texture.Bounds;

                spriteBatch.Draw(Texture, dimensions, source, Color.White);
            }

            if(OptionalText != null) Utils.DrawText(OptionalText, Color.White, dimensions.Center.ToVector2());
        }

        protected override void OnLeftClick()
        {
            OnClick?.Invoke();
        }
    }
    internal class Text : Box
    {
        protected override Color color => Color.White;
        public string inputText = "Test";
        public bool hasCursor;
        protected KeyboardState oldKeyboardState = Keyboard.GetState();
        protected KeyboardState currentKeyboardState = Keyboard.GetState();
        protected float alpha = 1;
        public void UpdateInput()
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
        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = FlipE.font.MeasureString(inputText);
            int disp = dimensions.Height / 2 - (int)FS.Y/2;
            Utils.DrawTextToLeft(inputText, Color.Black * alpha, dimensions.Location.ToVector2() + new Vector2(0, disp));

            if (hasCursor)
            {
                Point pos = new Point(dimensions.Location.X + (int)FS.X + 1, dimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                spriteBatch.Draw(FlipTextureCache.magicPixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * alpha);
            }
            CustomDraw(spriteBatch);
        }
        protected override void OnUpdate()
        {
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
        protected virtual void CustomDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostUpdate() { }
        protected override void OnLeftClick()
        =>
            hasCursor = true;
        protected override void OnLeftClickAway()
        =>
            hasCursor = false;
    }
    internal class NumberBox : Box
    {
        protected override Color color => Color.White;
        public string inputText = "";
        public bool hasCursor;
        public int MaxChars = 10;
        public float Number => float.Parse(inputText);
        protected KeyboardState oldKeyboardState = Keyboard.GetState();
        protected KeyboardState currentKeyboardState = Keyboard.GetState();
        public void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();
            if (hasCursor)
            {
                foreach (Keys key in pressedKeys)
                {
                    bool IsNumber = (key >= Keys.D0
                                             &&
                                            key <= Keys.D9 || key == Keys.OemPeriod || key == Keys.OemMinus);
                    if (oldKeyboardState.IsKeyUp(key) && (IsNumber && !(inputText.Contains(".") && key == Keys.OemPeriod) && inputText.Length <= MaxChars || key == Keys.Back))
                    {
                        KeyboardInput.Instance?.InputKey(key, ref inputText);
                    }
                }
            }
        }
        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = FlipE.font.MeasureString(inputText);
            int disp = dimensions.Height / 2 - (int)FS.Y / 2;
            Utils.DrawTextToLeft(inputText, Color.Black * Alpha, dimensions.Location.ToVector2() + new Vector2(0, disp));
            if (hasCursor)
            {
                Point pos = new Point(dimensions.Location.X + (int)FS.X + 3, dimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                spriteBatch.Draw(FlipTextureCache.magicPixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * Alpha);
            }
            CustomDraw(spriteBatch);
        }
        protected virtual void CustomDraw(SpriteBatch spriteBatch) { }
        protected override void OnUpdate()
        {
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
        protected virtual void PostUpdate() { }
        protected override void OnLeftClick()
        =>
            hasCursor = true;
        protected override void OnLeftClickAway()
        =>
            hasCursor = false;
    }
    internal class NumberBoxScalable : NumberBox
    {
        protected override void OnUpdate()
        {
            dimensions.Width = (int)FlipE.font.MeasureString(inputText).X + 10;
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
    }
    internal class TextBoxScalable : Text
    {
        protected override void OnUpdate()
        {
            dimensions.Width = (int)FlipE.font.MeasureString(inputText).X + 10;
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
    }
    internal class ButtonScroll : Button
    {
        public Rectangle RelativeDimensions;

        public ScrollPanel? ScrollParent { get; set; }

        protected override void OnUpdate()
        {
            if (ScrollParent != null)
            {
                Point p = ScrollParent.dimensions.Location;
                Point Position = new Point(p.X + RelativeDimensions.X, p.Y + RelativeDimensions.Y - (int)ScrollParent.ScrollValue);

                dimensions = new Rectangle(Position, RelativeDimensions.Size);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                if (source == default) source = Texture.Bounds;

                spriteBatch.Draw(Texture, RelativeDimensions, source, Color.White);
            }

            if (OptionalText != null) Utils.DrawText(OptionalText, Color.White, RelativeDimensions.Center.ToVector2());
        }
    }

    internal class TextBoxScalableScroll : Text
    {
        public Rectangle RelativeDimensions;

        public ScrollPanel? Parent { get; set; }

        public Action<string>? OnEnterEvent;

        public int BorderWidth = 1;

        public TextBoxScalableScroll(ScrollPanel Parent)
        {
            this.Parent = Parent;
        }

        protected override void OnUpdate()
        {
            RelativeDimensions.Width = (int)FlipE.font.MeasureString(inputText).X + 10;

            if (hasCursor)
            {
                UpdateInput();

                if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    hasCursor = false;
                    OnEnterEvent?.Invoke(inputText);
                }
            }
            PostUpdate();

            if (Parent != null)
            {
                Point p = Parent.dimensions.Location;
                Point Position = new Point(p.X + RelativeDimensions.X, p.Y + RelativeDimensions.Y - (int)Parent.ScrollValue);

                dimensions = new Rectangle(Position, RelativeDimensions.Size);
            }
        }

        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = FlipE.font.MeasureString(inputText);
            int disp = RelativeDimensions.Height / 2 - (int)FS.Y / 2;
            Utils.DrawTextToLeft(inputText, Color.Black * Alpha, RelativeDimensions.Location.ToVector2() + new Vector2(0, disp));
            if (hasCursor)
            {
                Point pos = new Point(RelativeDimensions.Location.X + (int)FS.X + 1, RelativeDimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                spriteBatch.Draw(FlipTextureCache.magicPixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * Alpha);
            }
            CustomDraw(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawBoxFill(RelativeDimensions.Inf(BorderWidth, BorderWidth), Color.CadetBlue * Alpha, 1f);
            Utils.DrawBoxFill(RelativeDimensions, Color.White * Alpha, 0.5f);
            PostDraw(spriteBatch);
        }
    }

    internal class NumberBoxScalableScroll : NumberBox
    {
        public Rectangle RelativeDimensions;

        public ScrollPanel? ScrollParent { get; set; } = EditorModeGUI.BottomPreview;

        public Action<float>? OnEnterEvent;

        public int BorderWidth = 0;

        public Color Color = Color.White;
        public Color BorderColor = Color.CadetBlue;

        public NumberBoxScalableScroll()
        {
            RelativeDimensions.Height = 10;

        }

        protected override void OnUpdate()
        {
            RelativeDimensions.Width = (int)FlipE.font.MeasureString(inputText).X + 10;

            if (hasCursor)
            {
                UpdateInput();

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    hasCursor = false;
                    float num;
                    if(float.TryParse(inputText, out num)) OnEnterEvent?.Invoke(num);
                }
            }
            PostUpdate();

            if (ScrollParent != null)
            {
                Point p = ScrollParent.dimensions.Location;
                Point Position = new Point(p.X + RelativeDimensions.X, p.Y + RelativeDimensions.Y - (int)ScrollParent.ScrollValue);

                dimensions = new Rectangle(Position, RelativeDimensions.Size);
            }
        }

        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = FlipE.font.MeasureString(inputText);
            int disp = RelativeDimensions.Height / 2 - (int)FS.Y / 2;
            Utils.DrawTextToLeft(inputText, Color.Black * Alpha, RelativeDimensions.Location.ToVector2() + new Vector2(0, disp));
            if (hasCursor)
            {
                Point pos = new Point(RelativeDimensions.Location.X + (int)FS.X , RelativeDimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                if (inputText == "")
                {
                    size.Y = 10;
                    pos.Y -= 5;
                }

                spriteBatch.Draw(FlipTextureCache.magicPixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * Alpha);
            }
            CustomDraw(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawRectangle(RelativeDimensions.Inf(BorderWidth, BorderWidth), BorderColor * Alpha, BorderWidth);
            Utils.DrawBoxFill(RelativeDimensions, Color * Alpha, 0.5f);
            PostDraw(spriteBatch);
        }
    }
}
