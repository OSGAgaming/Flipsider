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

namespace Flipsider
{
    public class EditorModes
    {
        public static bool EditorMode { get; set; }
        public static EditorUIState CurrentState;
        static void ControlEditorScreen()
        {
            Main.mainCamera.FixateOnPlayer(Main.player);
            Main.mainCamera.rotation = 0;
            Main.mainCamera.scale += (Main.targetScale - Main.mainCamera.scale) / 16f;
        }
        public static void Update()
        {

            ControlEditorScreen();
            if (GameInput.Instance["EditorPlaceTile"].IsDown())
            {
                AddTile();
            }
            if (GameInput.Instance["EdtiorRemoveTile"].IsDown())
            {
                RemoveTile();
            }
            if (GameInput.Instance["EditorSwitchModes"].IsJustPressed())
            {
                SwitchModes();
            }

            if (EditorMode)
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
            }
            if (EditorMode)
            {
                float scrollSpeed = 0.02f;
                float camMoveSpeed = 0.2f;
                if (GameInput.Instance["EditorZoomIn"].IsDown())
                {
                    Main.targetScale += scrollSpeed;
                }
                if (GameInput.Instance["EditorZoomOut"].IsDown())
                {
                    Main.targetScale -= scrollSpeed;
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
        }
        static void SwitchToMode(EditorUIState state)
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
        static void SwitchModes()
        {
            EditorMode = !EditorMode;
            if (EditorMode)
            {
                Main.targetScale = 0.8f;
            }
            else
            {
                Main.targetScale = 1.2f;
            }
        }
    }
}
