
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
            AddProp("TrafficLight", TextureCache.TrafficLight);
            AddProp("BusStop", TextureCache.BusStop);
            AddProp("BigBusStop", TextureCache.BigBusStop);
            AddProp("BikeRack", TextureCache.BikeRack);
            AddProp("StopSigns", TextureCache.StopSigns);
            AddProp("StreetLights", TextureCache.StreetLights);
            ChangeFrames("StreetLights", 5);
            ChangeAnimSpeed("StreetLights", 3);
        }

        public struct PropInfo
        {
            public int noOfFrames;
            public int animSpeed;
            public Vector2 position;
            public int frameCounter;
            public string prop;
            public Vector2 Center => position + new Vector2(PropTypes[prop].Width/2, PropTypes[prop].Height/2);
            public Rectangle alteredFrame => new Rectangle(frameCounter/ PropEntites[prop].animSpeed% PropEntites[prop].noOfFrames * (PropTypes[prop].Width / PropEntites[prop].noOfFrames), 0, PropTypes[prop].Width / (PropEntites[prop].noOfFrames), PropTypes[prop].Height);
            public int interactRange;
            public TileInteraction? tileInteraction;
            public PropInfo(string prop, Vector2 pos, TileInteraction? TileInteraction = null, int noOfFrames = 1, int animSpeed = -1,int frameCount = 0)
            {
                this.noOfFrames = noOfFrames;
                this.animSpeed = animSpeed;
                position = pos;
                this.prop = prop;
                interactRange = 30;
                tileInteraction = TileInteraction;
                frameCounter = frameCount;
            }

            
        }
        public static Dictionary<string, Texture2D> PropTypes = new Dictionary<string, Texture2D>();

        public static Dictionary<string, PropInfo> PropEntites = new Dictionary<string, PropInfo>();
        public static int AddProp(string Prop, Texture2D tex)
        { 
            PropTypes.Add(Prop, tex);
            PropEntites.Add(Prop, new PropInfo(Prop, Vector2.Zero, null));
            return PropTypes.Count - 1; 
        }
        public static void AddPropInteraction(string Prop, TileInteraction TI)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop] = new PropInfo(Prop,Vector2.Zero,TI);
            }
            else
            {
                PropEntites.Add(Prop, new PropInfo(Prop, Vector2.Zero, TI));
            }
        }
        public static void ChangeFrames(string Prop, int frames)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop] = new PropInfo(Prop, Vector2.Zero, PropEntites[Prop].tileInteraction,frames);
            }
            else
            {
                PropEntites.Add(Prop, new PropInfo(Prop, Vector2.Zero, null, frames));
            }
        }
        public static void ChangeAnimSpeed(string Prop, int speed)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop] = new PropInfo(Prop, Vector2.Zero, PropEntites[Prop].tileInteraction, PropEntites[Prop].noOfFrames,speed);
            }
            else
            {
                PropEntites.Add(Prop, new PropInfo(Prop, Vector2.Zero, null, 1,speed));
            }
        }
    }
    public static class PropInteraction
    {
       public static void BlobInteractable()
       {
            Debug.Write("GraydeeIsDumb");
       }

        public static void UpdatePropInteractions()
        {
            for(int i = 0; i<props.Count; i++)
            {
                if ((Main.MouseScreen.ToVector2() - props[i].Center).Length() < props[i].interactRange)
                {
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        props.RemoveAt(i);
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                        props[i].tileInteraction?.Invoke();
                }
            }
        }

    }
}
