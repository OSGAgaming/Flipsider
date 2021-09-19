using Flipsider.Content.IO.Graphics;
using FlipEngine;

using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public partial class Utils
    {
        public static readonly int BOTTOM = 3000;
        public static readonly int PrefferedWidth = 1980;
        public static readonly int PrefferedHeight = 1080;

        public static void SaveCurrentWorldAs(string Name)
        {
            //SAME NAME WORLDS WILL OVERRIDE
            Stream stream = File.OpenWrite(WorldPath + Name + ".flip");
            Main.World.levelInfo.Serialize(stream);
        }
        public static void SaveCurrentWorldAsWithExtension(string Name)
        {
          //  Main.serializers.Serialize(Main.CurrentWorld.levelInfo, Main.MainPath + Name);
        }

        public static string WorldPath => Main.MainPath + $@"Content\Worlds\";
        public static string CutscenePath => Main.MainPath + $@"Cutscenes\";

        public static InputBinding LeftClick => GameInput.Instance["EditorPlaceTile"];
        public static bool JustClicked => GameInput.Instance["EditorPlaceTile"].IsJustPressed();
        public static bool JustUnclicked => GameInput.Instance["EditorPlaceTile"].IsJustReleased();

        public static Vector2 SafeBoundX => new Vector2(Main.Gamecamera.Position.X, Main.Gamecamera.Position.X + Main.ActualScreenSize.X / Main.ScreenScale);
        public static Vector2 SafeBoundY => new Vector2(Main.Gamecamera.Position.Y, Main.Gamecamera.Position.Y + Main.ActualScreenSize.Y / Main.ScreenScale);
        public static Vector2 MouseTile => new Vector2(MouseScreen.X / TileManager.tileRes, MouseScreen.Y / TileManager.tileRes);
        public static Vector2 ScreenSize => Main.graphics.GraphicsDevice == null ? Vector2.One : Main.Renderer.PreferredSize;
        public static Vector2 ActualScreenSize => Main.AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => Main.ActualScreenSize - Main.ScreenSize;
        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);
        public static bool MouseInBounds => Main.Renderer.Destination.Contains(Mouse.GetState().Position);
    }
}
