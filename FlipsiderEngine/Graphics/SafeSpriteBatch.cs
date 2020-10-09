using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Graphics
{
    /// <summary>
    /// Defines a wrapper for <see cref="SpriteBatch"/> that allows safe beginning/ending.
    /// </summary>
    public sealed class SafeSpriteBatch
    {
        /// <summary>
        /// The actual spritebatch instance. Only use if you know what you're doing.
        /// </summary>
        public SpriteBatch Sb { get; } = new SpriteBatch(FlipsiderGame.GameInstance.GraphicsDevice);

        /// <summary>
        /// Whether this sprite batch has had its Begin method called. Begin must be called before calling Draw.
        /// </summary>
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
        /// Draws the given DrawData.
        /// </summary>
        public void Draw(DrawData data)
        {
            data.Draw(Sb);
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
