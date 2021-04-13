using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider
{
    public delegate void LayerEventDelegate(World world, SpriteBatch spriteBatch);
    public class Renderer
    {
        internal Lighting? lighting;
        public GraphicsDeviceManager? graphics;
        public RenderTarget2D? renderTarget;
        public Game? instance;
        public SpriteBatch spriteBatch;
        public GameCamera? mainCamera;
        public bool RenderPrimitiveMode { get; set; }
        public float ScreenScale
        {
            get
            {
                if (mainCamera != null)
                    return mainCamera.Scale;
                return 1;
            }
        }

        public readonly int UpScale = 1;
        public Renderer(Game game)
        {
            instance = game;
            graphics = new GraphicsDeviceManager(instance)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            graphics.ApplyChanges();
            mainCamera = new GameCamera();
            renderTarget = new RenderTarget2D(graphics?.GraphicsDevice, 2560 / UpScale, 1440 / UpScale);
            spriteBatch = new SpriteBatch(graphics?.GraphicsDevice);
        }

        public void Load()
        {
            if (instance != null)
                lighting = new Lighting(instance.Content);
        }
        public void Draw()
        {
            if (graphics != null)
            {
                graphics?.GraphicsDevice.SetRenderTarget(renderTarget);
                graphics?.GraphicsDevice.Clear(Color.Transparent);
                Render();
                graphics?.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: mainCamera?.Transform, samplerState: SamplerState.PointClamp);
                if (renderTarget != null && lighting != null)
                {
                    RenderTarget2D r = lighting.Maps.OrderedShaderPass(spriteBatch, renderTarget);
                    PrintRenderTarget(r);
                }
                spriteBatch.End();
            }
        }
        public Vector2 PreferredSize
        {
            get
            {
                if (graphics != null)
                    return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                return Vector2.Zero;
            }
        }

        public void PrintRenderTarget(RenderTarget2D renderTarget)
        {
            if (graphics != null)
            {
                Rectangle frame = new Rectangle(0, 0, 2560 / UpScale, 1440 / UpScale);
                spriteBatch.Draw(renderTarget, Main.Camera.Position, frame, Color.White, 0f, Vector2.Zero, 1 / ScreenScale, SpriteEffects.None, 0f);
            }
        }
        public void Render()
        {
            //Todo: Events
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.Camera.Transform, samplerState: SamplerState.PointClamp);
            Main.instance.sceneManager.Draw(spriteBatch);
            if (graphics != null)
                 lighting?.Maps.OrderedRenderPass(spriteBatch, graphics.GraphicsDevice);
            spriteBatch.End();
        }
        //need to move somewhere sensible
        public void RenderUI()
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, Utils.ActualScreenSize + new Vector2(-80,-30), Color.Aqua);
            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance.Components[i].active = true;
                UIScreenManager.Instance?.Components[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.Camera.Transform, samplerState: SamplerState.PointClamp);
            UIScreenManager.Instance?.DrawOnScreen();
        }
    }
}
