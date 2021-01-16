using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.GUI.HUD;
using Flipsider.Scenes;
using Flipsider.Engine.Particles;
using Flipsider.Engine;
using Flipsider.Engine.Audio;
using Flipsider.Engine.Input;
using Flipsider.GUI.TilePlacementGUI;
using static Flipsider.TileManager;
using System.Reflection;
using System.Linq;
using System.Threading;
using Flipsider.Engine.Interfaces;

#nullable disable
// TODO fix this..
namespace Flipsider
{
    public class Water : IComponent,ILayeredComponent
    {
        protected Primitive PrimitiveInstance;
        protected Primitive PrimitiveInstanceDamp;
        public int accuracy;
        public Vector2[] Pos;
        public Vector2[] PosDampened;
        private Vector2[] accel;
        private Vector2[] vel;
        private Vector2[] targetHeight;
        public Rectangle frame;
        private float[] disLeft;
        private float[] disRight;
        private float dampening;
        private float constant;
        private float viscosity;
        public Color color = Color.DarkSeaGreen;
        public void SetDampeningTo(float dampening) => this.dampening = dampening;
        public void SetFrame(Rectangle vertices) => frame = vertices;
        public int Layer { get; set; }
        public void Dispose()
        {
            Utils.layerHandler.Layers[Layer].PrimitiveDrawables.Remove(this);
            PrimitiveInstance.Dispose();
            PrimitiveInstanceDamp.Dispose();
        }
        public Water(Rectangle _frame)
        {
            SetFrame(_frame);
            Initialize();
            PrimitiveInstance = new WaterPrimtives(this);
            PrimitiveInstanceDamp = new WaterPrimitivesDampened(this);
            Layer = LayerHandler.CurrentLayer;
            Main.Primitives.AddComponent(PrimitiveInstance);
            Main.Primitives.AddComponent(PrimitiveInstanceDamp);
            Main.AppendPrimitiveToLayer(this);
        }
        public void Update()
        {
            foreach (Entity entity in Main.CurrentWorld.entityManager.Components)
            {
                float preContact = entity.CollisionFrame.Bottom - entity.velocity.Y * entity.velocity.Y;
                if (preContact < frame.Y && entity.Wet && frame.Intersects(entity.CollisionFrame))
                    SplashPerc((entity.Center.X - frame.X) / frame.Width, new Vector2(entity.velocity.X/4, entity.velocity.Y*2));
                if(entity.Wet && frame.Intersects(entity.CollisionFrame))
                {
                    Vector2 v = new Vector2(Math.Abs(entity.velocity.X), Math.Abs(entity.velocity.Y));
                    SplashPerc((entity.Center.X - frame.X + entity.velocity.X*12) / frame.Width, new Vector2(0,-v.X/4 * Main.rand.NextFloat(1,1.5f)));
                    SplashPerc((entity.Center.X - frame.X - entity.velocity.X * 12) / frame.Width, new Vector2(0, v.X/7 * Main.rand.NextFloat(1, 1.5f)));
                }
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                
                Pos[i].X += vel[i].X;
                Pos[i].Y += vel[i].Y;
                vel[i].X += accel[i].X;
                vel[i].Y += accel[i].Y;
                accel[i].X = (targetHeight[i].X - Pos[i].X) / constant - (vel[i].X * dampening);
                accel[i].Y = (targetHeight[i].Y - Pos[i].Y) / constant - (vel[i].Y * dampening);
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                if (i > 0)
                {
                    disLeft[i] = (Pos[i].Y - Pos[i - 1].Y) * viscosity;
                    vel[i - 1].Y += disLeft[i];
                    Pos[i - 1].Y += disLeft[i];
                    disLeft[i] = (Pos[i].X - Pos[i - 1].X) * viscosity;
                    vel[i - 1].X += disLeft[i];
                    Pos[i - 1].X += disLeft[i];
                }
                if (i < accuracy)
                {
                    disRight[i] = (Pos[i].Y - Pos[i + 1].Y) * viscosity;
                    vel[i + 1].Y += disRight[i];
                    Pos[i + 1].Y += disRight[i];
                    disLeft[i] = (Pos[i].X - Pos[i + 1].X) * viscosity;
                    vel[i + 1].X += disLeft[i];
                    Pos[i + 1].X += disLeft[i];
                }
                float dY = Pos[i].Y - frame.Top;
                PosDampened[i].X = Pos[i].X;
                PosDampened[i].Y = frame.Top + dY * 0.5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PrimitiveInstanceDamp.Draw(spriteBatch);
            PrimitiveInstance.Draw(spriteBatch);
        }
        public void Splash(int index, float speed) => vel[index].Y = speed;
        public void SplashPerc(float perc, Vector2 speed) => vel[(int)(MathHelper.Clamp(perc, 0, 1) * accuracy)] += speed;

        public void Initialize()
        {
            viscosity = 0.09f;
            dampening = 0.05f;
            constant = 50;
            accuracy = 100;
            disLeft = new float[accuracy + 1];
            disRight = new float[accuracy + 1];
            Pos = new Vector2[accuracy + 1];
            PosDampened = new Vector2[accuracy + 1];
            vel = new Vector2[accuracy + 1];
            accel = new Vector2[accuracy + 1];
            targetHeight = new Vector2[accuracy + 1];
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].Y = frame.Y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].X = i * (frame.Width / (float)accuracy) + frame.X;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].Y = frame.Y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].X = i * (frame.Width / (float)accuracy) + frame.X;
            }
        }
    }
}
