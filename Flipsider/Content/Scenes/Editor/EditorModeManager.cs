using Flipsider.Content.IO.Graphics;
using FlipEngine;

using FlipEngine;
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
            FlipE.Updateables.Add(Instance);
            Main.Gamecamera.Scale = 1;
        }

        public bool CanSwitch;

        public void Update()
        {
            CanZoom = true;
            CanSwitch = true;
            Interactable = true;
        }
    }
}
