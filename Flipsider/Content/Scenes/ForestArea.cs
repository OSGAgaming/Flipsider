
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
    public class ForestArea : Scene
    {
        public ParticleSystem ForestAreaParticles;

        public ForestArea()
        {
            ForestAreaParticles = new ParticleSystem(200);
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
            ForestAreaParticles.Position = Vector2.Zero;
            ForestAreaParticles.SpawnRate = 10f;
            ForestAreaParticles.WorldSpace = true;
            ForestAreaParticles.SpawnModules.Add(new SetTexture(TextureCache.pixel));
            ForestAreaParticles.SpawnModules.Add(new SetScale(2f));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionToEntity(Main.player));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionRand(new Vector2(-1000, 1000), new Vector2(600, 600), Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetColorBetweenTwoColours(Color.White, Color.Lime, Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetVelocity(Vector2.UnitY * -100f));
            ForestAreaParticles.SpawnModules.Add(new SetLifetime(10f));
            ForestAreaParticles.SpawnModules.Add(new SetLightIntensityRand(0.2f, 1f));
            ForestAreaParticles.SpawnModules.Add(new SetParalaxRand(.8f, -.8f));
            ForestAreaParticles.UpdateModules.Add(new OpacityOverLifetime(EaseFunction.ReverseLinear));
            ForestAreaParticles.UpdateModules.Add(new TurnRand(-.5f, .5f));

            Main.World.SetSkybox(new ForestSkybox());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.World.Draw(spriteBatch);

            ForestAreaParticles.Update();
            ForestAreaParticles.Draw(spriteBatch);
        }
    }
}
