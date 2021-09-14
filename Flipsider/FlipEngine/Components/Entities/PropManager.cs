using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static FlipEngine.PropManager;

namespace FlipEngine
{
    public partial class PropManager
    {
        public List<Prop> props = new List<Prop>();

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
            if (TileManager.UselessCanPlaceBool || FlipGame.isLoading)
            {
                props.Add(prop);
            }
            TileManager.UselessCanPlaceBool = true;
            return prop;
        }
        public void AddProp(string PropType, Vector2 position)
        {
            if (PropType != null && PropType != "")
            {
                Texture2D? tex = PropTypes[PropType ?? ""];
                if (tex != null)
                {
                    int alteredRes = FlipGame.World.TileRes / 4;
                    Vector2 Bounds = tex.Bounds.Size.ToVector2();
                    Vector2 posDis = -Bounds / 2 + new Vector2(alteredRes / 2);
                    Prop prop = new Prop(PropType ?? "", position + posDis, LayerHandler.CurrentLayer, true);
                    props.Add(prop);
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
