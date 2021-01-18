using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public abstract class Entity : IComponent,ILayeredComponent
    {
        public int Layer { get; set; }
        public int width;
        public Texture2D? texture;
        public Polygon CollideBox => new Polygon(new Vector2[] {new Vector2(-width/2,-height/2), new Vector2(width / 2, -height / 2), new Vector2(width / 2, height / 2), new Vector2(-width / 2, height / 2) },Center);
        public int frameY;
        public int framewidth;
        public int frameCounter;
        public Rectangle frame;
        public bool noGravity;
        public bool noAirResistance;
        public float friction = 0.982f;
        public void ResetVars()
        {
            noGravity = false;
            onGround = false;
            isColliding = false;
        }

        protected virtual void AI() { }
        protected virtual void PreAI() { }
        protected virtual void PostAI() { }
        protected virtual void PreDraw() { }
        public Rectangle CollisionFrame => new Rectangle((int)position.X, (int)position.Y + maxHeight - height, width, height);
        public Rectangle PreCollisionFrame => new Rectangle((int)oldPosition.X, (int)oldPosition.Y + maxHeight - height, width, height);
        public bool Wet
        {
            get;
            private set;
        }

        public int height;
        public int maxHeight;
        public bool Collides;
        public bool onGround;
        public Vector2 position;
        public int spriteDirection;

        public Vector2 velocity;

        public Vector2 oldPosition;

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
        protected internal virtual void Initialize() { }
        public float acceleration = 0.11f;
        public float gravity = 0.8f;

        public Vector2 airResistance = new Vector2(0.985f, 0.999f);

        public void TileCollisions(World world)
        {
            int res = world.TileRes;
            for (int i = (int)position.X / res - (width / res + 2); i < (int)position.X / res + (width / res + 2); i++)
            {
                for (int j = (int)position.Y / res - (height / res + 2); j < (int)position.Y / res + (height / res + 2); j++)
                {
                    if (world.IsTileInBounds(i,j))
                    {
                       Rectangle tileRect = new Rectangle(i * res, j * res, res, res);

                       CollisionInfo collisionInfo = Collision.AABBResolve(CollisionFrame,PreCollisionFrame,tileRect);
                                
                            if(collisionInfo.AABB == Bound.Top)
                            {
                                velocity.Y = 0;
                                onGround = true;
                            }
                            if (collisionInfo.AABB == Bound.Bottom)
                            {
                                velocity.Y = 0;
                            }
                            if (collisionInfo.AABB == Bound.Left)
                            {
                                velocity.X = 0;
                            }
                            if (collisionInfo.AABB == Bound.Right)
                            {
                                velocity.X = 0;
                            }
                            isColliding = true;
                            position += collisionInfo.d;
                    }
                }
            }

        }

        protected Entity()
        {
            oldPositions = new Vector2[TrailLength];
            Init();
        }

        private bool active = true;
        public void Kill()
        {
            active = false;
            Main.CurrentWorld.entityManager.RemoveComponent(this);
            OnKill();
        }

        public void Spawn()
        {
            if (Main.CurrentWorld != null)
                Main.CurrentWorld.entityManager.AddComponent(this);
        }

        public bool Animate(int per, int noOfFrames, int frameHeight, int column = 0, bool repeat = true, int startingFrame = 0)
        {
            bool hasEnded = false;
            if (frameY >= noOfFrames && repeat)
            {
                frameY = startingFrame;
            }
            if (per != 0)
            {
                if (frameCounter % per == 0)
                {
                    frameY++;
                    if (frameY >= noOfFrames)
                    {
                        if (repeat)
                        {
                            frameY = startingFrame;
                        }
                        else
                        {
                            hasEnded = true;
                            frameY = noOfFrames - 1;
                        }
                    }
                    
                }
            }
            frame = new Rectangle(framewidth * column, frameY * frameHeight, framewidth, frameHeight);
            return hasEnded;
        }
        public bool isNPC;
        public void Constraints()
        {
            position.Y = MathHelper.Clamp(position.Y, -200, Utils.BOTTOM - maxHeight);
            position.X = MathHelper.Clamp(position.X, 0, 100000);
            if (Bottom >= Utils.BOTTOM)
            {
                onGround = true;
                velocity.Y = 0;
            }
        }
        public void Update()
        {
            frameCounter++;
            oldVelocity = velocity;
            ResetVars();
            if (!noGravity)
                velocity.Y += gravity * Time.DeltaVar(120);
            if (!noAirResistance)
                velocity *= airResistance;
            oldPosition = position;
            position += velocity * Time.DeltaVar(120);
            OnUpdate();
            PreAI();
            AI();
            PostAI();
            if (Collides)
                TileCollisions(Main.CurrentWorld);
            if (isColliding)
                OnCollide();
            Wet = false;
            foreach (Water water in Main.CurrentWorld.WaterBodies.Components)
            {
                if (water.frame.Intersects(CollisionFrame))
                    Wet = true;
            }
            UpdateTrailCache();
        }

        public void Init()
        {

            Initialize();
            Spawn();
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnCollide() { }

        protected virtual void OnKill() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            PreDraw();
            spriteBatch.Draw(texture, position, frame, Color.White);
        }

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
