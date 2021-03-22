
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Flipsider.PropManager;

namespace Flipsider
{
    public partial class PropManager
    {
        public List<Prop> props = new List<Prop>();
        public int Layer { get; set; }

        public static Dictionary<string, Texture2D> PropTypes = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Prop> PropEntites = new Dictionary<string, Prop>();
        public static int AddPropType(string Prop, Texture2D tex)
        {
            PropTypes.Add(Prop, tex);
            PropEntites.Add(Prop, new Prop(Prop));
            return PropTypes.Count - 1;
        }
        public Prop AddProp(Prop prop)
        {
                if (TileManager.UselessCanPlaceBool || Main.isLoading || Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
                {
                    props.Add(prop);
                }
                TileManager.UselessCanPlaceBool = true;
            return prop;
        }
        public void AddProp(string PropType, Vector2 position)
        {
            try
            {
                if (TileManager.UselessCanPlaceBool || Main.isLoading || Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
                {
                    int alteredRes = Main.CurrentWorld.TileRes / 4;
                    Vector2 Bounds = PropTypes[PropType ?? ""].Bounds.Size.ToVector2();
                    Vector2 posDis = -Bounds / 2 + new Vector2(alteredRes / 2);
                    props.Add(new Prop(PropType ?? "", position + posDis,1,-1,0,LayerHandler.CurrentLayer,true));
                }
                TileManager.UselessCanPlaceBool = true;
            }
            catch
            {


            }
        }

        public static void ShowPropCursor()
        {
            if (Main.Editor.CurrentState == EditorUIState.PropEditorMode)
            {
                float sine = Time.SineTime(6);
                int alteredRes = Main.CurrentWorld.TileRes / 4;
                Vector2 tilePoint2 = Main.MouseScreen.ToVector2().Snap(alteredRes);
                if (Main.Editor.CurrentProp != null)
                {
                    Prop prop = PropEntites[Main.Editor.CurrentProp];
                    Rectangle altFrame = prop.alteredFrame;
                    Main.spriteBatch.Draw(PropTypes[Main.Editor.CurrentProp], tilePoint2 + new Vector2(alteredRes / 2), altFrame, Color.White * Math.Abs(sine), 0f, altFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public static void AddPropInteraction(string Prop, PropEntity PE)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop].PE = PE;
            }
        }
        public static void ChangeFrames(string Prop, int frames)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop].noOfFrames = frames;
            }
        }
        public static void ChangeAnimSpeed(string Prop, int speed)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop].animSpeed = speed;
            }
        }
    }
    public partial class PropInteraction : IUpdate
    {
        public PropInteraction(PropManager propManager)
        {
            this.propManager = propManager;
            Main.Updateables.Add(this);
        }

        private PropManager? propManager;
        private bool mousePressedRight = false;

        public void Update()
        {
            for (int i = 0; i < propManager?.props.Count; i++)
            {
                Point size = PropTypes[propManager.props[i].prop].Bounds.Size;
                Rectangle rect = new Rectangle(propManager.props[i].ParallaxedCenter.ToPoint() - new Point(size.X / 2, size.Y / 2), size);
                if (propManager.props[i].isDragging)
                {
                    propManager.props[i].position = Main.MouseScreen.ToVector2() - propManager.props[i].offsetFromMouseWhileDragging;
                    if (Mouse.GetState().RightButton != ButtonState.Pressed)
                    {
                        propManager.props[i].isDragging = false;
                    }
                }
                if (rect.Contains(Main.MouseScreen))
                {
                    if (Main.Editor.StateCheck(EditorUIState.PropEditorMode) && propManager.props[i].Layer == LayerHandler.CurrentLayer)
                    {
                        if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                        {
                            propManager.props[i].Dispose();
                        }
                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            if (!mousePressedRight && !propManager.props[i].isDragging)
                            {
                                propManager.props[i].offsetFromMouseWhileDragging = Main.MouseScreen.ToVector2() - propManager.props[i].position;
                                propManager.props[i].isDragging = true;
                            }
                        }
                    }
                }


            }
            mousePressedRight = Mouse.GetState().RightButton == ButtonState.Pressed;
        }

    }
}
