using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace FlipEngine
{
    public class Map
    {
        internal Dictionary<string, MapPass> MapPasses = new Dictionary<string, MapPass>();

        public void Sort() => MapPasses.OrderBy(key => key.Value.Priority);

        public void OrderedRenderPass(SpriteBatch sb, GraphicsDevice GD)
        {
            foreach (KeyValuePair<string, MapPass> Map in MapPasses) Map.Value.Render(sb, GD);
        }

        public List<RenderTarget2D> Buffers = new List<RenderTarget2D>();

        public RenderTarget2D OrderedShaderPass(SpriteBatch sb, RenderTarget2D target)
        {
            if (MapPasses.Count != 0)
            {
                int a = 0;
                foreach (KeyValuePair<string, MapPass> Map in MapPasses)
                {
                    FlipGame.graphics?.GraphicsDevice.SetRenderTarget(Buffers[a]);
                    FlipGame.graphics?.GraphicsDevice.Clear(Color.Transparent);

                    sb.End();
                    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: null, samplerState: SamplerState.PointClamp);

                    Map.Value.ApplyShader();                 

                    RenderTarget2D rT;
                    if (a < 1) rT = target; else rT = Buffers[a - 1];

                    if (FlipGame.graphics != null && FlipGame.Camera != null)
                    {
                        Rectangle frame = new Rectangle(0, 0,
                            (int)(FlipGame.Renderer.MaxResolution.X),
                            (int)(FlipGame.Renderer.MaxResolution.Y));

                        sb.Draw(rT, Vector2.Zero, frame, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }

                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: null, samplerState: SamplerState.PointClamp);

                    a++;
                }
                return Buffers[Buffers.Count - 1];
            }

            return target;
        }
        public void DrawToMap(string Map, MapRender MR) => MapPasses[Map].DrawToTarget(MR);

        public void AddMap(string MapName, int Index, MapPass MP)
        {
            MP.Parent = this;
            MapPasses.Add(MapName, MP);

            Buffers.Add(new RenderTarget2D(FlipGame.graphics.GraphicsDevice, 2560, 1440));
        }

        public MapPass Get(string MapName) => MapPasses[MapName];
    }
}