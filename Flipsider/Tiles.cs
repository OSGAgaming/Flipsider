using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class TileManager
    {
        public static int tileRes = 32;
        //In the alpha phase, Im keeping this as a struct when we want to port to drawn tiles
        struct Tile
        {
            public int type;
            public int style;
            public Tile(int type, int style)
            {
                this.type = type;
                this.style = style;
            }
        }
        

        public static void AddTile()
        {
            if(Main.EditorMode)
            {
                try
                {
                    MouseState state = Mouse.GetState();
                    Point tilePoint = new Point(state.Position.X / tileRes, state.Position.Y / tileRes);
                    Main.tiles[tilePoint.X, tilePoint.Y] = 1;
                }
                catch
                {
                    Debug.Write("Just put the cursor in your ass next time eh?");
                }
            }
        }

        public static void RenderTiles()
        {
            for(int i = 0; i< Main.tiles.GetLength(0); i++)
            {
                for(int j = 0; j< Main.tiles.GetLength(1);j++)
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
                Vector2 tilePoint = new Vector2(state.Position.X / tileRes * tileRes, state.Position.Y / tileRes * tileRes);
                float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds*6);
                DrawMethods.DrawSquare(tilePoint, tileRes, Color.White * Math.Abs(sine));
            }
        }
    }
}
