
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    public delegate void MapRender(SpriteBatch spriteBatch);
    public abstract class MapPass
    {
        public RenderTarget2D? MapTarget;

        internal event MapRender? MapActions;
        protected abstract Effect? MapEffect { get; }
        public abstract int Priority { get; }

        internal virtual void OnApplyShader()
        {
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }
        public void ApplyShader()
        {
            MapEffect?.Parameters["screenPosition"]?.SetValue(FlipGame.Camera.TransformPosition);
            MapEffect?.Parameters["screenScale"]?.SetValue(FlipGame.ScreenScale);
            MapEffect?.Parameters["screenSize"]?.SetValue(new Vector2(2560, 1440));
            MapEffect?.Parameters["noiseMap"]?.SetValue(TextureCache.Noise);
            MapEffect?.Parameters["Map"]?.SetValue(MapTarget);
            MapEffect?.Parameters["Time"]?.SetValue(Time.TotalTimeMil / 60f);

            OnApplyShader();
        }
        public void DrawToTarget(MapRender method) => MapActions += method;
        public void Render(SpriteBatch spriteBatch, GraphicsDevice GD)
        {
            GD.SetRenderTarget(MapTarget);
            GD.Clear(Color.Transparent);
            MapActions?.Invoke(spriteBatch);
            OnDraw();
            MapActions = null;
        }

        internal virtual void OnDraw() { }

        public Map? Parent;
        public MapPass()
        {
            Load();
        }

        public virtual void Load()
        {
            MapTarget = new RenderTarget2D(FlipGame.graphics.GraphicsDevice, 2560, 1440);
        }
    }
}