
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropInteraction;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class PropManager
    {
        public List<Prop> props = new List<Prop>();

        public delegate void TileInteraction();

        public void LoadProps()
        {
            # region death
            Layer = 1;
            AddPropType("Misc_Sky", TextureCache.GreenSlime);
            AddPropType("Ruins_1", TextureCache.BrickStructure1);
            AddPropType("Ruins_2", TextureCache.BrickStructure2);
            AddPropType("MediumTree_1", TextureCache.MediumTree1);
            AddPropType("MediumTree_2", TextureCache.MediumTree2);
            AddPropType("BackgroundTree_1", TextureCache.BackgroundTree1);
            AddPropType("BackgroundTree_2", TextureCache.BackgroundTree2);
            AddPropType("BackgroundTree_3", TextureCache.BackgroundTree3);
            AddPropType("BackgroundTree_4", TextureCache.BackgroundTree4);
            AddPropType("Foreground_Grass", TextureCache.ForegroundGrass1);
            AddPropType("City_TrafficLight", TextureCache.TrafficLight);
            AddPropType("City_BusStop", TextureCache.BusStop);
            AddPropType("City_BigBusStop", TextureCache.BigBusStop);
            AddPropType("City_BikeRack", TextureCache.BikeRack);
            AddPropType("City_StopSigns", TextureCache.StopSigns);
            AddPropType("City_StreetLights", TextureCache.StreetLights);

            AddPropType("Forest_ForestTree1", TextureCache.ForestTree1);
            AddPropType("Forest_ForestTree2", TextureCache.ForestTree2);
            AddPropType("Forest_ForestFlowerOne", TextureCache.ForestFlowerOne);
            AddPropType("Forest_ForestFlowerTwo", TextureCache.ForestFlowerTwo);
            AddPropType("Forest_ForestFlowerThree", TextureCache.ForestFlowerThree);
            AddPropType("Forest_ForestFlowerFour", TextureCache.ForestFlowerFour);
            AddPropType("Forest_ForestFlowerFive", TextureCache.ForestFlowerFive);
            AddPropType("Forest_ForestFlowerSix", TextureCache.ForestFlowerSix);

            AddPropType("Forest_ForestBushOne", TextureCache.ForestBushOne);
            AddPropType("Forest_ForestLogOne", TextureCache.ForestLogOne);
            AddPropType("Forest_ForestDecoOne", TextureCache.ForestDecoOne);
            AddPropType("Forest_ForestDecoTwo", TextureCache.ForestDecoTwo);

            AddPropType("Debug_Player", TextureCache.player);
            AddPropType("Debug_Blob", TextureCache.Blob);
            AddPropInteraction("Debug_Blob", BlobInteractable);
            AddPropType("Debug_HudSlot", TextureCache.hudSlot);
            AddPropType("Debug_TestGun", TextureCache.testGun);
            AddPropType("Debug_SaveTex", TextureCache.SaveTex);

            ChangeFrames("City_StreetLights", 5);
            ChangeFrames("City_StopSigns", 3);
            ChangeAnimSpeed("City_StreetLights", 20);
            ChangeAnimSpeed("City_StopSigns", 20);
            # endregion
        }
        public int Layer { get; set; }

        public static Dictionary<string, Texture2D> PropTypes = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Prop> PropEntites = new Dictionary<string, Prop>();
        public static int AddPropType(string Prop, Texture2D tex)
        {
            PropTypes.Add(Prop, tex);
            PropEntites.Add(Prop, new Prop(Prop));
            return PropTypes.Count - 1;
        }
        public void AddProp(World world, string PropType, Vector2 position)
        {
            try
            {
                TileInteraction? currentInteraction = null;
                if (PropEntites.ContainsKey(PropType ?? ""))
                {
                    currentInteraction = PropEntites[PropType ?? ""].tileInteraction;
                }
                if (TileManager.UselessCanPlaceBool || Main.isLoading || Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
                {
                    int alteredRes = Main.CurrentWorld.TileRes / 4;
                    props.Add(new Prop(PropType ?? "", position - PropTypes[PropType ?? ""].Bounds.Size.ToVector2() / 2 + new Vector2(alteredRes / 2, alteredRes / 2), currentInteraction, 1, -1, 0, LayerHandler.CurrentLayer));
                }
                TileManager.UselessCanPlaceBool = true;
            }
            catch
            {


            }
        }
        public void AddProp(World world, string PropType, Vector2 position, int Layer)
        {
            try
            {
                TileInteraction? currentInteraction = null;
                if (PropEntites.ContainsKey(PropType ?? ""))
                {
                    currentInteraction = PropEntites[PropType ?? ""].tileInteraction;
                }
                if (TileManager.UselessCanPlaceBool || Main.isLoading || Main.Editor.CurrentState == EditorUIState.WorldSaverMode)
                {
                    int alteredRes = Main.CurrentWorld.TileRes / 4;
                    props.Add(new Prop(PropType ?? "", position - PropTypes[PropType ?? ""].Bounds.Size.ToVector2() / 2 + new Vector2(alteredRes / 2, alteredRes / 2), currentInteraction, 1, -1, 0, Layer));
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
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                int alteredRes = Main.CurrentWorld.TileRes / 4;
                Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                if (Main.Editor.CurrentProp != null)
                {
                    Main.spriteBatch.Draw(PropTypes[Main.Editor.CurrentProp], tilePoint2 + new Vector2(alteredRes / 2, alteredRes / 2), PropEntites[Main.Editor.CurrentProp].alteredFrame, Color.White * Math.Abs(sine), 0f, PropEntites[Main.Editor.CurrentProp].alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
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
    public class PropInteraction : IUpdate
    {
        public static void BlobInteractable()
        {
 
        }
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
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                        propManager.props[i].tileInteraction?.Invoke();
                }


            }
            mousePressedRight = Mouse.GetState().RightButton == ButtonState.Pressed;
        }

    }
}
