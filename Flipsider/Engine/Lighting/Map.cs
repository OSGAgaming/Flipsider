using Flipsider.Engine.Interfaces;
using Flipsider.GUI.TilePlacementGUI;
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
            for (int a = 0; a < MapPasses.Count; a++)
            {
                foreach (KeyValuePair<string, MapPass> Map in MapPasses)
                {
                    var Pass = Map.Value;

                    if (Pass.Priority == a) Pass.Render(sb, GD);
                }
            }
        }

        public List<RenderTarget2D> Buffers = new List<RenderTarget2D>();

        public RenderTarget2D OrderedShaderPass(SpriteBatch sb, RenderTarget2D target)
        {
            for (int a = 0; a < MapPasses.Count; a++)
            {
                Main.graphics?.GraphicsDevice.SetRenderTarget(Buffers[a]);
                Main.graphics?.GraphicsDevice.Clear(Color.Transparent);



                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: Main.Camera?.Transform, samplerState: SamplerState.PointClamp);

                foreach (KeyValuePair<string, MapPass> Map in MapPasses)
                {
                    var Pass = Map.Value;

                    if (Pass.Priority == a) Pass.ApplyShader();
                }

                RenderTarget2D rT;
                if (a < 1) rT = target; else rT = Buffers[a - 1];

                if (Main.graphics != null && Main.Camera != null)
                {
                    Rectangle frame = new Rectangle(0, 0, 2560, 1440);
                    sb.Draw(rT, Main.Camera.Position, frame, Color.White, 0f, Vector2.Zero, new Vector2(1 / Main.ScreenScale, 1 / Main.ScreenScale), SpriteEffects.None, 0f);
                }

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: Main.Camera?.Transform, samplerState: SamplerState.PointClamp);
            }

            return Buffers[Buffers.Count - 1];
        }
        public void DrawToMap(string Map, MapRender MR) => MapPasses[Map].DrawToTarget(MR);

        public void AddMap(string MapName, int Index, MapPass MP)
        {
            MP.Parent = this;
            MapPasses.Add(MapName, MP);

            Buffers.Add(new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440));
        }

        public MapPass Get(string MapName) => MapPasses[MapName];
    }
}