using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

#nullable disable
// TODO fix this..
namespace FlipEngine
{
    [Serializable]
    public class Water : Entity
    {
        [NonSerialized]
        protected Primitive PrimitiveInstance;
        [NonSerialized]
        protected Primitive PrimitiveInstanceDamp;
        public int accuracy;
        [NonSerialized]
        public Vector2[] Pos;
        [NonSerialized]
        public Vector2[] PosDampened;
        [NonSerialized]
        private Vector2[] accel;
        [NonSerialized]
        private Vector2[] vel;
        [NonSerialized]
        private Vector2[] targetHeight;
        public RectangleF _frame;
        public Rectangle frame => _frame.ToR();
        private float[] disLeft;
        private float[] disRight;
        private float dampening;
        private float constant;
        private float viscosity;
        [NonSerialized]
        public Color color = Color.LightBlue;
        public void SetDampeningTo(float dampening) => this.dampening = dampening;
        public void SetFrame(RectangleF vertices) => _frame = vertices;
        public override void Dispose()
        {
            Utils.layerHandler.Layers[Layer].PrimitiveDrawables.Remove(this);
            PrimitiveInstance.Dispose();
            PrimitiveInstanceDamp.Dispose();
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(frame);
            binaryWriter.Write(Layer);
            base.Serialize(stream);
        }
        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            Rectangle Frame = binaryReader.ReadRect();
            int Layer = binaryReader.ReadInt32();
            Water water = new Water(new RectangleF(Frame.Location.ToVector2(),Frame.Size.ToVector2()), Layer);
            Main.WaterBodies.Add(water);
            return water;
        }
        public Water(RectangleF _frame) : base()
        {
            SetFrame(_frame);
            Position = _frame.TL;
            Layer = LayerHandler.CurrentLayer;
            Initialize();
            PrimitiveInstance = new WaterPrimtives(this);
            PrimitiveInstanceDamp = new WaterPrimitivesDampened(this);
            Main.Renderer.Primitives.AddComponent(PrimitiveInstance);
            Main.Renderer.Primitives.AddComponent(PrimitiveInstanceDamp);
            Main.AppendPrimitiveToLayer(this);
        }
        public Water(RectangleF _frame, int Layer) : base()
        {
            SetFrame(_frame);
            Position = _frame.TL;
            this.Layer = Layer;
            Initialize();
            PrimitiveInstance = new WaterPrimtives(this);
            PrimitiveInstanceDamp = new WaterPrimitivesDampened(this);
            Main.Renderer.Primitives.AddComponent(PrimitiveInstance);
            Main.Renderer.Primitives.AddComponent(PrimitiveInstanceDamp);
            Main.AppendPrimitiveToLayer(this);
        }
        public Water()
        {

        }
        protected override void OnUpdate()
        {
            foreach (Chunk chunk in Main.World.tileManager.chunks)
            {
                if (chunk.Active)
                {
                    foreach (Entity Entity in chunk.Entities)
                    {
                        if (Entity is LivingEntity)
                        {
                            LivingEntity entity = (LivingEntity)Entity;
                            float preContact = entity.CollisionFrame.Bottom - entity.velocity.Y * entity.velocity.Y;
                            if (preContact < frame.Y && entity.Wet && frame.Intersects(entity.CollisionFrame))
                                SplashPerc((entity.Center.X - frame.X) / frame.Width, new Vector2(entity.velocity.X / 4, entity.velocity.Y * 2));
                            if (entity.Wet && frame.Intersects(entity.CollisionFrame))
                            {
                                Vector2 v = new Vector2(Math.Abs(entity.velocity.X), Math.Abs(entity.velocity.Y));
                                SplashPerc((entity.Center.X - frame.X + entity.velocity.X * 12) / frame.Width, new Vector2(0, -v.X / 4 * FlipE.rand.NextFloat(1, 1.5f)));
                                SplashPerc((entity.Center.X - frame.X - entity.velocity.X * 12) / frame.Width, new Vector2(0, v.X / 7 * FlipE.rand.NextFloat(1, 1.5f)));
                            }
                        }
                    }
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

        public override void Draw(SpriteBatch spriteBatch)
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
