
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public partial class RigidBody : IEntityModifier
    {
        public Entity BindableEntity;

        public float Mass;

        public const float Gravity = 0.08f;

        public const float Inertia = 3f;
        public void Update(in Entity entity)
        {
           if(entity is LivingEntity)
           {
                LivingEntity Entity = (LivingEntity)entity;
                if (!Entity.onGround)
                {
                    Entity.velocity.Y += Gravity * Mass * Time.DeltaVar(120);
                }
                if(Entity.onSlope)
                {
                    if (!GameInput.Instance.CurrentKeyState.IsKeyDown(Keys.W))
                    {
                        Entity.Position.Y += Inertia*Mass * Time.DeltaVar(120);
                    }
                    else
                    {
                       
                    }
                }
           }
        }
        public int Layer { get; set; }
        public RigidBody(Entity entity, float mass)
        {
            Mass = mass;
            BindableEntity = entity;
        }
    }
}
