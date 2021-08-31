
using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
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
            Main.renderer.RenderPrimitiveMode = true;

            if (CutsceneManager.Instance != null)
            {
                if (!CutsceneManager.Instance.IsPlayingCutscene)
                {
                    Main.Camera.Offset -= Main.Camera.Offset / 16f;
                }
            }

            foreach (IUpdate updateable in Main.UpdateablesOffScreen.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }

            foreach (IUpdate updateable in Main.Updateables.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }

            Main.World.GlobalParticles.Update();
            CutsceneManager.Instance?.Update();

            Scene? scene = Main.instance.sceneManager.Scene;

            if (scene != null)
            {
                if (scene.Name == Name)
                {
                    Main.renderer.Destination = new Rectangle(0, 0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y);
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
