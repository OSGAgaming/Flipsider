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
    public static partial class Utils
    {
        public static readonly int BOTTOM = 3000;
        public static readonly int PrefferedWidth = 1980;
        public static readonly int PrefferedHeight = 1080;

        public static string WorldPath => Main.MainPath + $@"Content\Worlds\";
        public static string CutscenePath => Main.MainPath + $@"Cutscenes\";

        public static InputBinding LeftClick => GameInput.Instance["EditorPlaceTile"];
        public static bool JustClicked => GameInput.Instance["EditorPlaceTile"].IsJustPressed();
        public static bool JustUnclicked => GameInput.Instance["EditorPlaceTile"].IsJustReleased();
        public static bool JustSaved => GameInput.Instance["MoveDown"].IsJustPressed() && Keyboard.GetState().IsKeyDown(Keys.LeftControl);

        public static Vector2 ScreenSize => Main.graphics.GraphicsDevice == null ? Vector2.One : Main.Renderer.PreferredSize;
        public static Vector2 ActualScreenSize => Main.AScreenSize;
        public static Point MouseScreen => Mouse.GetState().Position.ToScreen();
        public static Vector2 AbsD => Main.ActualScreenSize - Main.ScreenSize;
        public static Vector2 ScreenCenterUI => new Vector2(ActualScreenSize.X / 2, ActualScreenSize.Y / 2);
        public static bool MouseInBounds => Main.Renderer.Destination.Contains(Mouse.GetState().Position);
    }
}
