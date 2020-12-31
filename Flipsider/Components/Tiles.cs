using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
namespace Flipsider
{
    public class TileManager
    {
        public static int tileRes = 32;
        public static List<Tile> tileTypes = new List<Tile>();

        public static void LoadTiles()
        {
            AddTileType(0, TextureCache.TileSet1);
            AddTileType(1, TextureCache.TileSet2);
            AddTileType(2, TextureCache.TileSet3);
            LoadProps();
        }

        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
           // Main.instance.ser.Serialize(tiles, Main.MainPath + Name + ".txt");
        }
        //In the alpha phase, Im keeping this as a struct when we want to port to drawn tiles
      
        public static Dictionary<int, Texture2D> tileDict = new Dictionary<int, Texture2D>();

        public static void AddTile(World world, int X, int Y)
        {

            if (EditorModes.CurrentState == EditorUIState.TileEditorMode)
            {
                try
                {
                    world.tiles[X,Y] = new Tile(EditorModes.currentType, EditorModes.currentFrame)
                    {
                        active = true
                    };
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }

        }
        public static void AddTile(World world, Vector2 XY)
        {

            if (EditorModes.CurrentState == EditorUIState.TileEditorMode)
            {
                try
                {
                    world.tiles[(int)XY.X, (int)XY.Y] = new Tile(EditorModes.currentType, EditorModes.currentFrame)
                    {
                        active = true
                    };
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }

        public static void AddTileType(int type, Texture2D atlas, bool ifWall = false)
        {
            tileTypes.Add(new Tile(type, new Rectangle(0, 0, 32, 32),ifWall));
            tileDict.Add(type, atlas);
        }

        public static void RemoveTile(World world, int X, int Y)
        {
            if (EditorModes.EditorMode)
            {
                try
                {
                    world.tiles[X, Y].active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }
        public static void RemoveTile(World world, Vector2 XY)
        {
            if (EditorModes.EditorMode)
            {
                try
                {
                    world.tiles[(int)XY.X, (int)XY.Y].active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }

        public static void RenderTiles(World world)
        {
            float scale = Main.mainCamera.scale;
            scale = Math.Clamp(scale, 0.5f, 1);
            int fluff = 10;
            Vector2 SafeBoundX = new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ScreenSize.X / Main.ScreenScale) / 32;
            Vector2 SafeBoundY = new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ScreenSize.Y / Main.ScreenScale) / 32;
            for (int i = (int)SafeBoundX.X - fluff; i < (int)SafeBoundX.Y + fluff; i++)
            {
                for (int j = (int)SafeBoundY.X - fluff; j < (int)SafeBoundY.Y + fluff; j++)
                {
                    if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && world.tiles[i,j] != null)
                    {
                        if (world.tiles[i, j].active)
                        {
                            if (world.tiles[i, j].type == -1)
                            {
                                DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                            }
                            else
                            {
                                world.tiles[i, j].frame = Framing.GetTileFrame(world,i, j);
                                Main.spriteBatch.Draw(tileDict[world.tiles[i, j].type], new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes), world.tiles[i, j].frame, Color.White);
                            }
                        }
                    }
                }
            }

        }
        
        public static bool UselessCanPlaceBool;
        public static void ShowTileCursor(World world)
        {
            if (EditorModes.EditorMode)
            {
                int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                Rectangle TileFrame = Framing.GetTileFrame(world,(int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                if (EditorModes.CurrentState == EditorUIState.TileEditorMode)
                {
                    if (EditorModes.currentType == -1)
                    {
                        DrawMethods.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
                    }
                    else
                    {
                        Main.spriteBatch.Draw(tileDict[EditorModes.currentType], tilePoint + new Vector2(tileRes / 2, tileRes / 2), TileFrame, Color.White * Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
