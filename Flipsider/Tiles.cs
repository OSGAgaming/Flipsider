using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Flipsider
{
    public class TileManager
    {
        public static int tileRes = 32;
        public static List<Tile> tileTypes = new List<Tile>();
        public static List<PropInfo> props = new List<PropInfo>();
        public static Tile[,] tiles;
        public static string CurrentProp;

        public static void LoadTiles()
        {
            AddTileType(0, TextureCache.TileSet1);
            AddTileType(1, TextureCache.TileSet2);
            AddTileType(2, TextureCache.TileSet3);
            AddProp("Sky", TextureCache.GreenSlime);
            AddProp("Player", TextureCache.player);
            AddProp("Blob", TextureCache.Blob);
            AddProp("HudSlot", TextureCache.hudSlot);
            AddProp("TestGun", TextureCache.testGun);
            AddProp("SaveTex", TextureCache.SaveTex);
        }

        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
            Main.instance.ser.Serialize(tiles, Main.MainPath + Name + ".txt");
        }
        public static int MaxTilesX
        {
            get => 1000;
        }

        public static int MaxTilesY
        {
            get => 1000;
        }
        public struct PropInfo
        {
            //do not make inherit from an Info interface, as I want to serialize this.. may turn these into bytes
            public int noOfFrames;
            public int animSpeed;
            public Vector2 position;
            public string prop;
            public PropInfo(string prop, Vector2 pos, int noOfFrames = 1, int animSpeed = 1)
            {
                this.noOfFrames = noOfFrames;
                this.animSpeed = animSpeed;
                position = pos;
                this.prop = prop;
            }
            //   public void Draw(Vector2 pos) => Main.spriteBatch.Draw(atlas, pos * 16, frame, Color.White);
        }
        //In the alpha phase, Im keeping this as a struct when we want to port to drawn tiles
        [Serializable]
        public struct Tile
        {
            public int type;
            [NonSerialized]
            public Rectangle frame;
            public bool active;
            public bool wall;
            public Tile(int type, Rectangle frame, bool ifWall = false)
            {
                this.type = type;
                this.frame = frame;
                active = false;
                wall = ifWall;
            }
            //   public void Draw(Vector2 pos) => Main.spriteBatch.Draw(atlas, pos * 16, frame, Color.White);
        }

        public static Dictionary<int, Texture2D> tileDict = new Dictionary<int, Texture2D>();
        public static Dictionary<string, Texture2D> Props = new Dictionary<string, Texture2D>();

        public static void AddTile()
        {

            if (EditorModes.EditorMode && EditorModes.CurrentState != EditorUIState.TileEditorMode && EditorModes.CurrentState != EditorUIState.PropEditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    Point tilePoint = new Point((int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                    tiles[tilePoint.X, tilePoint.Y] = new Tile(Main.currentType, Main.currentFrame)
                    {
                        active = true
                    };
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
            if (EditorModes.CurrentState == EditorUIState.PropEditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    int alteredRes = tileRes / 2;
                    Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                    props.Add(new PropInfo(CurrentProp, tilePoint2 - Props[CurrentProp].Bounds.Size.ToVector2() / 2 + new Vector2(alteredRes/2, alteredRes/2)));
                }
                catch
                {


                }
            }
        }

        public static void AddTileType(int type, Texture2D atlas, bool ifWall = false)
        {
            tileTypes.Add(new Tile(type, new Rectangle(0, 0, 32, 32),ifWall));
            tileDict.Add(type, atlas);
        }
        public static void AddProp(string Prop, Texture2D tex) => Props.Add(Prop, tex);

        public static void RemoveTile()
        {
            if (EditorModes.EditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    Point tilePoint = new Point(Main.MouseScreen.X / tileRes, Main.MouseScreen.Y / tileRes);
                    tiles[tilePoint.X, tilePoint.Y].active = false;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }

        public static void RenderTiles()
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
                    if (i > 0 && j > 0 && i < MaxTilesX && j < MaxTilesY)
                    {
                        if (tiles[i, j].active)
                        {
                            if (tiles[i, j].type == -1)
                            {
                                DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                            }
                            else
                            {
                                tiles[i, j].frame = GetTileFrame(i, j);
                                Main.spriteBatch.Draw(tileDict[tiles[i, j].type], new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes), tiles[i, j].frame, Color.White);
                            }
                        }
                    }
                }
            }
            foreach (PropInfo propInfo in props)
            {
                Main.spriteBatch.Draw(Props[propInfo.prop], propInfo.position,Props[propInfo.prop].Bounds, Color.White);
            }
        }
        public static Rectangle GetTileFrame(int i, int j)
        {
            //fuck this is gonna be messy:
            if (i > 0 && j > 0 && i < MaxTilesX && j < MaxTilesY)
            {
                bool upLeft = tiles[i - 1, j - 1].active;
                bool upMid = tiles[i, j - 1].active;
                bool upRight = tiles[i + 1, j - 1].active;

                bool left = tiles[i - 1, j].active;
                bool right = tiles[i + 1, j].active;

                bool downLeft = tiles[i - 1, j + 1].active;
                bool downMid = tiles[i, j + 1].active;
                bool downRight = tiles[i + 1, j + 1].active;

                //non sloped for now

                if (!upMid && !left && right && !downMid)
                {
                    return new Rectangle(0, 0, 32, 32);
                }
                if (!upMid && left && right && !downMid)
                {
                    return new Rectangle(32, 0, 32, 32);
                }
                if (!upMid && left && !right && !downMid)
                {
                    return new Rectangle(64, 0, 32, 32);
                }
                if (!upMid && !left && !right && downMid)
                {
                    return new Rectangle(96, 0, 32, 32);
                }

                if (!upMid && !left && right && downMid)
                {
                    if (!downRight)
                    {
                        return new Rectangle(192, 128, 32, 32);
                    }
                    return new Rectangle(0, 32, 32, 32);
                }
                if (!upMid && left && right && downMid)
                {
                    if (!downLeft && !downRight)
                    {
                        return new Rectangle(224, 192, 32, 32);
                    }
                    if (!downLeft)
                    {
                        return new Rectangle(0, 352, 32, 32);
                    }
                    if (!downRight)
                    {
                        return new Rectangle(32, 352, 32, 32);
                    }
                    return new Rectangle(32, 32, 32, 32);
                }
                if (!upMid && left && !right && downMid)
                {
                    if (!downLeft)
                    {
                        return new Rectangle(224, 128, 32, 32);
                    }
                    return new Rectangle(64, 32, 32, 32);
                }
                if (upMid && !left && !right && downMid)
                {
                    return new Rectangle(96, 32, 32, 32);
                }

                if (upMid && !left && right && downMid)
                {
                    if (!upRight && !downRight)
                    {
                        return new Rectangle(160, 192, 32, 32);
                    }
                    if (!upRight)
                    {
                        return new Rectangle(64, 352, 32, 32);
                    }
                    if (!downRight)
                    {
                        return new Rectangle(64, 384, 32, 32);
                    }
                    return new Rectangle(0, 64, 32, 32);
                }
                if (upMid && left && right && downMid)
                {
                    if (!upLeft && upRight && downLeft && !downRight)
                    {
                        return new Rectangle(0, 128, 32, 32);
                    }
                    if (upLeft && !upRight && !downLeft && downRight)
                    {
                        return new Rectangle(32, 128, 32, 32);
                    }
                    if (!upLeft && !upRight && !downLeft && downRight)
                    {
                        return new Rectangle(64, 128, 32, 32);
                    }
                    if (!upLeft && !upRight && downLeft && !downRight)
                    {
                        return new Rectangle(96, 128, 32, 32);
                    }
                    if (!upLeft && upRight && downLeft && downRight)
                    {
                        return new Rectangle(128, 128, 32, 32);
                    }
                    if (upLeft && !upRight && downLeft && downRight)
                    {
                        return new Rectangle(160, 128, 32, 32);
                    }

                    if (!upLeft && upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(64, 160, 32, 32);
                    }
                    if (upLeft && !upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(96, 160, 32, 32);
                    }
                    if (upLeft && upRight && !downLeft && downRight)
                    {
                        return new Rectangle(128, 160, 32, 32);
                    }
                    if (upLeft && upRight && downLeft && !downRight)
                    {
                        return new Rectangle(160, 160, 32, 32);
                    }
                    if (!upLeft && !upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(0, 192, 32, 32);
                    }
                    if (!upLeft && !upRight && downLeft && downRight)
                    {
                        return new Rectangle(32, 192, 32, 32);
                    }
                    if (upLeft && upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(64, 192, 32, 32);
                    }
                    if (!upLeft && upRight && !downLeft && downRight)
                    {
                        return new Rectangle(96, 192, 32, 32);
                    }
                    if (upLeft && !upRight && downLeft && !downRight)
                    {
                        return new Rectangle(128, 192, 32, 32);
                    }

                    return new Rectangle(32, 64, 32, 32);
                }
                if (upMid && left && !right && downMid)
                {
                    if (!upLeft && !downLeft)
                    {
                        return new Rectangle(192, 192, 32, 32);
                    }
                    if (!upLeft)
                    {
                        return new Rectangle(96, 352, 32, 32);
                    }
                    if (!downLeft)
                    {
                        return new Rectangle(96, 384, 32, 32);
                    }
                    return new Rectangle(64, 64, 32, 32);
                }
                if (upMid && !left && !right && !downMid)
                {
                    return new Rectangle(96, 64, 32, 32);
                }

                if (upMid && !left && right && !downMid)
                {
                    if (!upRight)
                    {
                        return new Rectangle(192, 160, 32, 32);
                    }
                    return new Rectangle(0, 96, 32, 32);
                }
                if (upMid && left && right && !downMid)
                {
                    if (!upLeft && !upRight)
                    {
                        return new Rectangle(224, 224, 32, 32);
                    }
                    if (!upLeft)
                    {
                        return new Rectangle(0, 384, 32, 32);
                    }
                    if (!upRight)
                    {
                        return new Rectangle(32, 384, 32, 32);
                    }
                    return new Rectangle(32, 96, 32, 32);
                }
                if (upMid && left && !right && !downMid)
                {
                    if (!upLeft)
                    {
                        return new Rectangle(224, 160, 32, 32);
                    }
                    return new Rectangle(64, 96, 32, 32);
                }
                if (!upMid && !left && !right && !downMid)
                {
                    return new Rectangle(96, 96, 32, 32);
                }

            }
            return new Rectangle(0, 0, 32, 32);

        }

        public static void ShowTileCursor()
        {
            if (EditorModes.EditorMode)
            {
                int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                Rectangle TileFrame = GetTileFrame((int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                if (EditorModes.CurrentState != EditorUIState.TileEditorMode && EditorModes.CurrentState != EditorUIState.PropEditorMode)
                {
                    if (Main.currentType == -1)
                    {
                        DrawMethods.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
                    }
                    else
                    {
                        Main.spriteBatch.Draw(tileDict[Main.currentType], tilePoint + new Vector2(tileRes / 2, tileRes / 2), TileFrame, Color.White * Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
                    }
                }
                if(EditorModes.CurrentState == EditorUIState.PropEditorMode)
                {
                    int alteredRes = tileRes / 2;
                    Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                    if (CurrentProp != null)
                    {
                        Main.spriteBatch.Draw(Props[CurrentProp], tilePoint2 + new Vector2(alteredRes / 2, alteredRes / 2), Props[CurrentProp].Bounds, Color.White * Math.Abs(sine), 0f, Props[CurrentProp].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
