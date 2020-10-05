using Flipsider.Assets;
using Flipsider.Assets.Repositories;
using Flipsider.Core;
using Flipsider.Core.Collections;
using Flipsider.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Flipsider
{
    public class FlipsiderGame : Game
    {
        private FlipsiderGame()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Graphics.ApplyChanges();
            GameServices = Services;

            Window.AllowUserResizing = false;
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        // We know better than the compiler here. Everything here is set before it can possibly be accessed. So we're safe to set them to "null!".
        private SafeSpriteBatch sb = null!;

        public static Camera CurrentCamera { get; set; } = new Camera();
        public static ReadOnlyCollection<string> EntryArgs { get; private set; } = null!;
        public static GraphicsDeviceManager Graphics { get; private set; } = null!;
        public static IServiceProvider GameServices { get; private set; } = null!;
        public static World? CurrentWorld
        {
            get => currentWorld;
            set
            {
                currentWorld?.Unload();
                currentWorld = value;
                currentWorld?.Load();
            }
        }
        private static World? currentWorld;

        public static OrderedSet<IUpdated> Updateables { get; } = new OrderedSet<IUpdated>();
        public static OrderedSet<IDrawn> Drawables { get; } = new OrderedSet<IDrawn>();

        internal static GameTime time = new GameTime();

        static void Main(string[] args)
        {
            EntryArgs = new ReadOnlyCollection<string>(args);
            using var game = new FlipsiderGame();
            game.Run();
        }

        protected override void Initialize()
        {
            // TODO: Make a main menu. We shouldn't ALWAYS be in a world.
            CurrentWorld = new World();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SafeSpriteBatch();
        }

        protected override void Update(GameTime gameTime)
        {
            time = gameTime;

            foreach (var item in Updateables)
            {
                try
                {
                    item.Update();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"{item} threw an exception. {e}");
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            time = gameTime;

            GraphicsDevice.Clear(Color.Black);

            foreach (var item in Drawables)
            {
                try
                {
                    item.Draw(sb);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"{item} threw an exception. {e}");
                    if (sb.Sb.IsDisposed)
                        // Panic if someone screws up the rest of the spritebatch.
                        throw;
                }
            }

            sb.End();
        }
    }
}
