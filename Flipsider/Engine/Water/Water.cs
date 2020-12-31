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

#nullable disable
// TODO fix this..
namespace Flipsider
{
    public class Water
    {
        public static List<Water> WaterBodies = new List<Water>();
        int accuracy;
        Vector2[] Pos;
        Vector2[] accel;
        Vector2[] vel;
        Vector2[] targetHeight;
        public Rectangle frame;
        float[] disLeft;
        float[] disRight;
        float dampening;
        float constant;
        float viscosity;

        public void SetDampeningTo(float dampening) => this.dampening = dampening;
        public void SetFrame(Rectangle vertices) => frame = vertices;

        public Water(Rectangle _frame)
        {
            SetFrame(_frame);
            WaterBodies.Add(this);
        }
        public void Update()
        {
            foreach (Entity entity in Main.entities)
            {
                float preContact = entity.CollisionFrame.Bottom - entity.velocity.Y * entity.velocity.Y;
                if (preContact < frame.Y && entity.Wet)
                    SplashPerc((entity.Center.X - frame.X) / frame.Width, entity.velocity.Y * 4);
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
                }
                if (i < accuracy)
                {
                    disRight[i] = (Pos[i].Y - Pos[i + 1].Y) * viscosity;
                    vel[i + 1].Y += disRight[i];
                    Pos[i + 1].Y += disRight[i];
                }
            }
        }

        public void Render()
        {
            for (int i = 0; i < accuracy; i++)
            {
                DrawMethods.DrawLine(Pos[i], Pos[i] - new Vector2(0,Pos[i].Y - frame.Bottom),Color.Blue*0.5f);
            }
         }
        public void Splash(int index, float speed) => vel[index].Y = speed;
        public void SplashPerc(float perc, float speed) => vel[(int)(MathHelper.Clamp(perc, 0, 1) * accuracy)].Y = speed;

        public void Initialize()
        {
            viscosity = 0.09f;
            dampening = 0.05f;
            constant = 50;
            accuracy = 100;
            disLeft = new float[accuracy + 1];
            disRight = new float[accuracy + 1];
            Pos = new Vector2[accuracy + 1];
            vel = new Vector2[accuracy + 1];
            accel = new Vector2[accuracy + 1];
            targetHeight = new Vector2[accuracy + 1];
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].Y = frame.Y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].X = i * (frame.Width / accuracy) + frame.X;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].Y = frame.Y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].X = i * (frame.Width / accuracy) + frame.X;
            }
        }
    }
}
