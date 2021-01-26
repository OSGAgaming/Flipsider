﻿using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flipsider
{
    public class EditorMode : IUpdate
    {
        public bool IsActive { get; set; }
        public EditorUIState CurrentState;
        public int currentType;
        public Tile[,]? currentTileSet;
        public Rectangle currentFrame;
        public string? CurrentProp;
        public string? CurrentSaveFile;
        public static EditorMode Instance;
        static EditorMode()
        {
            Instance = new EditorMode();
            Main.Updateables.Add(Instance);
            Main.mainCamera.scale = 1;
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
        public bool CanSwitch;
        internal EditorUIState GetState()
            => CurrentState;
        internal bool StateCheck(EditorUIState EUS)
            => EUS == CurrentState;
        public void Update()
        {
            if (!IsActive && Main.CurrentScene.Name != "Main Menu")
            {
                Main.mainCamera.offset -= Main.mainCamera.offset / 16f;
            }
            ControlEditorScreen();
            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
                    Main.CurrentWorld.tileManager.AddTile(Main.CurrentWorld, Main.Editor.currentType, Main.MouseTile);
            }
            if (GameInput.Instance["EditorPlaceTile"].IsJustPressed())
            {
                MouseState state = Mouse.GetState();
                Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                int alteredRes = Main.CurrentWorld.TileRes / 4;
                Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                if (Main.Editor.CurrentState == EditorUIState.PropEditorMode)
                    Main.CurrentWorld.propManager.AddProp(Main.CurrentWorld, Main.Editor.CurrentProp ?? "", tilePoint2);
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
                if (GameInput.Instance["EditorTileEditor"].IsJustPressed() && CurrentState != EditorUIState.WorldSaverMode)
                {
                    SwitchToMode(EditorUIState.TileEditorMode);
                }
                if (GameInput.Instance["NPCEditor"].IsJustPressed() && CurrentState != EditorUIState.WorldSaverMode)
                {
                    SwitchToMode(EditorUIState.NPCSpawnerMode);
                }
                if (GameInput.Instance["WorldSaverMode"].IsJustPressed())
                {
                    SwitchToMode(EditorUIState.WorldSaverMode);
                }
                if (GameInput.Instance["PropEditorMode"].IsJustPressed() && CanSwitch && CurrentState != EditorUIState.WorldSaverMode)
                {
                    SwitchToMode(EditorUIState.PropEditorMode);
                }
                if (GameInput.Instance["LightEditorMode"].IsJustPressed() && CurrentState != EditorUIState.WorldSaverMode)
                {
                    SwitchToMode(EditorUIState.LightEditorMode);
                }
                if (GameInput.Instance["WaterEditorMode"].IsJustPressed() && CurrentState != EditorUIState.WorldSaverMode)
                {
                    SwitchToMode(EditorUIState.WaterEditorMode);
                }
                if (GameInput.Instance["CollideablesEditorMode"].IsJustPressed() && CurrentState != EditorUIState.CollideablesEditorMode)
                {
                    SwitchToMode(EditorUIState.CollideablesEditorMode);
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
            CanSwitch = true;
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
