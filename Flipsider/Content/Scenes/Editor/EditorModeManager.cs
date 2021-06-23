using Flipsider.Content.IO.Graphics;
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
            Main.Camera.UpdateTransform();
            Main.Camera.Rotation = 0;
            Main.Camera.Scale += (Main.targetScale / GameCamera.Scaling - Main.Camera.Scale) / 10f;
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
            ControlEditorScreen();
            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
                    Main.tileManager.AddTile(Main.CurrentWorld,new Tile(currentType,currentFrame, Main.MouseTile));
            }
            if (GameInput.Instance["EditorPlaceTile"].IsJustPressed())
            {
                MouseState state = Mouse.GetState();
                Vector2 mousePos = new Vector2(state.Position.X, state.Position.Y).ToScreen();
                int alteredRes = Main.CurrentWorld.TileRes / 4;
                Vector2 tilePoint2 = new Vector2((int)mousePos.X / alteredRes * alteredRes, (int)mousePos.Y / alteredRes * alteredRes);
                if (Main.Editor.CurrentState == EditorUIState.PropEditorMode)
                    Main.CurrentWorld.propManager.AddProp(Main.Editor.CurrentProp ?? "", tilePoint2);

                if (Main.Editor.CurrentState == EditorUIState.NPCSpawnerMode && NPC.SelectedNPCType != null)
                    NPC.SpawnNPC(Main.MouseScreen.ToVector2(), NPC.SelectedNPCType);
            }
            if (GameInput.Instance["EdtiorRemoveTile"].IsDown())
            {
                Main.CurrentWorld.tileManager.RemoveTile(Main.CurrentWorld, Main.MouseTile.ToPoint());
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
                float camMoveSpeed = 5f;
                if (Interactable)
                {
                    if (CanZoom)
                    {
                        if (GameInput.Instance["EditorZoomIn"].IsDown())
                        {
                            Main.Camera.targetScale += scrollSpeed;
                        }
                        if (GameInput.Instance["EditorZoomOut"].IsDown())
                        {
                            Main.Camera.targetScale -= scrollSpeed;
                        }
                    }
                    if (GameInput.Instance["MoveRight"].IsDown())
                    {
                        Main.Camera.Offset.X += camMoveSpeed;
                    }
                    if (GameInput.Instance["MoveLeft"].IsDown())
                    {
                        Main.Camera.Offset.X -= camMoveSpeed;
                    }
                    if (GameInput.Instance["MoveUp"].IsDown())
                    {
                        Main.Camera.Offset.Y -= camMoveSpeed;
                    }
                    if (GameInput.Instance["MoveDown"].IsDown())
                    {
                        Main.Camera.Offset.Y += camMoveSpeed;
                    }
                }
            }
            else
            {
                CurrentState = EditorUIState.None;
            }
            CanZoom = true;
            CanSwitch = true;
            Interactable = true;
        }
        public void SwitchToMode(EditorUIState state)
        {
            if (Interactable)
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
        }
        public void SwitchModes()
        {
            if (Interactable)
            {
                IsActive = !IsActive;
                if (IsActive)
                {
                    Main.Camera.targetScale = 0.8f;
                }
                else
                {
                    Main.Camera.targetScale = 2f;
                }
            }
        }
    }
}
