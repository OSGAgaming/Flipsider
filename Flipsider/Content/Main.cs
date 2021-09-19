using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using FlipEngine;
using Flipsider.Scenes;
using Flipsider.Content.IO.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable

    public partial class Main : FlipGame
    {
        protected override void OnInitialize()
        {
            FlipEngine.EffectCache.LoadEffects(Content);
            EffectCache.LoadEffects(Content);
            AutoloadTextures.GetAllAssetPaths(Utils.AssetDirectory);
            AutoloadTextures.LoadTexturesToAssetCache(Content);
            Textures.LoadTextures();

            World = new FlipsiderWorld(2000, 2000);
            World.SetSkybox(new CitySkybox());
            FlipWorld.AppendPlayer(new Player(new Vector2(100, Utils.BOTTOM)));
            TextureCache.LoadTextures(Content);
            Renderer.MainCamera = new GameCamera();
            RegisterControls.Invoke();
            SceneManager.Instance.SetNextScene(new MainMenu(), null);
            // Register controls
        }
        protected override void OnLoadContent()
        {
        }
        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void InitializeEnd()
        {
        }
    }
}
