
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider.Scenes
{
    public class ForestArea : Scene
    {
        public ParticleSystem ForestAreaParticles;

        public ForestArea()
        {
            ForestAreaParticles = new ParticleSystem();
        }

        float Test;

        public override void Update()
        {
            Main.Renderer.RenderPrimitiveMode = true;

            if(Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Test += 2.6f;

            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Test -= 2.6f;
            }

            Main.lighting.Maps.DrawToMap("SunMap", (sb) =>
            {
                sb.Draw(TextureCache.pixel, new Vector2(0, -Test), new Rectangle(0,0,10000,Utils.BOTTOM), Color.Orange * 0.2f, 0.2f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            });

            if(Utils.JustClicked && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                SceneManager.Instance.SetNextScene(new CityArea(), new WhiteFadeInFadeOut(), true);
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
            ForestAreaParticles.Position = Vector2.Zero;
            ForestAreaParticles.SpawnRate = 10f;
            ForestAreaParticles.WorldSpace = true;
            ForestAreaParticles.SpawnModules.Add(new SetTexture(TextureCache.pixel));
            ForestAreaParticles.SpawnModules.Add(new SetScale(2f));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionToEntity(Main.player));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionRand(new Vector2(-1000, 1000), new Vector2(600, 600), FlipE.rand));
            ForestAreaParticles.SpawnModules.Add(new SetColorBetweenTwoColours(Color.White, Color.Lime, FlipE.rand));
            ForestAreaParticles.SpawnModules.Add(new SetVelocity(Vector2.UnitY * -100f));
            ForestAreaParticles.SpawnModules.Add(new SetLifetime(10f));
            ForestAreaParticles.SpawnModules.Add(new SetLightIntensityRand(0.2f, 1f));
            ForestAreaParticles.SpawnModules.Add(new SetParalaxRand(.8f, -.8f));
            ForestAreaParticles.UpdateModules.Add(new OpacityOverLifetime(EaseFunction.ReverseLinear));
            ForestAreaParticles.UpdateModules.Add(new TurnRand(-.5f, .5f));

            //Main.World.SetSkybox(new ForestSkybox());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.World.Draw(spriteBatch);

            ForestAreaParticles.Update();
            ForestAreaParticles.Draw(spriteBatch);
        }
    }
}
