using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FlipEngine
{
    public static class FlipE
    {
        public static Random rand = new Random();
        public static GameTime gameTime = new GameTime();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static SpriteFont font;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static List<IUpdate> Updateables = new List<IUpdate>();
        public static List<IUpdateGT> GameTimeUpdateables = new List<IUpdateGT>();
        public static List<IUpdate> AlwaysUpdate = new List<IUpdate>();

        public static event Action? LoadQueue;

        public static void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("FlipFont");

            FlipTextureCache.LoadTextures(Content);
            EffectCache.LoadEffects(Content);
            Fonts.LoadFonts(Content);

            foreach (Type type in Utils.GetInheritedClasses(typeof(ILoadable))) 
                (Activator.CreateInstance(type) as ILoadable)?.Load();
            

            Updateables.AddRange(Utils.GetInheritedClasses<ILoadUpdate>()
                .Where(x => !(x is IAlwaysUpdate))
                .Cast<IUpdate>().ToList());

            GameTimeUpdateables.AddRange(Utils.GetInheritedClasses<ILoadUpdateGT>()
                .Cast<IUpdateGT>().ToList());

            AlwaysUpdate.AddRange(Utils.GetInheritedClasses<ILoadAlwaysUpdate>()
                .Cast<IUpdate>().ToList());

            foreach (Type type in Utils.GetInheritedClasses(typeof(UIScreen)))
            {
                UIScreen? Screen = Activator.CreateInstance(type) as UIScreen;
                if (Screen != null)
                {
                    if (Screen is ModeScreen s)
                    {
                        if (s.Mode != Mode.None)
                            EditorModeGUI.AddScreen(s);
                        else BottomModeSelectPreview.Screen = s as LayerScreen;
                    }
                }
            }
        }

        public static void Update(GameTime gameTime)
        {
            FlipE.gameTime = gameTime;

            foreach (IUpdateGT gt in GameTimeUpdateables.ToArray()) gt.Update(gameTime);
            foreach (IUpdate gt in Updateables.ToArray()) gt.Update();
            foreach (IAlwaysUpdate gt in AlwaysUpdate.ToArray()) gt.Update();

            LoadQueue?.Invoke();
            LoadQueue = null;
        }
    }
}
