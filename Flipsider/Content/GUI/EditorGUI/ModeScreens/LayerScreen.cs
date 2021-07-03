using Flipsider.Engine.Input;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class LayerScreen : ModeScreen
    {
        private LayerGUIPanel[]? layerGUIPanels;
        LayerGUIAdd? Add;

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

                Logger.NewText(layerGUIPanels[a].Layer);
                Logger.NewText(layerGUIPanels[b].Layer);

                layerGUIPanels[a].Layer = l2;
                layerGUIPanels[b].Layer = l1;

                Logger.NewText(layerGUIPanels[a].Layer);
                Logger.NewText(layerGUIPanels[b].Layer);
            }

        }

        protected override void OnLoad() => Recalculate();

        public void Recalculate()
        {
            layerGUIPanels = new LayerGUIPanel[Main.CurrentWorld.layerHandler.GetLayerCount()];
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
            Add?.Update();

            if (layerGUIPanels != null)
            {
                for (int i = 0; i < layerGUIPanels.Length; i++)
                {
                    layerGUIPanels[i].Update();
                }
            }
        }
        public override void DrawToBottomPanel(SpriteBatch sb)
        {
            Add?.Draw(sb);
            Utils.DrawBoxFill(new Rectangle(0, 0, Main.renderer.Destination.Width, 10), new Color(50, 50, 50), .1f);

            if (layerGUIPanels != null)
            {
                for (int i = 0; i < layerGUIPanels.Length; i++)
                {
                    layerGUIPanels[i].Draw(sb);
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

        public static bool IsGrabbing;
        public Vector2 Correction;
        private int MouseLayer;
        public LayerGUIPanel(int Layer, ScrollPanel p, LayerScreen s) : base(p)
        {
            this.Layer = Layer;
            Hide = new LayerGUIHide(this, p);
            ScreenParent = s;
            NumberBox = new NumberBoxScalableScroll();
            NumberBox.inputText = Main.layerHandler.GetLayer(Layer).parallax.ToString();
            NumberBox.Color = new Color(50, 50, 50);
            NumberBox.MaxChars = 5;
            NumberBox.OnEnterEvent = (float val) =>
            {
                Main.layerHandler.SetLayerParallax(Layer, val);
            };

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            RelativeDimensions = new Rectangle(0, Layer * 20 + 10, 128, 20);

            Utils.DrawBoxFill(RelativeDimensions, new Color(10, 10, 10), 1f);
            Utils.DrawRectangle(RelativeDimensions, Color.White * Alpha);
            Utils.DrawTextToLeft(Main.layerHandler.GetLayer(Layer).Name ?? " ", new Color(40, 40, 40), new Vector2(25, Layer * 20 + 13));

            if (NumberBox != null)
            {
                NumberBox.RelativeDimensions.Location = new Point(75, Layer * 20 + 14);
                NumberBox.RelativeDimensions.Height = 10;
                NumberBox?.Draw(spriteBatch);
            }

            Hide.Draw(spriteBatch);
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


