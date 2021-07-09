using Flipsider.Content.IO.Graphics;
using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flipsider
{
    public class EditorMode : IUpdate
    {
        public bool CanZoom { get; set; }

        public bool IsActive { get; set; }
        public bool Interactable { get; set; }
        public EditorUIState CurrentState;
        public int currentType;
        public Tile[,]? currentTileSet;
        public Rectangle currentFrame;
        public string? CurrentProp;
        public string? CurrentSaveFile;
        public static EditorMode Instance;
        public bool AutoFrame = true;
        static EditorMode()
        {
            Instance = new EditorMode();
            Main.Updateables.Add(Instance);
            Main.Camera.Scale = 1;
        }
        public void ControlEditorScreen()
        {
            Main.Camera.Rotation = 0;
            Main.Camera.Scale += (Main.targetScale / GameCamera.Scaling - Main.Camera.Scale) / 16f;

            Main.Camera.UpdateTransform();
        }

        public void Draw()
        {
            if (Main.player.SelectedItem != null)
            {
                Texture2D tex = Main.player.SelectedItem.inventoryIcon ?? TextureCache.magicPixel;
                Main.spriteBatch.Draw(tex, Main.MouseScreen.ToVector2() - tex.Bounds.Size.ToVector2() / 2 + Vector2.One * 4, Color.Black * 0.3f * (float)Math.Abs(Math.Sin(Time.TotalTimeMil / 120f)));
                Main.spriteBatch.Draw(tex, Main.MouseScreen.ToVector2() - tex.Bounds.Size.ToVector2() / 2, Color.White * (float)Math.Abs(Math.Sin(Time.TotalTimeMil / 120f)));
            }
        }
        public bool CanSwitch;
        internal bool StateCheck(EditorUIState EUS)
            => EUS == CurrentState;
        public void Update()
        {
            ControlEditorScreen();
            CanZoom = true;
            CanSwitch = true;
            Interactable = true;
        }
    }
}
