using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name + ".flip");
        }
        public static void SaveCurrentWorldAsWithExtension(string Name)
        {
            Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name);
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
                        Active = true
                    };

                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }

        }
        public static bool CanPlace;
        public void AddTile(World world, int type, Vector2 XY)
        {
            if (CanPlace)
            {
                try
                {
                    if (world.IsTileActive((int)XY.X, (int)XY.Y))
                    {
                        tiles[(int)XY.X, (int)XY.Y].Kill();
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(Main.Editor.currentType, Main.Editor.currentFrame, new Vector2((int)XY.X, (int)XY.Y))
                        {
                            Active = true
                        };
                    }
                    else
                    {
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(type, Main.Editor.currentFrame, XY)
                        {
                            Active = true
                        };
                    }
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, (int)XY.X, (int)XY.Y);
                    tiles[(int)XY.X, (int)XY.Y].AddModule("Collision", new Collideable(tiles[(int)XY.X, (int)XY.Y], true, CollisionPoly, true,default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
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
        public void AddTile(World world, int type, Vector2 XY, int Layer)
        {
            if (CanPlace)
            {
                try
                {
                    if (world.IsTileActive((int)XY.X, (int)XY.Y))
                    {
                        tiles[(int)XY.X, (int)XY.Y].Kill();
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(Main.Editor.currentType, Main.Editor.currentFrame, XY, Layer)
                        {
                            Active = true
                        };
                    }
                    else
                    {
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(type, Main.Editor.currentFrame, XY, Layer)
                        {
                            Active = true
                        };
                    }
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, (int)XY.X, (int)XY.Y);
                    tiles[(int)XY.X, (int)XY.Y].AddModule("Collision",new Collideable(tiles[(int)XY.X, (int)XY.Y], true, CollisionPoly, true, default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
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
        public void AddTile(World world, int type, Vector2 XY, int Layer,Rectangle frame)
        {
            if (CanPlace)
            {
                try
                {
                    if (world.IsTileActive((int)XY.X, (int)XY.Y))
                    {
                        tiles[(int)XY.X, (int)XY.Y].Kill();
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(Main.Editor.currentType, frame, XY, Layer)
                        {
                            Active = true
                        };
                    }
                    else
                    {
                        tiles[(int)XY.X, (int)XY.Y] = new Tile(type, frame, XY, Layer)
                        {
                            Active = true
                        };
                    }
                    Polygon CollisionPoly = Framing.GetPolygon(Main.CurrentWorld, (int)XY.X, (int)XY.Y);
                    tiles[(int)XY.X, (int)XY.Y].AddModule("Collision", new Collideable(tiles[(int)XY.X, (int)XY.Y], true, CollisionPoly, true, default, CollisionPoly.Center == Vector2.Zero ? PolyType.Rectangle : PolyType.ConvexPoly));
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
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
                    tiles[X, Y].Kill();
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
                        tiles[(int)XY.X, (int)XY.Y].Active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
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
        }

        public static bool UselessCanPlaceBool;
        public void ShowTileCursor(World world)
        {
            if (Main.Editor.IsActive)
            {
                if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
                {
                    int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                    Vector2 mousePos = Main.MouseScreen.ToVector2();
                    Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                    float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                    Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                    Rectangle TileFrame = AutoFrame ? Framing.GetTileFrame(world, (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes) : Main.Editor.currentFrame;

                    if (Main.Editor.currentType == -1)
                    {
                        Utils.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
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
