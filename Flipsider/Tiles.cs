﻿using Microsoft.Xna.Framework;
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
            int SafeLowerBoundX = (int)(Main.player.Center.X - Main.ScreenSize.X) / 16;
            int SafeUpperBoundX = (int)(Main.player.Center.X + Main.ScreenSize.X) / 16;
            int SafeLowerBoundY = (int)(Main.player.Center.Y - Main.ScreenSize.Y) / 16;
            int SafeUpperBoundY = (int)(Main.player.Center.Y + Main.ScreenSize.Y) / 16;
            for (int i = SafeLowerBoundX; i< SafeUpperBoundX; i++)
            {
                for(int j = SafeLowerBoundY; j < SafeUpperBoundY; j++)
                {
                    if (i > 0 && j > 0 && i < Main.MaxTilesX && j < Main.MaxTilesY)
                    {
                        if (Main.tiles[i, j].active && Main.tiles[i, j].atlas == null)
                        {
                            DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                        }
                        else if (Main.tiles[i, j].atlas != null)
                        {
                            Main.spriteBatch.Draw(Main.tiles[i, j].atlas, new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes), Main.tiles[i, j].frame, Color.White);
                        }
                    }
                }
            }
        }
        public static void ShowTileCursor()
        {
            if (Main.EditorMode && !Main.TileEditorMode)
            {
                int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                MouseState state = Mouse.GetState();
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
