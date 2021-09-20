using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace FlipEngine
{
    public delegate void LayerEventDelegate(World world, SpriteBatch spriteBatch);
    public class Renderer
    {
        public Manager<Primitive> Primitives;

        public event Action? TargetCalls;

        /// <summary>
        /// Make sure this is in line with your preffered aspect ratio
        /// </summary>
        protected virtual Point MaxResolution { get; set; } = new Point(2560, 1440);
        public Rectangle Destination { get; set; } = new Rectangle(0,0, (int)FlipGame.ActualScreenSize.X, (int)FlipGame.ActualScreenSize.Y);

        internal Lighting? Lighting { get; set; }
        public GraphicsDeviceManager? Graphics { get; set; }

        public RenderTarget2D? PixelationTarget { get; set; }
        public RenderTarget2D? RenderTarget { get; set; }
        public RenderTarget2D? PostProcessedTarget { get; set; }
        /// <summary>
        /// For Convenience. Can be Misc render calls
        /// </summary>
        public RenderTarget2D? UITarget { get; set; }

        public Game? Instance { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public CameraTransform? MainCamera { get; set; }
        public bool RenderUITarget { get; set; } = true;
        public bool RenderPrimitiveMode { get; set; }
        public float PixelationUpscale => 2f;
        public int TargetQueueIndex;

        public float ScreenScale
        {
            get
            {
                if (MainCamera != null)
                    return MainCamera.Scale;
                return 1;
            }
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

        Vector2 FPSPosition => Utils.ActualScreenSize + new Vector2(-80, -30);

        public Renderer(Game game)
        {
            Instance = game;

            Graphics = new GraphicsDeviceManager(Instance)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();
            MainCamera = new CameraTransform();

            int X = MaxResolution.X;
            int Y = MaxResolution.Y;

            RenderTarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            PostProcessedTarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            UITarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            PixelationTarget = new RenderTarget2D(Graphics?.GraphicsDevice, X, Y);
            SpriteBatch = new SpriteBatch(Graphics?.GraphicsDevice);
            Primitives = new Manager<Primitive>();
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
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (target != null)
            {
                Graphics?.GraphicsDevice.SetRenderTarget(target);
                Graphics?.GraphicsDevice.Clear(Color.Transparent);

                Call.Invoke(SpriteBatch);
            }

            SpriteBatch.End();
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
                if (RenderTarget != null)
                {
                    Graphics?.GraphicsDevice.SetRenderTarget(RenderTarget);
                    Graphics?.GraphicsDevice.Clear(Color.Transparent);

                    RenderPreEffect(SpriteBatch);
                }

                TargetCalls?.Invoke();
                TargetCalls = null;

                //Doesnt have to be UI. Can be anything Misc that doesnt use the Main Transform
                Graphics?.GraphicsDevice.SetRenderTarget(UITarget);
                Graphics?.GraphicsDevice.Clear(Color.Transparent);

                if(RenderUITarget) RenderUI(SpriteBatch);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                if (RenderTarget != null && Lighting != null && UITarget != null)
                {
                    RenderTarget2D r = Lighting.Maps.OrderedShaderPass(SpriteBatch, RenderTarget);

                    Graphics?.GraphicsDevice.SetRenderTarget(PostProcessedTarget);
                    Graphics?.GraphicsDevice.Clear(Color.Transparent);

                    PrintRenderTarget(r);
                }

                SpriteBatch.End();

                Graphics?.GraphicsDevice.SetRenderTarget(PixelationTarget);
                Graphics?.GraphicsDevice.Clear(Color.Transparent);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

                SpriteBatch.Draw(PostProcessedTarget, 
                    new Rectangle(new Point(0,0), (FlipGame.ActualScreenSize / (PixelationUpscale * ScreenScale)).ToPoint()), 
                    new Rectangle(0, 0, (int)FlipGame.ActualScreenSize.X, (int)FlipGame.ActualScreenSize.Y), Color.White);

                SpriteBatch.End();

                Graphics?.GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

                SpriteBatch.Draw(PixelationTarget, 
                    new Rectangle(Destination.Location, (Destination.Size.ToVector2() * ScreenScale).ToPoint()), 
                    new Rectangle(0, 0, (int)(FlipGame.ActualScreenSize.X / PixelationUpscale), (int)(FlipGame.ActualScreenSize.Y / PixelationUpscale)), Color.White);

                SpriteBatch.Draw(UITarget, Destination, new Rectangle(0, 0, (int)FlipGame.ActualScreenSize.X, (int)FlipGame.ActualScreenSize.Y), Color.White);

                RenderToScreen(SpriteBatch);

                SpriteBatch.End();

            }
        }

        public void PrintRenderTarget(RenderTarget2D RT)
        {
            if (Graphics != null)
            {
                Rectangle frame = new Rectangle(0, 0, MaxResolution.X, MaxResolution.Y);
                SpriteBatch.Draw(RT, FlipGame.Camera.TransformPosition, frame, Color.White, 0f, Vector2.Zero, 1 / ScreenScale, SpriteEffects.None, 0f);
            }
        }

        public virtual void RenderPreEffect(SpriteBatch sb) 
        {
            sb.Begin(SpriteSortMode.Immediate, null, transformMatrix: FlipGame.Camera.Transform, samplerState: SamplerState.PointClamp);

            SceneManager.Instance.Draw(SpriteBatch);

            if (Graphics != null) Lighting?.Maps.OrderedRenderPass(SpriteBatch, Graphics.GraphicsDevice);

            sb.End();
        }
        public virtual void RenderUI(SpriteBatch sb) 
        {
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            FPS.Instance.DrawFps(FlipGame.spriteBatch, FlipE.font, FPSPosition, Color.Aqua);

            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance?.Components[i].Draw(SpriteBatch);
            }

            SceneManager.Instance.DrawTransitionUI(SpriteBatch);

            sb.End();

            sb.Begin(SpriteSortMode.FrontToBack, null, transformMatrix: FlipGame.Camera.Transform, samplerState: SamplerState.PointClamp);

            UIScreenManager.Instance?.DrawOnScreen();

            sb.End();
        }
        public virtual void RenderToScreen(SpriteBatch sb)
        {
            UIScreenManager.Instance?.DrawDirectOnScreen(sb);
        }
    }
}
