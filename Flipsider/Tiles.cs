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
            public Tile(Texture2D atlas, Rectangle frame)
            {
                this.atlas = atlas;
                this.frame = frame;
            }
            public void Draw(Vector2 pos) => Main.spriteBatch.Draw(atlas, pos * 16, frame, Color.White);
        }
        

        public static void AddTile()
        {
            if(Main.EditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                    Point tilePoint = new Point((int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                    Main.tiles[tilePoint.X, tilePoint.Y] = 1;
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
                    Point tilePoint = new Point((int)mousePos.X / tileRes, (int)mousePos.Y / tileRes);
                    Main.tiles[tilePoint.X, tilePoint.Y] = 0;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }

        public static void RenderTiles()
        {
            for(int i = 0; i< Main.MaxTilesX; i++)
            {
                for(int j = 0; j< Main.MaxTilesY; j++)
                {
                    if(Main.tiles[i,j] == 1)
                    {
                        DrawMethods.DrawSquare(new Vector2(i*tileRes, j * tileRes), tileRes, Color.White);
                    }
                }
            }
        }
        public static void ShowTileCursor()
        {
            if (Main.EditorMode)
            {
                MouseState state = Mouse.GetState();
                Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds*6);
                DrawMethods.DrawSquare(tilePoint, tileRes, Color.White * Math.Abs(sine));
            }
        }
    }
}
