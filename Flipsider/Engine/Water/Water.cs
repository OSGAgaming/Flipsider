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

namespace Flipsider
{
    public class Water
    {
        int accuracy;
        float lengthOfScreen = 400;
        Vector2[] DisplacmentFromOriginal;
        Vector2[] Pos;
        Vector2[] accel;
        Vector2[] vel;
        Vector2[] normalPos;
        Vector2[] targetHeight;
        float[] disLeft;
        float[] disRight;
        float dampening;
        float constant;
        float viscosity;
        void setup()
        {
            viscosity = 0.09f;
            dampening = 0.05f;
            constant = 50;
            accuracy = 1000;
            DisplacmentFromOriginal = new Vector2[accuracy + 1];
            disLeft = new float[accuracy + 1];
            disRight = new float[accuracy + 1];
            Pos = new Vector2[accuracy + 1];
            vel = new Vector2[accuracy + 1];
            accel = new Vector2[accuracy + 1];
            normalPos = new Vector2[accuracy + 1];
            targetHeight = new Vector2[accuracy + 1];
            for (int i = 0; i < accuracy + 1; i++)
            {
                normalPos[i].Y = 100;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                normalPos[i].X = i * (lengthOfScreen / accuracy);
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].Y = 100;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].X = i * (lengthOfScreen / accuracy);
            }
        }
        void draw()
        {
 
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].X = (normalPos[i].X + DisplacmentFromOriginal[i].X);
                targetHeight[i].Y = (normalPos[i].Y + DisplacmentFromOriginal[i].Y);
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
        void Splash(int index, float speed)
        {
            vel[index].Y = speed;
        }
    }
}
