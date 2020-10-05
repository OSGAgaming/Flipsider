using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Core
{
    /// <summary>
    /// Defines a wrapper for <see cref="SpriteBatch"/> that allows safe beginning/ending.
    /// </summary>
    public sealed class SafeSpriteBatch
    {
        /// <summary>
        /// The actual spritebatch instance. Only call Draw on it. Use the safe spritebatch methods for Begin/End.
        /// </summary>
        public SpriteBatch Sb { get; } = new SpriteBatch(FlipsiderGame.Graphics.GraphicsDevice);

        public bool Begun { get; private set; }

        /// <summary>
        /// See <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix?)"/>
        /// </summary>
        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState? blendState = null,
                            SamplerState? samplerState = null, DepthStencilState? depthStencilState = null,
                            RasterizerState? rasterizerState = null, Effect? effect = null,
                            Matrix? transformMatrix = null)
        {
            End();
            Begun = true;
            Sb.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        /// <summary>
        /// See <see cref="SpriteBatch.End"/>
        /// </summary>
        public void End()
        {
            if (Begun)
            {
                Begun = false;
                Sb.End();
            }
        }

        ~SafeSpriteBatch()
        {
            if (!Sb.IsDisposed)
                Sb.Dispose();
        }
    }
}
