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
    public partial class TileManager
    {
        public const int tileRes = 32;
        public List<Tile> tileTypes = new List<Tile>();
        public Dictionary<int, Texture2D> tileDict = new Dictionary<int, Texture2D>();
        public Tile[,] tiles;
        public bool AutoFrame = true;
        public TileManager(int width, int height)
        {
            Debug.Write("ran");
            tiles = new Tile[width, height];
            LoadTileTypes();
        }
        public void AddTileType(int type, Texture2D atlas, bool ifWall = false)
        {
            tileTypes.Add(new Tile(type, new Rectangle(0, 0, 32, 32), ifWall));
            tileDict.Add(type, atlas);
        }

        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
             Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name + ".txt");
        }
        //In the alpha phase, Im keeping this as a struct when we want to port to drawn tiles


        public void AddTile(World world, int X, int Y)
        {

            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                try
                {
                    tiles[X, Y] = new Tile(Main.Editor.currentType, Main.Editor.currentFrame, new Vector2(X, Y))
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
        public static bool CanPlace;
        public void AddTile(World world,int type, Vector2 XY)
        {
            if (CanPlace)
            {
                if (Main.Editor.CurrentState == EditorUIState.TileEditorMode || Main.isLoading)
                {
                    try
                    {
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(type, Main.Editor.currentFrame, XY)
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
            if (AutoFrame)
            {
                for (int i = (int)XY.X - 1; i < (int)XY.X + 2; i++)
                    for (int j = (int)XY.Y - 1; j < (int)XY.Y + 2; j++)
                    {
                        if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && tiles[i, j] != null)
                        {
                             tiles[i, j].frameX = Framing.GetTileFrame(world, i, j).Location.X;
                             tiles[i, j].frameY = Framing.GetTileFrame(world, i, j).Location.Y;
                        }
                    }
            }
            CanPlace = true;
        }
        public void RemoveTile(World world, int X, int Y)
        {
            if (Main.Editor.IsActive)
            {
                try
                {
                    tiles[X, Y].active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }
        public void RemoveTile(World world, Vector2 XY)
        {
            if (Main.Editor.IsActive)
            {
                try
                {
                    if (tiles[(int)XY.X, (int)XY.Y] != null)
                        tiles[(int)XY.X, (int)XY.Y].active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
                if (AutoFrame)
                {
                    for (int i = (int)XY.X - 1; i < (int)XY.X + 2; i++)
                        for (int j = (int)XY.Y - 1; j < (int)XY.Y + 2; j++)
                        {
                            if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && tiles[i, j] != null)
                            {
                                tiles[i, j].frame = Framing.GetTileFrame(world, i, j);
                            }
                        }
                }
            }
        }

        public void RenderTiles(World world, SpriteBatch spriteBatch)
        {
            float scale = Main.mainCamera.scale;
            scale = Math.Clamp(scale, 0.5f, 1);
            int fluff = 10;
            Vector2 SafeBoundX = new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ActualScreenSize.X / Main.ScreenScale) / 32;
            Vector2 SafeBoundY = new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ActualScreenSize.Y / Main.ScreenScale) / 32;
            for (int i = (int)SafeBoundX.X - fluff; i < (int)SafeBoundX.Y + fluff; i++)
            {
                for (int j = (int)SafeBoundY.X - fluff; j < (int)SafeBoundY.Y + fluff; j++)
                {
                    if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && tiles[i, j] != null)
                    {
                        if (tiles[i, j].active)
                        {
                            if (tiles[i, j].type == -1)
                            {
                                DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(tileDict[tiles[i, j].type], new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes), tiles[i, j].frame, Color.White);
                            }
                        }
                    }
                }
            }
        }

        public static bool UselessCanPlaceBool;
        public void ShowTileCursor(World world)
        {
            if (Main.Editor.IsActive)
            {
                int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                Rectangle TileFrame = Framing.GetTileFrame(world, (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
                {
                    if (Main.Editor.currentType == -1)
                    {
                        DrawMethods.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
                    }
                    else
                    {
                        if (tileDict[Main.Editor.currentType] != null)
                            Main.spriteBatch.Draw(tileDict[Main.Editor.currentType], tilePoint + new Vector2(tileRes / 2, tileRes / 2), TileFrame, Color.White * Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
