using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended.BitmapFonts;

namespace FlipEngine
{
    public static partial class Utils
    {
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 GetParallaxOffset(Vector2 center, float strenght) => (FlipGame.Camera.TransformPosition - center) * strenght;

        public static MapPass GetMap(string MapName) => FlipGame.lighting.Maps.Get(MapName);

        public static void DrawToMap(string MapName, MapRender MR) => FlipGame.lighting.Maps.DrawToMap(MapName, MR);
        public static void BeginCameraSpritebatch()
        => FlipGame.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: FlipGame.Camera.Transform, samplerState: SamplerState.PointClamp);

        public static void BeginEndCameraSpritebatch()
        {
            FlipGame.spriteBatch.End();
            FlipGame.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: FlipGame.Camera.Transform, samplerState: SamplerState.PointClamp); 
        }
        public static void BeginAdditiveCameraSpritebatch()
       => FlipGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: FlipGame.Camera.Transform, samplerState: SamplerState.PointClamp);
        public static void QuickApplyShader(Effect effect)
        => effect?.CurrentTechnique.Passes[0].Apply();
        public static void QuickApplyShader(Effect effect, params float[] yo)
        {
            effect?.CurrentTechnique.Passes[0].Apply();
            for (int i = 0; i < yo.Length; i++)
            {
                effect?.Parameters[i]?.SetValue(yo[i]);
            }
        }
        public static void DrawPixel(Vector2 pos, Color tint) => FlipGame.spriteBatch.Draw(FlipTextureCache.pixel, pos, tint);
        public static void DrawBoxFill(Vector2 pos, int width, int height, Color tint) => FlipGame.spriteBatch.Draw(FlipTextureCache.pixel, pos, new Rectangle(0, 0, width, height), tint);
        public static void DrawBoxFill(Rectangle rectangle, Color tint, float depth = 0f) => 
            FlipGame.spriteBatch.Draw(FlipTextureCache.pixel, rectangle.Location.ToVector2(), new Rectangle(0, 0, rectangle.Width, rectangle.Height), 
            tint, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f, float depth = 0f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            FlipGame.spriteBatch.Draw(FlipTextureCache.pixel, p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, depth);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            sb.Draw(FlipTextureCache.pixel, p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawText(string text, Color colour, Vector2 position, float rotation = 0f)
        {
            SpriteFont font = FlipE.font;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            FlipGame.spriteBatch.DrawString(Fonts.Calibri, text, new Vector2(textPositionLeft, position.Y), colour, rotation, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
        }

        public static float DrawTextToLeft(string text, Color colour, Vector2 position, float layerDepth = 0f, float scale = 0.5f)
        {
            SpriteFont font = FlipE.font;
            float textPositionLeft = position.X;
            FlipGame.spriteBatch.DrawString(Fonts.Calibri, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            return font.MeasureString(text).X;
        }

        public static void DrawSquare(Vector2 point, float size, Color color)
        {
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y), color);
            DrawLine(point, new Vector2(point.X, point.Y + size), color);
            DrawLine(point, new Vector2(point.X + size, point.Y), color);
        }

        public static void DrawRectangle(Vector2 point, float sizeX, float sizeY, Color color, float thickness = 1)
        {
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }

        public static void RenderBG(SpriteBatch spriteBatch, Color Color, Texture2D Tex, float paralax, float scale, Vector2 offset = default, float paralaxY = 0)
        {
            Rectangle dims = new Rectangle(0, 0, Tex.Width, Tex.Height);
            for (int i = 0; i < 6; i++)
                spriteBatch.Draw(Tex, 
                    new Vector2(i * (Tex.Width * scale), 0).AddParallaxAcrossX(paralax).AddParallaxAcrossY(paralaxY) - 
                    new Vector2(FlipGame.Camera.LeftBound * -paralax, 0) + offset, dims, Color, 0f, 
                    new Vector2(0, Tex.Height), scale, SpriteEffects.None, 0f);

        }
        public static void RenderBGMoving(SpriteBatch spriteBatch, float speed, Color Color, Texture2D Tex, float paralax, float scale, Vector2 offset = default, float paralaxY = 0)
        {
            Rectangle dims = new Rectangle(0, 0, Tex.Width, Tex.Height);
            for (int i = 0; i < 6; i++)
                spriteBatch.Draw(Tex, new Vector2(i * (Tex.Width * scale) + speed * Time.TotalTimeSec, 0).AddParallaxAcrossX(paralax).AddParallaxAcrossY(paralaxY)
                    - new Vector2(FlipGame.Camera.LeftBound * -paralax, 0) + offset, dims, 
                    Color, 0f, new Vector2(0, Tex.Height * scale), scale, SpriteEffects.None, 0f);

        }
        public static void DrawRectangle(Rectangle rectangle, Color color = default, float thickness = 1)
        {
            if (color == default) color = Color.White;

            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rectangle, Color color, float thickness = 1)
        {
            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(sb, new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(sb, new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(sb, point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(sb, point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
        public static void DrawRectangle(RectangleF rectangle, Color color = default, float thickness = 1)
        {
            if (color == default) color = Color.White;

            Vector2 point = rectangle.TL;
            float sizeX = rectangle.width;
            float sizeY = rectangle.height;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
    }
}
