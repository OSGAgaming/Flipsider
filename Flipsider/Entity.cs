using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider
{
    public abstract class Entity
    {

        public int width;
        public Texture2D texture;

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
            int fluff = 1;
            int res = TileManager.tileRes;
            for (int i = (int)position.X / res - (width / res + 2); i < (int)position.X / res + (width / res + 2); i++)
            {
                for (int j = (int)position.Y / res - (height / res + 2); j < (int)position.Y / res + (height / res + 2); j++)
                {
                    if (i >= 0 && j >= 0 && i < Main.MaxTilesX && j < Main.MaxTilesY)
                    {
                        if (Main.tiles[i, j].active)
                        {
                            Rectangle tileRect = new Rectangle(i * res - fluff, j * res - fluff, res + fluff, res + fluff);
                            if (CollisionFrame.Intersects(tileRect))
                            {
                                Vector2 positionPreCollision = position - velocity * Time.DeltaVar(120);
                                isColliding = true;
                                if (positionPreCollision.X + width - fluff <= tileRect.X || positionPreCollision.X + fluff >= tileRect.X + res)
                                {
                                    if (positionPreCollision.Y + height - fluff > tileRect.Y)
                                    {
                                        if (positionPreCollision.X >= tileRect.X + res * .5f - fluff)
                                            position.X -= position.X - (tileRect.X + res + fluff);
                                        else if (positionPreCollision.X + width < tileRect.X + fluff + res * .5f)
                                            position.X -= (position.X + width) - tileRect.X - fluff;
                                        velocity.X = 0;
                                    }
                                }
                                else
                                {
                                    if (positionPreCollision.X + width - fluff > tileRect.X + fluff)
                                    {
                                        if (positionPreCollision.Y >= tileRect.Y + res * .5f)
                                            position.Y -= position.Y - (tileRect.Y + res + fluff);
                                        else if (positionPreCollision.Y + height < tileRect.Y + fluff + res * .5f)
                                        {
                                            position.Y -= (position.Y + height) - tileRect.Y - fluff;
                                            onGround = true;
                                        }
                                        velocity.Y = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public Entity()
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
            if (frameCounter % per == 0)
            {
                frameY++;
                if (frameY >= noOfFrames)
                    frameY = 0;
            }
            frame = new Rectangle(framewidth * column, frameY * frameHeight, framewidth, frameHeight);
        }
        public void Update()
        {
            frameCounter++;
            ResetVars();
            velocity.Y += gravity * Time.DeltaVar(120);
            velocity *= airResistance;
            position += velocity * Time.DeltaVar(120);
            OnUpdate();
            PreAI();
            AI();
            PostAI();
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
