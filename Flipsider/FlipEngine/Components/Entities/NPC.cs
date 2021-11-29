
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public class DamageTextHandler : ILoadUpdate
    {
        public DamageTextHandler()
        {
            FlipE.Updateables.Add(this);
        }

        private readonly List<DamageText> DT = new List<DamageText>();

        public void AddDT(Vector2 position, int Damage)
=>
            DT.Add(new DamageText(position, Damage));
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (DamageText text in DT)
            {
                if (text != null)
                    text.Draw(spriteBatch);
            }
        }
        public void Update()
        {
            Dispose();
            foreach (DamageText text in DT)
            {
                if (text != null)
                    text.Update();
            }
        }
        public void Dispose()
        {
            for (int i = 0; i < DT.Count; i++)
            {
                if (DT[i].timeLeft <= 0)
                {
                    DT.RemoveAt(i);
                }
            } 
        }

        private class DamageText : IComponent
        {
            public Vector2 position;
            public string Text;
            public int timeLeft = 60;
            public float alpha = 1;
            public Vector2 velocity;
            public float rotation;
            public float rotationSpeed;
            public void Draw(SpriteBatch spriteBatch)
            {
                Utils.DrawText(Text, Color.Red * alpha, position, rotation);
            }
            public void Update()
            {
                rotation += rotationSpeed;
                velocity *= 0.98f;
                position += velocity;
                alpha *= 0.98f;
                timeLeft--;
            }
            public DamageText(Vector2 position, int Damage)
            {
                rotationSpeed = FlipE.rand.NextFloat(-0.01f, 0.01f);
                velocity = new Vector2(0,-1).RotatedBy(FlipE.rand.NextFloat(-1));
                this.position = position;
                Text = Damage.ToString();
            }
        }
    }
    public abstract class NPC : LivingEntity
    {
        public static DamageTextHandler DTH = new DamageTextHandler();
        public int life;
        public int maxLife;
        public int damage;
        public int IFrames;
        public static int GlobalIFrames = 10;
        protected virtual void Jump(Vector2 direction, bool hasToBeOnGround = true)
        {
            if (hasToBeOnGround ? onGround : true)
                velocity += direction;
        }
        public struct NPCInfo
        {
            public Texture2D icon;
            public Type type;
            public NPCInfo(Texture2D icon, Type type)
            {
                this.icon = icon;
                this.type = type;
            }
        }
        public static NPCInfo[] NPCTypes = new NPCInfo[0];
        public float percentLife => life / (float)maxLife;

        public bool hostile;
        protected virtual void OnCollision(HitBox hitbox) { }

        public static NPC SpawnNPC(Vector2 position, Type type)
        {
            NPC NPC = Activator.CreateInstance(type) as NPC ?? throw new InvalidOperationException("Type wasn't an NPC");
            NPC.Layer = LayerHandler.CurrentLayer;
            NPC.Active = true;
            NPC.SetDefaults();
            NPC.Position = position;
            return NPC;
        }

        protected override void PreAI()
        {
            if (IFrames > 0)
                IFrames--;

            if (life <= 0)
            {
                Dispose();
                Position.Y--;
            }
            else
            {
                GetEntityModifier<HitBox>().GenerateHitbox(CollisionFrame, true, OnCollision);
            }
        }
        public static void SpawnNPC<T>(Vector2 position) where T : NPC, new()
        {
            T NPC = new T
            {
                Layer = LayerHandler.CurrentLayer
            };                             
            FlipGame.AppendToLayer(NPC);
            NPC.SetDefaults();
            NPC.Position = position;
        }

        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(GetType().FullName ?? "Entity");
            binaryWriter.Write(Position.X);
            binaryWriter.Write(Position.Y);
        }

        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            string typeName = binaryReader.ReadString();

                Type? type = Type.GetType(typeName);
                Vector2 position = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
                if (type != null)
                {
                    return SpawnNPC(position, type);
                }
            return base.Deserialize(stream);
        }

        protected virtual void SetDefaults() { }

        public void TakeDamage(int amount)
        {
            if (IFrames == 0 && amount != 0)
            {
                life -= amount;
                DTH.AddDT(Center, amount);
                IFrames = GlobalIFrames;
            }
        }

        public static Type? SelectedNPCType;

        public override void Draw(SpriteBatch spriteBatch)
        {
            PreDraw(spriteBatch);
            spriteBatch.Draw(Texture, Center, frame, Color.White, 0f, frame.Size.ToVector2() / 2, 1f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }

    }
}
