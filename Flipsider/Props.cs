
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropInteraction;

namespace Flipsider
{
    public class Prop
    {
        public static List<PropInfo> props = new List<PropInfo>();

        public delegate void TileInteraction();

        public static string? CurrentProp;
        public static void LoadProps()
        {
            AddProp("Sky", TextureCache.GreenSlime);
            AddProp("Player", TextureCache.player);
            AddProp("Blob", TextureCache.Blob);
            AddPropInteraction("Blob", BlobInteractable);
            AddProp("HudSlot", TextureCache.hudSlot);
            AddProp("TestGun", TextureCache.testGun);
            AddProp("SaveTex", TextureCache.SaveTex);
        }
        public struct PropInfo
        {
            //do not make inherit from an Info interface, as I want to serialize this.. may turn these into bytes
            public int noOfFrames;
            public int animSpeed;
            public Vector2 position;
            public string prop;
            public int interactRange;
            public TileInteraction? tileInteraction;
            public PropInfo(string prop, Vector2 pos, TileInteraction? TileInteraction = null, int noOfFrames = 1, int animSpeed = 1)
            {
                this.noOfFrames = noOfFrames;
                this.animSpeed = animSpeed;
                position = pos;
                this.prop = prop;
                interactRange = 200;
                tileInteraction = TileInteraction;
            }
        }
        public static Dictionary<string, Texture2D> PropTypes = new Dictionary<string, Texture2D>();

        public static Dictionary<string, TileInteraction> PropInteractions = new Dictionary<string, TileInteraction>();
        public static void AddProp(string Prop, Texture2D tex) => PropTypes.Add(Prop, tex);
        public static void AddPropInteraction(string Prop, TileInteraction TI) => PropInteractions.Add(Prop, TI);
    }
    public static class PropInteraction
    {
       public static void BlobInteractable()
       {
            Debug.Write("GraydeeIsDumb");
       }

        public static void UpdatePropInteractions()
        {
            foreach(PropInfo PI in props)
            {
                if((Main.MouseScreen.ToVector2() - PI.position).Length() < PI.interactRange)
                {
                    if(Keyboard.GetState().IsKeyDown(Keys.E))
                    PI.tileInteraction?.Invoke();
                }
            }
        }

    }
}
