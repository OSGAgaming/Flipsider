
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
using System.Reflection;

namespace Flipsider
{
    public class PropManager
    {
        public static List<Prop> props = new List<Prop>();

        public delegate void TileInteraction();

        public static string? CurrentProp;
        public static void LoadProps()
        {
            AddPropType("Sky", TextureCache.GreenSlime);
            AddPropType("Player", TextureCache.player);
            AddPropType("Blob", TextureCache.Blob);
            AddPropInteraction("Blob", BlobInteractable);
            AddPropType("HudSlot", TextureCache.hudSlot);
            AddPropType("TestGun", TextureCache.testGun);
            AddPropType("SaveTex", TextureCache.SaveTex);
            AddPropType("TrafficLight", TextureCache.TrafficLight);
            AddPropType("BusStop", TextureCache.BusStop);
            AddPropType("BigBusStop", TextureCache.BigBusStop);
            AddPropType("BikeRack", TextureCache.BikeRack);
            AddPropType("StopSigns", TextureCache.StopSigns);
            AddPropType("StreetLights", TextureCache.StreetLights);
            ChangeFrames("StreetLights", 5);
            ChangeFrames("StopSigns", 3);
            ChangeAnimSpeed("StreetLights", 20);
            ChangeAnimSpeed("StopSigns", 20);
        }

        public static void RenderProps()
        {
            if(delay > 0)
            delay--;
            for (int i = 0; i < props.Count; i++)
            {
                props[i].frameCounter++;
                Main.spriteBatch.Draw(PropTypes[props[i].prop], props[i].Center, props[i].alteredFrame, Color.White, 0f, props[i].alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }

        public static Dictionary<string, Texture2D> PropTypes = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Prop> PropEntites = new Dictionary<string, Prop>();
        public static int AddPropType(string Prop, Texture2D tex)
        { 
            PropTypes.Add(Prop, tex);
            PropEntites.Add(Prop, new Prop(Prop, Vector2.Zero, null));
            return PropTypes.Count - 1; 
        }
        public static int delay;
        public static void AddProp()
        {
            if (Main.Editor.CurrentState == EditorUIState.PropEditorMode && delay == 0)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    int alteredRes = TileManager.tileRes / 4;
                    Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                    TileInteraction? currentInteraction = null;
                    if (PropEntites.ContainsKey(CurrentProp ?? ""))
                    {
                        currentInteraction = PropEntites[CurrentProp ?? ""].tileInteraction;
                    }
                    if (TileManager.UselessCanPlaceBool)
                    {
                        props.Add(new Prop(CurrentProp ?? "", tilePoint2 - PropTypes[CurrentProp ?? ""].Bounds.Size.ToVector2() / 2 + new Vector2(alteredRes / 2, alteredRes / 2), currentInteraction));
                        delay = 30;
                    }
                    TileManager.UselessCanPlaceBool = true;
                }
                catch
                {


                }
            }
        }

        public static void ShowPropCursor()
        {
            if (Main.Editor.CurrentState == EditorUIState.PropEditorMode)
            {
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                int alteredRes = TileManager.tileRes / 4;
                Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                if (CurrentProp != null)
                {
                    Main.spriteBatch.Draw(PropTypes[CurrentProp], tilePoint2 + new Vector2(alteredRes / 2, alteredRes / 2), PropEntites[CurrentProp].alteredFrame, Color.White * Math.Abs(sine), 0f, PropEntites[CurrentProp].alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public static void AddPropInteraction(string Prop, TileInteraction TI)
        {
            if (PropEntites.ContainsKey(Prop))
            {
                PropEntites[Prop].tileInteraction = TI;
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
