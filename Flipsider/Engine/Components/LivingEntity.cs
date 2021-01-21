using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public abstract partial class LivingEntity : Entity
    {
        public int frameY;
        public int framewidth;
        public int frameCounter;
        public Rectangle frame;
        //In Collideables
        public bool noAirResistance;
        public bool noGravity;
        public float friction = 0.982f;
        public void ResetVars()
        {
            noGravity = false;
        }

        protected virtual void AI() { }
        protected virtual void PreAI() { }
        protected virtual void PostAI() { }
        protected virtual void PreDraw() { }

        public bool Wet
        {
            get;
            private set;
        }

        public bool Collides;
        public bool onGround;
        public int spriteDirection;

        public Vector2 velocity;

        public Vector2 oldVelocity;

        public Vector2[] oldPositions;

        protected internal virtual int TrailLength => 5;

        private int a;
        public void UpdateTrailCache()
        {
            if (active)
            {
                a++;
                if (a > TrailLength - 1)
                    a = 0;
                oldPositions[a] = position;
            }
        }

        public bool isColliding;
        public float Bottom
        {
            get => position.Y + maxHeight;
            set => position.Y = value - maxHeight;
        }
        public float acceleration = 0.08f;
        public float gravity = 0.8f;

        public Vector2 airResistance = new Vector2(0.985f, 0.999f);

        protected LivingEntity() : base()
        {
            oldPositions = new Vector2[TrailLength];
            AddModule(new Collideable(this, false));
        }

        private bool active = true;
        public void Kill()
        {
            active = false;
            Main.CurrentWorld.entityManager.RemoveComponent(this);
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            OnKill();
        }
        public bool isNPC;
        public override void Update()
        {
            oldVelocity = velocity;
            oldPosition = position;
            frameCounter++;
            ResetVars();
            if (!noGravity && !onGround)
                velocity.Y += gravity * Time.DeltaVar(120);
            if (!noAirResistance)
                velocity *= airResistance;
            PreUpdate();
            OnUpdate();
            position += velocity * Time.DeltaVar(120);
            UpdateEntityModifiers();
            Constraints();
            PostUpdate();
            PreAI();
            AI();
            PostAI();
            Wet = false;
            foreach (Water water in Main.CurrentWorld.WaterBodies.Components)
            {
                if (water.frame.Intersects(CollisionFrame))
                    Wet = true;
            }
            UpdateTrailCache();
        }
        public bool isDraggable = true; //dragging stuff
        public bool isDragging = false;
        public bool mousePressed = false; //this is so you have to click on it, instead of overlapping with click
        public Vector2 offsetFromMouseWhileDragging;
        public bool mouseOverlap //touch up please
        {
            get
            {
                return CollisionFrame.Contains(Main.MouseScreen.ToVector2());
            }
        }
        protected virtual void OnUpdateInEditor() { }

        protected virtual void OnCollide() { }

        protected virtual void OnKill() { }

        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
            }
            set
            {
                position = new Vector2(value.X - width * 0.5f, value.Y - height * 0.5f);
            }
        }
    }
}
