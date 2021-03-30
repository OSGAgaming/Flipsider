using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public Camera? mainCamera;
        public bool RenderingWater { get; set; }
        public float ScreenScale
        {
            get
            {
                if (mainCamera != null)
                    return mainCamera.scale;
                return 1;
            }
        }
        public Vector2 ScreenSize => graphics?.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
        public Renderer(Game game)
        {
            instance = game;
            graphics = new GraphicsDeviceManager(instance)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            graphics.ApplyChanges();
            mainCamera = new Camera();
            renderTarget = new RenderTarget2D(graphics?.GraphicsDevice, 2560, 1440);
            spriteBatch = new SpriteBatch(graphics?.GraphicsDevice);
        }

        public void Load()
        {
            if (instance != null)
                lighting = new Lighting(instance.Content, 0.7f);
        }
        public void Draw()
        {
            if (graphics != null)
            {
                graphics?.GraphicsDevice.Clear(Color.CornflowerBlue);
                graphics?.GraphicsDevice.SetRenderTarget(renderTarget);
                Main.graphics?.GraphicsDevice.Clear(Color.Transparent);
                Render();

                graphics?.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: mainCamera?.Transform, samplerState: SamplerState.PointClamp);
                lighting?.ApplyShader();
                lighting?.Maps.OrderedShaderPass();
                if (renderTarget != null)
                    PrintRenderTarget(renderTarget);
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
                Rectangle frame = new Rectangle(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);
                spriteBatch.Draw(renderTarget, Main.mainCamera.CamPos, frame, Color.White, 0f, Vector2.Zero,new Vector2(1/ScreenScale, 1/ScreenScale), SpriteEffects.None, 0f);
            }
        }
        public void Render()
        {
            //Todo: Events
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            if (graphics != null)
            lighting?.Maps.OrderedRenderPass(spriteBatch, graphics.GraphicsDevice);
            Main.instance.sceneManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void RenderUI()
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, Vector2.One * 10, Color.Aqua);
            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance.Components[i].active = true;
                UIScreenManager.Instance?.Components[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            UIScreenManager.Instance?.DrawOnScreen();
            //debuganthinghere
            // Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, new Vector2(10, 36), Color.Black);
        }
    }
}
