
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public struct HitBoxInfo
    {
        public Rectangle box;
        public bool canTakeDamage;
        public int damage;

        public HitBoxInfo(Rectangle box, bool canTakeDamage, int damage)
        {
            this.box = box; this.canTakeDamage = canTakeDamage; this.damage = damage;
        }

        public static HitBoxInfo InActive = new HitBoxInfo(Rectangle.Empty, false, 0);
    }
    public class HitBox : IEntityModifier
    {
        public bool isColliding;

        public List<HitBoxInfo> HitBoxes = new List<HitBoxInfo>();

        private event Action? HitBoxGeneration;

        internal LivingEntity LE;
        public void Update(in Entity entity)
        {
            HitBoxes.Clear();

            HitBoxGeneration?.Invoke();
            HitBoxGeneration = null;

            foreach (Chunk chunk in Main.CurrentWorld.tileManager.chunks)
            {
                if (chunk.Active)
                {
                    foreach (HitBox hitBox in chunk.HitBoxes.HitBoxes)
                    {
                        foreach (HitBoxInfo hitBoxInfo in hitBox.HitBoxes)
                        {
                            foreach (HitBoxInfo hitBoxInfo2 in HitBoxes)
                            {
                                if(hitBoxInfo.box.Intersects(hitBoxInfo2.box) && !hitBox.LE.Equals(LE))
                                {
                                    OnCollide(hitBoxInfo, hitBoxInfo2);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerateHitbox(Rectangle box, bool canTakeDamage, int dmg)
        {
            HitBoxGeneration += () => HitBoxes.Add(new HitBoxInfo(box,canTakeDamage,dmg));
        }

        public void OnCollide(HitBoxInfo sender, HitBoxInfo receiver)
        {
            isColliding = true;
            if(receiver.canTakeDamage)
            {
                LE.position.Y -= sender.damage;
            }
        }
        public void Dispose()
        {
            LE.Chunk.HitBoxes.HitBoxes.Remove(this);
        }
        public HitBox(LivingEntity LE)
        {
            this.LE = LE;
            LE.Chunk.HitBoxes.HitBoxes.Add(this);
        }
    }
}
