using Flipsider.Core;
using Flipsider.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Flipsider.Core.Collections;

namespace Flipsider.Entities
{
    public class Liquid : Entity, ICollisionObserver
    {
        public Liquid(RectangleF frame, int accuracy = 100)
        {
            OnUpdate += Update;
            Center = frame.Center;
            Size = frame.Size;
            OnSpawn += delegate { InWorld.Collision.AddObserver(this); };
            OnRemove += delegate { InWorld.Collision.RemoveObserver(this); };
            viscosity = 0.09f;
            dampening = 0.05f;
            constant = 50;
            this.accuracy = accuracy;
            disLeft = new float[accuracy + 1];
            disRight = new float[accuracy + 1];
            Pos = new Vector2[accuracy + 1];
            vel = new Vector2[accuracy + 1];
            accel = new Vector2[accuracy + 1];
            targetHeight = new Vector2[accuracy + 1];
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].Y = Bounds.y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                targetHeight[i].X = i * (Bounds.w / accuracy) + Bounds.x;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].Y = Bounds.y;
            }
            for (int i = 0; i < accuracy + 1; i++)
            {
                Pos[i].X = i * (Bounds.w / accuracy) + Bounds.x;
            }
        }

        readonly int accuracy;
        readonly Vector2[] Pos;
        readonly Vector2[] accel;
        readonly Vector2[] vel;
        readonly Vector2[] targetHeight;
        readonly float[] disLeft;
        readonly float[] disRight;

        public float dampening;
        public float constant;
        public float viscosity;

        public Vector2 Size;

        public RectangleF Bounds => new RectangleF { Center = Center, Size = Size };

        public void Splash(int index, float speed) => vel[index].Y = speed;
        public void SplashPerc(float perc, float speed) => vel[(int)(MathHelper.Clamp(perc, 0, 1) * accuracy)].Y = speed;

        protected virtual void Update()
        {
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

            // Remove any entities that are ded
            wet.RemoveWhere(wet =>
            {
                if (!wet.Bounds.Intersects(Bounds))
                {
                    wet.OnExit(this);
                    return true;
                }
                return false;
            });
        }

        private readonly WeakObjectSet<IWettable> wet = new WeakObjectSet<IWettable>();

        void ICollisionObserver.Intersect(ICollideable other)
        {
            if (other is IWettable obj)
            {
                var handle = GCHandle.Alloc(obj, GCHandleType.Weak);
                if (!wet.Contains(obj) && obj.Bounds.Intersects(Bounds))
                {
                    obj.OnEnter(this);
                    SplashPerc((obj.Position.X - Bounds.x) / Bounds.w, obj.Velocity.Y * 4);
                }
                else
                {
                    handle.Free();
                }
            }
        }
    }
}
