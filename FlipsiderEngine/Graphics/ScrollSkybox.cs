using Flipsider.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider.Graphics
{
    public abstract class ScrollSkybox : Skybox
    {
        private class TypeFromDelegate : ScrollSkybox
        {
            private readonly Func<IEnumerable<ParallaxDrawData>> get;

            public TypeFromDelegate(Func<IEnumerable<ParallaxDrawData>> get)
            {
                this.get = get;
            }

            protected override IEnumerable<ParallaxDrawData> GetScrollingTextures()
            {
                return get();
            }
        }

        public static ScrollSkybox FromDelegate(Func<IEnumerable<ParallaxDrawData>> getDrawn)
        {
            return new TypeFromDelegate(getDrawn);
        }

        private static readonly SpriteBatch sb = new SpriteBatch(FlipsiderGame.GameInstance.GraphicsDevice);

        protected abstract IEnumerable<ParallaxDrawData> GetScrollingTextures();

        public override void Update() { }

        public override void Draw(SafeSpriteBatch spriteBatch)
        {
            var viewport = FlipsiderGame.GameInstance.CurrentCamera.Viewport;
            sb.Begin(samplerState: SamplerState.LinearWrap);
            foreach (var item in GetScrollingTextures())
            {
                var data = item.Data;
                data.ZDepth = item.ParallaxFactor;
                data.Frame = new Rectangle(item.Data.ScreenPosition.ToPoint(), viewport.Bounds.Size);
                data.ScreenPosition = new Vector2(viewport.X, viewport.Y);
                data.Draw(sb);
            }
            sb.End();
        }
    }
}
