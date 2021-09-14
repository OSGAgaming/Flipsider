

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlipEngine
{
    public struct HitBoxInfo
    {
        public Rectangle box;
        public bool Hittable;

        public Action<HitBox>? OnHit;

        public HitBoxGrouping Grouping;
        public HitBoxInfo(Rectangle box, bool Hittable, Action<HitBox>? OnHit = null, HitBoxGrouping Grouping = default)
        {
            this.box = box; this.Hittable = Hittable; this.OnHit = OnHit; this.OnHit = OnHit; this.Grouping = Grouping;
        }

        public static HitBoxInfo InActive = new HitBoxInfo(Rectangle.Empty, false);
    }
    public enum HitBoxGrouping
    {
        Default,
        Friendly,
        Hostile
    }
    public class HitBox : IEntityModifier, ILayeredComponent
    {
        public bool isColliding;

        public List<HitBoxInfo> HitBoxes = new List<HitBoxInfo>();

        private event Action? HitBoxGeneration;

        internal LivingEntity LE;

        public int Layer { get; set; }

        public void Update(in Entity entity)
        {
            HitBoxes.Clear();

            HitBoxGeneration?.Invoke();
            HitBoxGeneration = null;

            foreach (Chunk chunk in FlipGame.World.tileManager.chunks)
            {
                if (chunk.Active)
                {
                    foreach (HitBox hitBox in chunk.HitBoxes.HitBoxes)
                    {
                        foreach (HitBoxInfo hitBoxInfo in hitBox.HitBoxes)
                        {
                            foreach (HitBoxInfo hitBoxInfo2 in HitBoxes)
                            {
                                if (hitBoxInfo.box.Intersects(hitBoxInfo2.box) && !hitBox.LE.Equals(LE))
                                {
                                    OnCollide(hitBoxInfo2, hitBoxInfo, hitBox);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerateHitbox(Rectangle box, bool canTakeDamage, Action<HitBox> action)
        {
            HitBoxGeneration += () => HitBoxes.Add(new HitBoxInfo(box, canTakeDamage, action));
        }
        public void OnCollide(HitBoxInfo sender, HitBoxInfo receiverBox, HitBox receiver)
        {
            isColliding = true;
            if(receiverBox.Hittable)
            sender.OnHit?.Invoke(receiver);
        }
        public void Dispose()
        {
            LE.Chunk.HitBoxes.HitBoxes.Remove(this);
            Debug.WriteLine(FlipGame.layerHandler.Layers[Layer].Drawables.Remove(this));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*Layer = LayerHandler.CurrentLayer;
            foreach (HitBoxInfo hitBoxInfo in HitBoxes)
            {
                 if (hitBoxInfo.box != Rectangle.Empty)
                Utils.DrawRectangle(hitBoxInfo.box, Color.White, 1f);
            }*/
        }

        public HitBox(LivingEntity LE)
        {
            this.LE = LE;
            LE.Chunk.HitBoxes.HitBoxes.Add(this);
            FlipGame.AutoAppendToLayer(this);
        }
    }
}

