using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public delegate void LayerEventDelegate(World world, SpriteBatch spriteBatch);
    public class Renderer
    {
        public event Action? TargetCalls;

        /// <summary>
        /// Make sure this is in line with your preffered aspect ratio
        /// </summary>
        protected virtual Point MaxResolution { get; set; } = new Point(2560, 1440);
        public Rectangle Destination { get; set; } = new Rectangle(0,0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y);

        internal Lighting? Lighting { get; set; }
        public GraphicsDeviceManager? Graphics { get; set; }

        public RenderTarget2D? RenderTarget { get; set; }
        public RenderTarget2D? PostProcessedTarget { get; set; }
        /// <summary>
        /// For Convenience. Can be Misc render calls
        /// </summary>
        public RenderTarget2D? PreUITarget { get; set; }
        public RenderTarget2D? PostUITarget { get; set; }

        public Game? Instance { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public GameCamera? MainCamera { get; set; }
        public bool RenderUITarget { get; set; }
        public bool RenderPrimitiveMode { get; set; }

        //Does not work yet
        public readonly int PixelationUpscale = 1;

        public float ScreenScale
        {
            get
            {
                if (MainCamera != null)
                    return MainCamera.Scale;
                return 1;
            }
        }

        public Renderer(Game game)
        {
            Instance = game;

            Graphics = new GraphicsDeviceManager(Instance)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };

            Graphics.ApplyChanges();
            MainCamera = new GameCamera();

            int X = MaxResolution.X / PixelationUpscale;
            int Y = MaxResolution.Y / PixelationUpscale;

            RenderTarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            PostProcessedTarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            PreUITarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            PostUITarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);

            SpriteBatch = new SpriteBatch(Graphics?.GraphicsDevice);
        }

        public void AddTargetCall(RenderTarget2D target, Action<SpriteBatch> Call)
        {
            TargetCalls += () =>
            {
                DrawToTarget(target, Call);
            };
        }

        public void DrawToTarget(RenderTarget2D? target, Action<SpriteBatch> Call)
        {
            if (target != null)
            {
                Graphics?.GraphicsDevice.SetRenderTarget(target);
                Graphics?.GraphicsDevice.Clear(Color.Transparent);

                Call.Invoke(SpriteBatch);
            }
        }

        public void Load()
        {
            if (Instance != null)
                Lighting = new Lighting(Instance.Content);
        }
        public void Draw()
        {
            if (Graphics != null)
            {
                DrawToTarget(RenderTarget, RenderPreEffect);

                SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                TargetCalls?.Invoke();
                TargetCalls = null;

                SpriteBatch.End();

                //Doesnt have to be UI. Can be anything Misc that doesnt use the Main Transform
                Graphics?.GraphicsDevice.SetRenderTarget(PreUITarget);
                Graphics?.GraphicsDevice.Clear(Color.Transparent);

                if(RenderUITarget) RenderUI(SpriteBatch);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: MainCamera?.Transform, samplerState: SamplerState.PointClamp);

                if (RenderTarget != null && Lighting != null && PreUITarget != null)
                {
                    RenderTarget2D r = Lighting.Maps.OrderedShaderPass(SpriteBatch, RenderTarget);

                    Graphics?.GraphicsDevice.SetRenderTarget(PostProcessedTarget);
                    Graphics?.GraphicsDevice.Clear(Color.Transparent);

                    PrintRenderTarget(r);

                    Graphics?.GraphicsDevice.SetRenderTarget(PostUITarget);
                    Graphics?.GraphicsDevice.Clear(Color.Transparent);

                    PrintRenderTarget(PreUITarget);
                }

                SpriteBatch.End();

                Graphics?.GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                SpriteBatch.Draw(PostProcessedTarget, Destination, new Rectangle(0, 0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y), Color.White);
                SpriteBatch.Draw(PostUITarget, Destination, new Rectangle(0, 0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y), Color.White);

                RenderToScreen(SpriteBatch);

                SpriteBatch.End();

            }
        }

        public void PrintRenderTarget(RenderTarget2D RT)
        {
            if (Graphics != null)
            {
                Rectangle frame = new Rectangle(0, 0, MaxResolution.X / PixelationUpscale, MaxResolution.Y / PixelationUpscale);
                SpriteBatch.Draw(RT, Main.Camera.Position, frame, Color.White, 0f, Vector2.Zero, 1 / ScreenScale, SpriteEffects.None, 0f);
            }
        }

        public virtual void RenderPreEffect(SpriteBatch sb) { }
        public virtual void RenderUI(SpriteBatch sb) { }
        public virtual void RenderToScreen(SpriteBatch sb) { }
    }
}
