using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Scenes
{
    public class CityArea : Scene
    {
        public CityArea()
        {
        }

        public override void Update()
        {
            Main.Renderer.RenderPrimitiveMode = true;

            if (CutsceneManager.Instance != null)
            {
                if (!CutsceneManager.Instance.IsPlayingCutscene)
                {
                    Main.Gamecamera.Offset -= Main.Gamecamera.Offset / 16f;
                }
            }

            foreach (IUpdate updateable in FlipE.AlwaysUpdate.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }

            foreach (IUpdate updateable in FlipE.Updateables.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }

            Main.World.GlobalParticles.Update();
            CutsceneManager.Instance?.Update();

            Scene? scene = SceneManager.Instance.Scene;

            if (scene != null)
            {
                if (scene.Name == Name)
                {
                    Main.Renderer.Destination = new Rectangle(0, 0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y);
                }
            }
        }

        public override void OnActivate()
        {
            Main.World.SetSkybox(new CitySkybox());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.World.Draw(spriteBatch);
        }
    }
}
