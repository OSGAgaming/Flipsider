using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class Map
    {
        internal Dictionary<string, MapPass> MapPasses = new Dictionary<string, MapPass>();
        public void OrderedRenderPass(SpriteBatch sb, GraphicsDevice GD)
        {
            int i = 0; 
            foreach(KeyValuePair<string, MapPass> Map in MapPasses)
            {
                var Pass = Map.Value;

                if(Pass.Index == i) Pass.Render(sb, GD);

                i++;
            }
        }

        public List<RenderTarget2D> Buffers = new List<RenderTarget2D>();
        public RenderTarget2D OrderedShaderPass(SpriteBatch sb, RenderTarget2D target)
        {
            int i = 0;
            foreach (KeyValuePair<string, MapPass> Map in MapPasses)
            {
                Main.graphics?.GraphicsDevice.SetRenderTarget(Buffers[i]);
                Main.graphics?.GraphicsDevice.Clear(Color.Transparent);

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: Main.mainCamera?.Transform, samplerState: SamplerState.PointClamp);

                var Pass = Map.Value;

                if (Pass.Index == i) Pass.ApplyShader();

                RenderTarget2D rT;
                if (i - 1 < 0) rT = target; else rT = Buffers[i - 1];

                if (Main.graphics != null && Main.mainCamera != null)
                {
                    Rectangle frame = new Rectangle(0,0,2560,1440);
                    sb.Draw(rT, Main.mainCamera.CamPos, frame, Color.White, 0f, Vector2.Zero, new Vector2(1/Main.ScreenScale, 1/Main.ScreenScale), SpriteEffects.None, 0f);
                }

                i++;

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: Main.mainCamera?.Transform, samplerState: SamplerState.PointClamp);
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(null);

            return Buffers[Buffers.Count - 1];
        }
        public void DrawToMap(string Map, MapRender MR) => MapPasses[Map].DrawToTarget(MR);

        public void AddMap(string MapName,int Index, MapPass MP) 
        {
            MP.Parent = this;
            MP.Index = Index;
            MapPasses.Add(MapName, MP);
            Buffers.Add(new RenderTarget2D(Main.graphics.GraphicsDevice,2560,1440));
        }

        public MapPass Get(string MapName) => MapPasses[MapName];
    }
}