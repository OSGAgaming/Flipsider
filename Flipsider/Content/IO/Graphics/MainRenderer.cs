using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider
{
    public class MainRenderer : Renderer
    {
        Vector2 FPSPosition => Utils.ActualScreenSize + new Vector2(-80, -30);
        public MainRenderer(Game game) : base(game) { }

        public override void RenderPreEffect(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.Camera.Transform, samplerState: SamplerState.PointClamp);

            Main.instance.sceneManager.Draw(SpriteBatch);

            if (Graphics != null)
                Lighting?.Maps.OrderedRenderPass(SpriteBatch, Graphics.GraphicsDevice);

            sb.End();
        }

        public override void RenderUI(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, FPSPosition, Color.Aqua);

            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance.Components[i].active = true;
                UIScreenManager.Instance?.Components[i].Draw(SpriteBatch);
            }

            sb.End();

            sb.Begin(SpriteSortMode.FrontToBack, null, transformMatrix: Main.Camera.Transform, samplerState: SamplerState.PointClamp);

            UIScreenManager.Instance?.DrawOnScreen();

            sb.End();
        }

        public override void RenderToScreen(SpriteBatch sb)
        {
            UIScreenManager.Instance?.DrawDirectOnScreen(sb);
        }

        public Vector2 PreferredSize
        {
            get
            {
                if (Graphics != null)
                    return new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
                return Vector2.Zero;
            }
        }
    }
}
