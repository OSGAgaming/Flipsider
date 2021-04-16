
using Flipsider.Engine.Interfaces;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    Prop prop = new Prop(PropType ?? "", position + posDis, LayerHandler.CurrentLayer, true);
                    props.Add(prop);
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
                    Rectangle altFrame = PropTypes[Main.Editor.CurrentProp].Bounds;
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
    }
}
