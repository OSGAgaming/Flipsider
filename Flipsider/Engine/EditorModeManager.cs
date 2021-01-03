using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.GUI.HUD;
using Flipsider.Scenes;
using Flipsider.Engine.Particles;
using Flipsider.Engine;
using Flipsider.Engine.Audio;
using Flipsider.Engine.Input;
using Flipsider.GUI.TilePlacementGUI;
using static Flipsider.TileManager;

using System.Reflection;
using System.Linq;
using System.Threading;
using Flipsider.Engine.Interfaces;

namespace Flipsider
{
    public class EditorMode : IUpdate
    {
        public bool IsActive { get; set; }
        public EditorUIState CurrentState;
        public int currentType;
        public Rectangle currentFrame;
        public static EditorMode Instance;
        static EditorMode()
        {
            Instance = new EditorMode();
            Main.Updateables.Add(Instance);
        }
        public void ControlEditorScreen()
        {
            Main.mainCamera.FixateOnPlayer(Main.player);
            Main.mainCamera.rotation = 0;
            Main.mainCamera.scale += (Main.targetScale - Main.mainCamera.scale) / 16f;
        }

        public void Draw()
        {
            if (Main.player.SelectedItem != null)
            {
                Texture2D tex = Main.player.SelectedItem.inventoryIcon ?? TextureCache.magicPixel;
                Main.spriteBatch.Draw(tex, Main.MouseScreen.ToVector2() - tex.Bounds.Size.ToVector2() / 2 + Vector2.One * 4, Color.Black * 0.3f * (float)Math.Abs(Math.Sin(Time.TotalTimeMil / 120f)));
                Main.spriteBatch.Draw(tex, Main.MouseScreen.ToVector2() - tex.Bounds.Size.ToVector2() / 2, Color.White * (float)Math.Abs(Math.Sin(Time.TotalTimeMil / 120f)));

            }
            if (CurrentState == EditorUIState.LightEditorMode)
            {
                // Main.spriteBatch.Draw();
            }
        }
        public void Update()
        {
            if (!IsActive)
            {
                Main.mainCamera.offset -= Main.mainCamera.offset / 16f;
            }
            ControlEditorScreen();
            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                Main.CurrentWorld.tileManager.AddTile(Main.CurrentWorld, Main.MouseTile);
                PropManager.AddProp(Main.CurrentWorld);
            }
            if (GameInput.Instance["EdtiorRemoveTile"].IsDown())
            {
                Main.CurrentWorld.tileManager.RemoveTile(Main.CurrentWorld, Main.MouseTile);
            }
            if (GameInput.Instance["EditorSwitchModes"].IsJustPressed())
            {
                SwitchModes();
            }
            if (GameInput.Instance["InvEditorMode"].IsJustPressed())
            {
                SwitchToMode(EditorUIState.Inventory);
            }
            if (IsActive)
            {
                if (GameInput.Instance["EditorTileEditor"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.TileEditorMode);
                }
                if (GameInput.Instance["NPCEditor"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.NPCSpawnerMode);
                }
                if (GameInput.Instance["WorldSaverMode"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.WorldSaverMode);
                }
                if (GameInput.Instance["PropEditorMode"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.PropEditorMode);
                }
                if (GameInput.Instance["LightEditorMode"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.LightEditorMode);
                }
                float scrollSpeed = 0.02f;
                float camMoveSpeed = 0.2f;
                if (GameInput.Instance["EditorZoomIn"].IsDown())
                {
                    Main.mainCamera.targetScale += scrollSpeed;
                }
                if (GameInput.Instance["EditorZoomOut"].IsDown())
                {
                    Main.mainCamera.targetScale -= scrollSpeed;
                }

                if (GameInput.Instance["MoveRight"].IsDown())
                {
                    Main.mainCamera.offset.X += camMoveSpeed;
                }
                if (GameInput.Instance["MoveLeft"].IsDown())
                {
                    Main.mainCamera.offset.X -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveUp"].IsDown())
                {
                    Main.mainCamera.offset.Y -= camMoveSpeed;
                }
                if (GameInput.Instance["MoveDown"].IsDown())
                {
                    Main.mainCamera.offset.Y += camMoveSpeed;
                }
            }
            else
            {
                CurrentState = EditorUIState.None;
            }
        }
        public void SwitchToMode(EditorUIState state)
        {
            if (CurrentState == EditorUIState.None)
            {
                CurrentState = state;
            }
            else
            {
                CurrentState = EditorUIState.None;
            }
        }
        public void SwitchModes()
        {
            IsActive = !IsActive;
            if (IsActive)
            {
                Main.mainCamera.targetScale = 0.8f;
            }
            else
            {
                Main.mainCamera.targetScale = 1.2f;
            }
        }
    }
}
