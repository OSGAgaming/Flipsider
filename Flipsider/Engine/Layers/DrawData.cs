using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
   public struct DrawData
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle source;
        public Color color;
        public float rotation;
        public Vector2 origin;
        public Vector2 scale;
        public SpriteEffects spriteEffects;
        public float layerDepth;
        public DrawData(
            Texture2D texture, 
            Vector2 position, 
            Rectangle source, 
            Color color, 
            float rotation, 
            Vector2 origin,
            Vector2 scale, 
            SpriteEffects spriteEffects, 
            float layerDepth)
        {
            this.texture = texture;
            this.position = position;
            this.source = source;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
        }
        public DrawData(
            Texture2D texture,
            Rectangle destination,
            Rectangle source,
            Color color)
        {
            this.texture = texture;
            position = destination.Location.ToVector2();
            this.source = source;
            this.color = color;
            rotation = 0f;
            origin = Vector2.Zero;
            scale = new Vector2(destination.Width/(float)source.Width,destination.Height/(float)source.Height);
            spriteEffects = SpriteEffects.None;
            layerDepth = 0f;
        }

        public static DrawData Null => new DrawData();

    }

    public static class DrawDataUtils
    {
        public static void Draw(this SpriteBatch sb, DrawData DD)
        {
            if(DD.texture != null)
            sb.Draw(DD.texture,DD.position,DD.source,DD.color,DD.rotation,DD.origin,DD.scale,DD.spriteEffects,DD.layerDepth);
        }
        public static void DrawOffset(this SpriteBatch sb, DrawData DD, Vector2 offset)
        {
            if (DD.texture != null)
                sb.Draw(DD.texture, DD.position + offset, DD.source, DD.color, DD.rotation, DD.origin, DD.scale, DD.spriteEffects, DD.layerDepth);
        }
    }
}
