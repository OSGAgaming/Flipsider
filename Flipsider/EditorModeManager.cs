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
        public static bool TileEditorMode { get; set; }
        public static bool NPCSpawnerMode { get; set; }
        public static bool WorldSaverMode { get; set; }
        public static bool EditorMode { get; set; }
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
                    SwitchToTileEditorMode();
                }
                if (GameInput.Instance["NPCEditor"].IsJustPressed())
                {
                    SwitchToNPCEditorMode();
                }
                if (GameInput.Instance["WorldSaverMode"].IsJustPressed())
                {
                    SwitchToWorldSaverMode();
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
        static void SwitchToTileEditorMode()
        {
            TileEditorMode = !TileEditorMode;
        }
        static void SwitchToNPCEditorMode()
        {
            NPCSpawnerMode = !NPCSpawnerMode;
        }
        static void SwitchToWorldSaverMode()
        {
            WorldSaverMode = !WorldSaverMode;
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
