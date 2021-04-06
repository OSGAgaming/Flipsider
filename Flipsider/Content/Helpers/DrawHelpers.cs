using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Flipsider
{
    public static partial class Utils
    {
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 GetParallaxOffset(Vector2 center, float strenght) => (Main.mainCamera.CamPos - center) * strenght;

        public static MapPass GetMap(string MapName) => Main.lighting.Maps.Get(MapName);

        public static void DrawToMap(string MapName, MapRender MR) => Main.lighting.Maps.DrawToMap(MapName, MR);
        public static void BeginCameraSpritebatch()
        => Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);

        public static void BeginEndCameraSpritebatch()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp); 
        }
        public static void BeginAdditiveCameraSpritebatch()
       => Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
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
        public static void ApplyBloom(BloomSettings BS, Vector2 Dims)
        {
            Lighting.Bloom?.Parameters["NoOfFramesX"]?.SetValue(BS.NoOfFramesX);
            Lighting.Bloom?.Parameters["NoOfFramesY"]?.SetValue(BS.NoOfFramesY);
            Lighting.Bloom?.Parameters["FrameX"]?.SetValue(BS.FrameX);
            Lighting.Bloom?.Parameters["FrameY"]?.SetValue(BS.FrameY);
            Lighting.Bloom?.Parameters["BloomIntensity"]?.SetValue(BS.Intensity);
            Lighting.Bloom?.Parameters["BloomSaturation"]?.SetValue(BS.Saturation);
            Lighting.Bloom?.Parameters["Offsets"]?.SetValue(BS.Offsets);
            Lighting.Bloom?.Parameters["Weights"]?.SetValue(BS.Weights);
            Lighting.Bloom?.Parameters["Dims"]?.SetValue(Dims);
            Lighting.Bloom?.CurrentTechnique.Passes[0].Apply();
        }
        public static void DrawPixel(Vector2 pos, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, pos, tint);
        public static void DrawBoxFill(Vector2 pos, int width, int height, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, pos, new Rectangle(0, 0, width, height), tint);
        public static void DrawBoxFill(Rectangle rectangle, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, rectangle.Location.ToVector2(), new Rectangle(0, 0, rectangle.Width, rectangle.Height), tint);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            Main.spriteBatch.Draw(TextureCache.pixel, p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawText(string text, Color colour, Vector2 position, float rotation = 0f)
        {
            SpriteFont font = Main.font;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, rotation, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static float DrawTextToLeft(string text, Color colour, Vector2 position)
        {
            SpriteFont font = Main.font;
            float textPositionLeft = position.X;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
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
                spriteBatch.Draw(Tex, new Vector2(i * (Tex.Width * scale), 0).AddParallaxAcrossX(paralax).AddParallaxAcrossY(paralaxY) - new Vector2(Main.mainCamera.LeftBound * -paralax, 0) + offset, dims, Color, 0f, new Vector2(0, Tex.Height * scale), scale, SpriteEffects.None, 0f);

        }
        public static void RenderBGMoving(SpriteBatch spriteBatch, float speed, Color Color, Texture2D Tex, float paralax, float scale, Vector2 offset = default, float paralaxY = 0)
        {
            Rectangle dims = new Rectangle(0, 0, Tex.Width, Tex.Height);
            for (int i = 0; i < 6; i++)
                spriteBatch.Draw(Tex, new Vector2(i * (Tex.Width * scale) + speed * Time.TotalTimeSec, 0).AddParallaxAcrossX(paralax).AddParallaxAcrossY(paralaxY) - new Vector2(Main.mainCamera.LeftBound * -paralax, 0) + offset, dims, Color, 0f, new Vector2(0, Tex.Height * scale), scale, SpriteEffects.None, 0f);

        }
        public static void DrawRectangle(Rectangle rectangle, Color color, float thickness = 1)
        {
            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
        public static void DrawRectangle(RectangleF rectangle, Color color, float thickness = 1)
        {
            Vector2 point = rectangle.TL;
            float sizeX = rectangle.w;
            float sizeY = rectangle.h;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
    }
}
