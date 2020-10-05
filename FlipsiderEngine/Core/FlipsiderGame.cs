using Flipsider.Assets;
using Flipsider.Assets.Repositories;
using Flipsider.Core.Collections;
using Flipsider.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;

namespace Flipsider.Core
{
    public delegate void EnterWorldDelegate(World? oldWorld, World? newWorld);
    
    /// <summary>
    /// The game.
    /// </summary>
    public sealed class FlipsiderGame : Game
    {
        private FlipsiderGame(string[] args)
        {
            EntryArgs = new ReadOnlyCollection<string>(args);

            Graphics = new GraphicsDeviceManager(this);

            Window.AllowUserResizing = false;
            Window.Title = "Flipsider";

            IsMouseVisible = true;
            IsFixedTimeStep = false;

            ContentPacks = new AssemblyLoadContext(null, true);
        }

        // We know better than the compiler here. Everything here is set before it can possibly be accessed. So we're safe to set them to "null!".
        /// <summary>
        /// The game's graphics device manager.
        /// </summary>
        public GraphicsDeviceManager Graphics { get; }
        /// <summary>
        /// The arguments that were passed through the command line to this program.
        /// </summary>
        public ReadOnlyCollection<string> EntryArgs { get; }
        private SafeSpriteBatch sb = null!;

        /// <summary>
        /// The <see cref="Game"/> instance. Be very careful to not leave any dangling references in this object, or unloading content packs will result in a fatal crash.
        /// </summary>
        public static FlipsiderGame GameInstance { get; private set; } = null!;
        /// <summary>
        /// The camera.
        /// </summary>
        public static Camera CurrentCamera { get; set; } = new Camera();
        /// <summary>
        /// The current world, if any.
        /// </summary>
        public static World? CurrentWorld
        {
            get => currentWorld;
            set
            {
                OnEnterWorld?.Invoke(currentWorld, value);
                currentWorld?.Unload();
                currentWorld = value;
                currentWorld?.Load();
            }
        }
        private static World? currentWorld;

        /// <summary>
        /// Called just before unloading an old world and loading a new one.
        /// </summary>
        public static event EnterWorldDelegate? OnEnterWorld;

        /// <summary>
        /// Objects that should have their <see cref="IUpdated.Update"/> function called every update.
        /// </summary>
        public static OrderedSet<IUpdated> Updateables { get; } = new OrderedSet<IUpdated>();
        /// <summary>
        /// Objects that should have their <see cref="IDrawn.Draw(SafeSpriteBatch)"/> function called every update.
        /// </summary>
        public static OrderedSet<IDrawn> Drawables { get; } = new OrderedSet<IDrawn>();

        internal static GameTime time = new GameTime();

        /// <summary>
        /// All content packs that are currently loaded.
        /// </summary>
        public AssemblyLoadContext ContentPacks { get; private set; }

        static void Main(string[] args)
        {
            using (GameInstance = new FlipsiderGame(args))
                GameInstance.Run();
        }

        protected override void Initialize()
        {
            // TODO: Make a main menu. We shouldn't ALWAYS be in a world.
            base.Initialize();

            LoadContentPacks();

            CurrentWorld = new World();
            Updateables.Add(CurrentWorld);
            Drawables.Add(CurrentWorld);
        }

        private void LoadContentPacks()
        {
            // Get current dir
            string dir = Path.GetDirectoryName(Environment.CurrentDirectory) ?? throw new Exception("Catastrophic failure. Program was run not run within a subdirectory.");

            if (ContentPacks.Assemblies.Any())
                ReloadContent();

            foreach (string file in Directory.EnumerateFiles(dir, "*.dll"))
                try
                {
                    // Don't reference ourselves...
                    if (Path.GetFileName(file) != Path.GetFileName(Environment.CurrentDirectory))
                    {
                        AssemblyName asm = AssemblyName.GetAssemblyName(file);
                        ContentPacks.LoadFromAssemblyName(asm);
                    }
                }
                catch { }

            foreach (Assembly asm in ContentPacks.Assemblies)
            {
                try
                {
                    Action? entryPoint = null;
                    foreach (Type type in asm.GetTypes())
                    {
                        foreach (MethodInfo method in type.GetMethods(BindingFlags.Static))
                        {
                            if (method.Name != "LoadPack" || method.ReturnType != typeof(void) || method.ContainsGenericParameters)
                                continue;
                            var parameters = method.GetParameters();
                            if (parameters.Length > 0)
                                continue;
                            if (entryPoint != null)
                            {
                                throw new Exception("Content pack cannot have more than one entry point.");
                            }
                            entryPoint = (Action)Delegate.CreateDelegate(typeof(Action), method);
                        }
                    }
                    entryPoint?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.Warn($"Error while trying to load content pack for {asm}: {e}");
                }
            }
        }

        private void ReloadContent()
        {
            // Force clear them :)
            (Updateables as ICollection<IUpdated>).Clear();
            (Drawables as ICollection<IDrawn>).Clear();
            CurrentWorld = null;
            OnEnterWorld = null;

            Asms_Reload();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Asms_Reload()
        {
            var asmRef = new WeakReference(ContentPacks, true);
            ContentPacks = new AssemblyLoadContext(null, true);

            System.Threading.Thread.MemoryBarrier();
            for (int i = 0; asmRef.IsAlive; i++)
            {
                if (i > 20)
                {
                    string loadedAsms = string.Join(", ", (asmRef.Target as AssemblyLoadContext)!.Assemblies);
                    Logger.Fatal("One or more content packs refused to unload. Currently loaded: " + loadedAsms);
                    break;
                }
                // Beg the assemblies to finalize and cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected override void LoadContent()
        {
            sb = new SafeSpriteBatch();
        }

        protected override void Update(GameTime gameTime)
        {
            time = gameTime;

            foreach (var item in Updateables)
                try
                {
                    item.Update();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"{item} threw an exception. {e}");
                }
        }

        protected override void Draw(GameTime gameTime)
        {
            time = gameTime;

            GraphicsDevice.Clear(Color.Black);

            foreach (var item in Drawables)
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

            sb.End();
        }
    }
}
