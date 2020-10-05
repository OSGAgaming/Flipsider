using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public abstract class Entity
    {

        public int width;
        public Texture2D? texture;

        public int frameY;
        public int framewidth;
        public int frameCounter;
        public Rectangle frame;

        public float friction = 0.982f;
        public void ResetVars()
        {
            onGround = false;
            isColliding = false;
        }

        protected virtual void AI() { }
        protected virtual void PreAI() { }
        protected virtual void PostAI() { }
        protected virtual void PreDraw() { }
        public Rectangle CollisionFrame => new Rectangle((int)position.X, (int)position.Y + maxHeight - height, width, height);

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
        int a;
        public void UpdateTrailCache()
        {
            a++;
            if (a > TrailLength - 1)
                a = 0;
            oldPositions[a] = position;
        }

        public bool isColliding;
        public float Bottom
        {
            get => position.Y + maxHeight;
            set => position.Y = value - maxHeight;
        }
        protected internal virtual void Initialize() { }
        public float acceleration = 0.07f;
        public float gravity = 0.08f;

        public Vector2 airResistance = new Vector2(0.985f, 0.999f);

        public void TileCollisions()
        {


        }

        protected Entity()
        {
            oldPositions = new Vector2[TrailLength];
            Init();
        }

        public void Kill()
        {
            Main.entities.Remove(this);
        }

        public void Spawn()
        {
            Main.entities.Add(this);
        }

        public void Animate(int per, int noOfFrames, int frameHeight, int column = 0)
        {
            if (frameY >= noOfFrames)
            {
                frameY = 0;
            }
            if (per != 0)
            {
                if (frameCounter % per == 0)
                {
                    frameY++;
                    if (frameY >= noOfFrames)
                        frameY = 0;
                }
            }
            frame = new Rectangle(framewidth * column, frameY * frameHeight, framewidth, frameHeight);
        }
        public void Update()
        {
            frameCounter++;
            ResetVars();
            velocity.Y += gravity * Time.DeltaF * 120;
            velocity *= airResistance;
            position += velocity * Time.DeltaF * 120;
            OnUpdate();
            PreAI();
            AI();
            PostAI();
            Wet = false;
            for (int i = 0; i<Water.WaterBodies.Count; i++)
            {
                if (Water.WaterBodies[i].frame.Intersects(CollisionFrame))
                    Wet = true;
            }
        }

        public void Init()
        {
            Main.entities.Add(this);
            Initialize();
        }

        protected virtual void OnUpdate() { }

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
