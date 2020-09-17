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

        //In the alpha phase, Im keeping this as a struct when we want to port to drawn tiles
        public struct Tile
        {
            public Texture2D atlas;
            public Rectangle frame;
            public bool active;
            public Tile(Texture2D atlas, Rectangle frame)
            {
                this.atlas = atlas;
                this.frame = frame;
                active = false;
            }
            public void Draw(Vector2 pos) => Main.spriteBatch.Draw(atlas, pos * 16, frame, Color.White);
        }
        

        public static void AddTile()
        {
            if(Main.EditorMode && !Main.TileEditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    Point tilePoint = new Point((int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                    Main.tiles[tilePoint.X, tilePoint.Y] = new Tile(Main.currentAtlas, Main.currentFrame)
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

        public static void AddTileType(Texture2D Atlas,string Name)
        {
            tileTypes.Add(new Tile(Atlas, new Rectangle(0, 0, 32, 32)));
        }

        public static void RemoveTile()
        {
            if (Main.EditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    Point tilePoint = new Point(Main.MouseScreen.X / tileRes, Main.MouseScreen.Y / tileRes);
                    Main.tiles[tilePoint.X, tilePoint.Y].active = false;
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
            scale = Math.Clamp(scale,0.5f,1);
            int fluff = 10;
            Vector2 SafeBoundX = new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ScreenSize.X/Main.ScreenScale)/32;
            Vector2 SafeBoundY = new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ScreenSize.Y / Main.ScreenScale) /32;
            for (int i = (int)SafeBoundX.X - fluff; i< (int)SafeBoundX.Y + fluff; i++)
            {
                for(int j = (int)SafeBoundY.X - fluff; j < (int)SafeBoundY.Y + fluff; j++)
                {
                    if (i > 0 && j > 0 && i < Main.MaxTilesX && j < Main.MaxTilesY)
                    {
                        if (Main.tiles[i, j].active)
                        {
                            if (Main.tiles[i, j].atlas == null)
                            {
                                DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                            }
                            else
                            {
                                Main.tiles[i, j].frame = GetTileFrame(i, j);
                                Main.spriteBatch.Draw(Main.tiles[i, j].atlas, new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes), Main.tiles[i, j].frame, Color.White);
                            }
                        }
                    }
                }
            }
        }
        public static Rectangle GetTileFrame(int i, int j)
        {
            //fuck this is gonna be messy:

            bool upLeft = Main.tiles[i - 1, j - 1].active;
            bool upMid = Main.tiles[i, j - 1].active;
            bool upRight = Main.tiles[i + 1, j - 1].active;

            bool left = Main.tiles[i - 1, j].active;
            bool right = Main.tiles[i + 1, j].active;

            bool downLeft = Main.tiles[i - 1, j + 1].active;
            bool downMid = Main.tiles[i, j + 1].active;
            bool downRight = Main.tiles[i + 1, j + 1].active;

            //non sloped for now

            //This is for non diagonal relative ones
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
                return new Rectangle(0, 32, 32, 32);
            }
            if (!upMid && left && right && downMid)
            {
                return new Rectangle(32, 32, 32, 32);
            }
            if (!upMid && left && !right && downMid)
            {
                return new Rectangle(64, 32, 32, 32);
            }
            if (upMid && !left && !right && downMid)
            {
                return new Rectangle(96, 32, 32, 32);
            }

            if (upMid && !left && right && downMid)
            {
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
                return new Rectangle(64, 64, 32, 32);
            }
            if (upMid && !left && !right && !downMid)
            {
                return new Rectangle(96, 64, 32, 32);
            }

            if (upMid && !left && right && !downMid)
            {
                return new Rectangle(0, 96, 32, 32);
            }
            if (upMid && left && right && !downMid)
            {
                return new Rectangle(32, 96, 32, 32);
            }
            if (upMid && left && !right && !downMid)
            {
                return new Rectangle(64, 96, 32, 32);
            }
            if (!upMid && !left && !right && !downMid)
            {
                return new Rectangle(96, 96, 32, 32);
            }


            return new Rectangle(0, 0, 32, 32);
        }

        public static void ShowTileCursor()
        {
            if (Main.EditorMode && !Main.TileEditorMode)
            {
                int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                Vector2 mousePos = Main.MouseScreen.ToVector2();
                Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds*6);
                Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                if (Main.currentAtlas == null)
                {
                    DrawMethods.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
                }
                else
                {
                    Main.spriteBatch.Draw(Main.currentAtlas, tilePoint + new Vector2(tileRes / 2, tileRes / 2), Main.currentFrame, Color.White * Math.Abs(sine),0f,new Vector2(tileRes / 2, tileRes / 2),1f,SpriteEffects.None,0f);
                }
            }
        }
    }
}
