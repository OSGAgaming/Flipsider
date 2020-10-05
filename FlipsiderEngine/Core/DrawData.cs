using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Flipsider
{
    public sealed class DrawData
    {
        public IEnumerable<DrawData> Children { get; }

        public DrawData(IEnumerable<DrawData> children)
        {
            Children = children;
        }

        public DrawData(DrawDataInfo info) : this(info, Array.Empty<DrawData>()) { }

        public DrawData(DrawDataInfo info, IEnumerable<DrawData> children)
        {
            Info = info;
            Children = children;
        }

        public DrawDataInfo? Info { get; }

        // Used to prevent infinite recursion from children calling parent Draw functions.
        private bool drawing;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (drawing) 
                return;
            drawing = true;
            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
            if (Info != null)
            {
                spriteBatch.Draw(Info.Texture,
                                 Info.Position,
                                 Info.SourceRect,
                                 Info.Tint,
                                 Info.Rotation.RadF,
                                 new Vector2(Info.Texture.Value.Width / 2f, Info.Texture.Value.Height / 2f),
                                 Info.Scale ?? Vector2.One,
                                 Info.Effects,
                                 0);
            }
            drawing = false;
        }
    }
}