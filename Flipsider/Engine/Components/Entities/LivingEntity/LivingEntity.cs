using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;

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
        public void ResetVars()
        {
            Wet = false;
            noGravity = false;
        }

        protected virtual void AI() { }
        protected virtual void PreAI() { }
        protected virtual void PostAI() { }
        public bool Wet
        {
            get;
            private set;
        }
        public bool onSlope;
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
            if (Active)
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
            get => position.Y + height;
            set => position.Y = value - height;
        }
        protected override void PostConstructor()
        {
            AddModule("Collision", new Collideable(this, false));
            AddModule("RigidBody", new RigidBody(this, 1f));
            AddModule("Hitbox", new HitBox(this));
        }
        protected LivingEntity() : base()
        {
            oldPositions = new Vector2[TrailLength];
        }
        public override void Dispose()
        {
            Active = false;
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            Chunk.Colliedables.RemoveThroughEntity(this);
            Chunk.HitBoxes.RemoveThroughEntity(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
            OnKill();
        }
        public bool isNPC;
        public bool LoadFlag;
        protected override void OnUpdate()
        {
            oldVelocity = velocity;
            frameCounter++;
            ResetVars();
            PreAI();
            if (Active)
            {
                AI();
                ApplyForces();
                if (!noGravity)
                    UpdateEntityModifier("RigidBody");

                UpdatePosition();

                if (Collides && Active)
                    UpdateEntityModifier("Collision");

                UpdateEntityModifier("Hitbox");

                Constraints();
                PostAI();

                InFrame = ParallaxPosition.X >
                    Utils.SafeBoundX.X - 5 && position.Y >
                    Utils.SafeBoundY.X - 5 && ParallaxPosition.X <
                    Utils.SafeBoundX.Y + 5 && position.Y <
                    Utils.SafeBoundY.Y + 5;

                foreach (Water water in Main.CurrentWorld.WaterBodies.Components)
                {
                    if (water.frame.Intersects(CollisionFrame))
                        Wet = true;
                }
                UpdateTrailCache();
            }
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
        protected virtual void OnCollide() { }

        protected virtual void OnKill() { }


    }
}
