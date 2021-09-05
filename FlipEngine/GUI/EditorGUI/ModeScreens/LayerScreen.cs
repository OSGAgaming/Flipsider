

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class LayerScreen : ModeScreen
    {
        private LayerGUIPanel[]? layerGUIPanels;
        private LayerGUIAdd? Add;
        public static LayerScreen? Instance;

        public override int PreviewHeight
        {
            get
            {
                if (layerGUIPanels != null)
                    return (layerGUIPanels.Length) * 20 + 10;
                else return 0;
            }
        }

        public void Switch(int l1, int l2)
        {
            int a = 0;
            int b = 0;

            if (layerGUIPanels != null)
            {
                for (int i = 0; i < layerGUIPanels.Length; i++)
                {
                    if (layerGUIPanels[i].Layer == l1) a = i;
                    if (layerGUIPanels[i].Layer == l2) b = i;
                }

                layerGUIPanels[a].Layer = l2;
                layerGUIPanels[b].Layer = l1;
            }

        }

        protected override void OnLoad()
        {
            Recalculate();
            Instance = this;
        }

        public void Recalculate()
        {
            layerGUIPanels = new LayerGUIPanel[Main.World.layerHandler.GetLayerCount()];
            Add = new LayerGUIAdd(this, EditorModeGUI.BottomPreview);

            for (int i = 0; i < layerGUIPanels.Length; i++)
            {
                layerGUIPanels[i] = new LayerGUIPanel(i, EditorModeGUI.BottomPreview, this);
            }
        }

        internal override void OnDrawToScreenDirect()
        {
            if (layerGUIPanels != null)
            {
                for (int i = 0; i < layerGUIPanels.Length; i++)
                {
                    layerGUIPanels[i].DrawOnScreenDirect(Main.spriteBatch);
                }
            }
        }

        public override void CustomUpdate()
        {
            if (EditorModeGUI.mode != Mode.Cutscene)
            {
                Add?.Update();
                if (layerGUIPanels != null)
                {
                    for (int i = 0; i < layerGUIPanels.Length; i++)
                    {
                        layerGUIPanels[i].Update();
                    }
                }
            }
        }
        public override void DrawToBottomPanel(SpriteBatch sb)
        {
            if(EditorModeGUI.mode != Mode.Cutscene)
            {
                Add?.Draw(sb);

                Utils.DrawBoxFill(new Rectangle(0, 0, Main.renderer.Destination.Width, 10), new Color(50, 50, 50), .1f);
                Utils.DrawTextToLeft("Player Layer", Color.White, new Vector2(320, 7));

                Utils.DrawTextToLeft("Plx", Color.White, new Vector2(77, 0), 0, 0.4f);
                Utils.DrawTextToLeft("Sat", Color.White, new Vector2(190, 0), 0, 0.4f);

                if (layerGUIPanels != null)
                {
                    for (int i = 0; i < layerGUIPanels.Length; i++)
                    {
                        layerGUIPanels[i].Draw(sb);
                    }
                }
            }
        }
    }
    internal class LayerGUIPanel : PreviewElement
    {
        public int Layer;
        LayerGUIHide Hide;
        LayerScreen ScreenParent;
        public float Alpha;
        public bool isBeingPicked;
        NumberBoxScalableScroll? NumberBox;
        NumberBoxScalableScroll? SaturationBox;
        RGBPicker? RGB;

        public static bool IsGrabbing;
        public Vector2 Correction;
        private int MouseLayer;
        public LayerGUIPanel(int Layer, ScrollPanel p, LayerScreen s) : base(p)
        {
            this.Layer = Layer;
            Hide = new LayerGUIHide(this, p);
            RGB = new RGBPicker(this, p);

            ScreenParent = s;

            NumberBox = new NumberBoxScalableScroll();
            NumberBox.inputText = Main.layerHandler.GetLayer(Layer).parallax.ToString();
            NumberBox.Color = new Color(120, 120, 120);
            NumberBox.MaxChars = 5;
            NumberBox.OnEnterEvent = (float val) =>
            {
                Main.layerHandler.SetLayerParallax(Layer, val);
            };

            SaturationBox = new NumberBoxScalableScroll();
            SaturationBox.inputText = Main.layerHandler.GetLayer(Layer).SaturationValue.ToString();
            SaturationBox.Color = new Color(120, 120, 120);
            SaturationBox.MaxChars = 5;
            SaturationBox.OnEnterEvent = (float val) =>
            {
                Main.layerHandler.GetLayer(Layer).SaturationValue = val;
            };
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(0, Layer * 20 + 10, 220, 20);

            Utils.DrawBoxFill(RelativeDimensions, new Color(10, 10, 10), 1f);
            Utils.DrawRectangle(RelativeDimensions, Color.White * Alpha);

            if(LayerHandler.CurrentLayer == Layer)
            {
                Utils.DrawRectangle(RelativeDimensions, Color.Yellow * Alpha, 2);

                if (PreviewPanel != null)
                {
                    if (GameInput.Instance["Delete"].IsJustPressed() && PreviewPanel.Active)
                    {
                        Main.layerHandler.RemoveLayer(Layer);
                        LayerScreen.Instance?.Recalculate();
                    }
                }
            }

            Utils.DrawTextToLeft(Main.layerHandler.GetLayer(Layer).Name ?? " ", new Color(40, 40, 40), new Vector2(25, Layer * 20 + 13));

            if (NumberBox != null)
            {
                NumberBox.RelativeDimensions.Location = new Point(75, Layer * 20 + 15);
                NumberBox?.Draw(spriteBatch);
            }

            if (SaturationBox != null)
            {
                SaturationBox.RelativeDimensions.Location = new Point(185, Layer * 20 + 15);
                SaturationBox?.Draw(spriteBatch);
            }

            Hide.Draw(spriteBatch);
            RGB?.Draw(spriteBatch);
        }

        public override void DrawOnScreenDirect(SpriteBatch spriteBatch)
        {
            if (isBeingPicked)
            {
                Utils.DrawBoxFill(new Rectangle(Mouse.GetState().Position.Add(Correction), RelativeDimensions.Size), new Color(90, 90, 90) * 0.2f, .1f);
                Utils.DrawTextToLeft(Main.layerHandler.GetLayer(Layer).Name ?? " ", new Color(40, 40, 40) * 0.2f, Mouse.GetState().Position.ToVector2());
                if (PreviewPanel != null)
                {
                    Utils.DrawRectangle(new Rectangle(0, MouseLayer * 20 + 10 - (int)PreviewPanel.ScrollValue, 128, 20).AddPos(PreviewPanel.dimensions.Location), Color.Blue * Alpha);
                }
            }
        }

        protected override void OnLeftClick()
        {
            LayerHandler.CurrentLayer = Layer;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsGrabbing)
            {
                isBeingPicked = true;
                IsGrabbing = true;

                Correction = dimensions.Location.ToVector2() - Mouse.GetState().Position.ToVector2();
            }
        }

        protected override void CustomUpdate()
        {
            Hide.Update();
            NumberBox?.Update();
            RGB?.Update();
            SaturationBox?.Update();

            if (PreviewPanel != null)
            {
                MouseLayer = ((Mouse.GetState().Y - PreviewPanel.dimensions.Location.Y) - 10 + (int)PreviewPanel.ScrollValue) / 20;

                if (Mouse.GetState().LeftButton != ButtonState.Pressed && isBeingPicked)
                {
                    IsGrabbing = false;
                    isBeingPicked = false;

                    if (MouseLayer >= 0 && MouseLayer < Main.layerHandler.GetLayerCount() && MouseLayer != Layer)
                    {
                        if (PreviewPanel != null)
                        {
                            Main.layerHandler.SwitchLayers(MouseLayer, Layer);
                            Logger.NewText("Layer " + Layer + " and Layer " + MouseLayer + " have been switched");

                            ScreenParent.Switch(Layer, MouseLayer);
                        }
                    }
                }
            }

            if (Layer == LayerHandler.CurrentLayer)
            {
                Alpha = Alpha.ReciprocateTo(1);
            }
            else
            {
                Alpha = Alpha.ReciprocateTo(0);
            }
        }
    }
    internal class LayerGUIHide : PreviewElement
    {
        public LayerGUIPanel Panel;
        public LayerGUIHide(LayerGUIPanel Parent, ScrollPanel p) : base(p)
        {
            Panel = Parent;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(Panel.RelativeDimensions.Location, new Point(20, 20));
            PreviewPanel = EditorModeGUI.BottomPreview;

            spriteBatch.Draw(TextureCache.LayerHide, RelativeDimensions, new Rectangle(0, !Main.layerHandler.GetLayer(Panel.Layer).visible ? 32 : 0, 32, 32), Color.White);
        }

        protected override void OnLeftClick()
        {
            Main.layerHandler.SwitchLayerVisibility(Panel.Layer);
        }
    }

    internal class RGBPicker : PreviewElement
    {
        public LayerGUIPanel Panel;
        public NumberBoxScalableScroll? R;
        public NumberBoxScalableScroll? G;
        public NumberBoxScalableScroll? B;

        public RGBPicker(LayerGUIPanel Parent, ScrollPanel p) : base(p)
        {
            Panel = Parent;
            Layer layer = Main.layerHandler.GetLayer(Panel.Layer);
            Vector4 c = layer.ColorModification;

            R = new NumberBoxScalableScroll
            {
                inputText = c.X.ToString(),
                Color = new Color(120, 120, 120),
                MaxChars = 3,
                BorderWidth = 1,
                BorderColor = Color.Red,
                OnEnterEvent = (float val) =>
                {
                    layer.ColorModification.X = val;
                }
            };

            G = new NumberBoxScalableScroll
            {
                inputText = c.Y.ToString(),
                Color = new Color(120, 120, 120),
                MaxChars = 3,
                BorderWidth = 1,
                BorderColor = Color.Blue,
                OnEnterEvent = (float val) =>
                {
                    layer.ColorModification.Y = val;
                }
            };

            B = new NumberBoxScalableScroll
            {
                inputText = c.Z.ToString(),
                Color = new Color(120, 120, 120),
                MaxChars = 3,
                BorderWidth = 1,
                BorderColor = Color.Green,
                OnEnterEvent = (float val) =>
                {
                    layer.ColorModification.Z = val;
                }
            };
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(Panel.RelativeDimensions.Location, new Point(20, 20));
            PreviewPanel = EditorModeGUI.BottomPreview;

            if (R != null && G != null && B != null)
            {
                R.RelativeDimensions.Location = Panel.RelativeDimensions.Location.Add(new Vector2(100, 5));
                G.RelativeDimensions.Location = Panel.RelativeDimensions.Location.Add(new Vector2(130, 5));
                B.RelativeDimensions.Location = Panel.RelativeDimensions.Location.Add(new Vector2(160, 5));
            }

            R?.Draw(spriteBatch);
            G?.Draw(spriteBatch);
            B?.Draw(spriteBatch);

            spriteBatch.Draw(TextureCache.LayerHide, RelativeDimensions, new Rectangle(0, !Main.layerHandler.GetLayer(Panel.Layer).visible ? 32 : 0, 32, 32), Color.White);
        }

        protected override void CustomUpdate()
        {
            R?.Update();
            G?.Update();
            B?.Update();
        }
    }

    internal class LayerGUIAdd : PreviewElement
    {
        public LayerScreen Panel;
        public LayerGUIAdd(LayerScreen Parent, ScrollPanel p) : base(p)
        {
            Panel = Parent;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(new Point(0, 0), new Point(10, 10));
            PreviewPanel = EditorModeGUI.BottomPreview;

            spriteBatch.Draw(TextureCache.AddLayer, RelativeDimensions, new Rectangle(0, IsBeingClicked ? 32 : 0, 32, 32), Color.White);
        }

        protected override void OnLeftClick()
        {
            Main.layerHandler.AddLayer();
            Panel.Recalculate();
        }
    }

}


