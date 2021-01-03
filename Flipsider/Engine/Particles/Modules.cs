using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Flipsider.Engine.Maths;

namespace Flipsider.Engine.Particles
{
    public class SetPosition : IParticleModifier
    {
        private Vector2 _pos;

        public SetPosition(Vector2 position)
        {
            _pos = position;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Center = _pos;
        }
    }

    public class ModifyPosition : IParticleModifier
    {
        private Vector2 _amount;

        public ModifyPosition(Vector2 amount)
        {
            _amount = amount;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Center += _amount;
        }
    }
    public class FloatUp : IParticleModifier
    {
        private readonly float _speed;
        private readonly float _friction;
        public FloatUp(float speed, float friction)
        {
            _speed = speed;
            _friction = friction;
        }
        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Velocity.Y -= _speed;
            particles[index].Velocity.X *= _friction;
        }
    }

    public class SetVelocity : IParticleModifier
    {
        private Vector2 _velocity;

        public SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Velocity = _velocity;
        }
    }
    public class SetRandomVelocity : IParticleModifier
    {
        private readonly float _speed;
        private readonly Random _random;
        private float _angle;
        public SetRandomVelocity(float speed, Random random)
        {
            _speed = speed;
            _random = random;
        }

        public void Invoke(Particle[] particles, int index)
        {
            _angle = _random.NextFloat(6.28f);
            particles[index].Velocity = new Vector2((float)Math.Sin(_angle), (float)Math.Cos(_angle)) * _speed;
        }
    }

    public class SetColor : IParticleModifier
    {
        private Color _c;

        public SetColor(Color color)
        {
            _c = color;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Color = _c;
        }
    }

    public class SetColorBetweenTwoColours : IParticleModifier
    {
        private Color _c;
        private Color _c2;
        private readonly Random _random;

        public SetColorBetweenTwoColours(Color color1, Color color2, Random random)
        {
            _c = color1;
            _c2 = color2;
            _random = random;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Color = Color.Lerp(_c, _c2, _random.NextFloat(1f));
        }
    }

    public class SetScale : IParticleModifier
    {
        private readonly float _s;

        public SetScale(float scale)
        {
            _s = scale;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Scale = _s;
        }
    }

    public class SetLifetime : IParticleModifier
    {
        private readonly float _age;

        public SetLifetime(float age)
        {
            _age = age;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Lifetime = _age;
        }
    }

    public class SetTexture : IParticleModifier
    {
        private readonly Texture2D _t;

        public SetTexture(Texture2D tex)
        {
            _t = tex;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Texture = _t;
        }
    }

    public class ModifyVelocity : IParticleModifier
    {
        private Vector2 _acceleration;

        public ModifyVelocity(Vector2 acceleration)
        {
            _acceleration = acceleration;
        }

        public void Invoke(Particle[] particles, int index)
        {
            particles[index].Velocity += _acceleration;
        }
    }

    public class Turn : IParticleModifier
    {
        private readonly float _turnRadians;

        public Turn(float rads)
        {
            _turnRadians = rads;
        }

        public void Invoke(Particle[] particles, int index)
        {
            Vector2 v = particles[index].Velocity;
            float speed = v.Length();
            float angle = (float)Math.Atan2(v.Y, v.X);
            angle += _turnRadians * Time.DeltaT;
            particles[index].Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
        }
    }

    public class OpacityOverLifetime : IParticleModifier
    {
        private readonly EaseFunction _interp;

        public OpacityOverLifetime(EaseFunction interpolation)
        {
            _interp = interpolation;
        }

        public void Invoke(Particle[] particles, int index)
        {
            new OpacityOverLifetime(EaseFunction.ReverseLinear);

            particles[index].Opacity = _interp.Ease(particles[index].Age / particles[index].Lifetime);
        }
    }

    public class FollowEntity : IParticleModifier
    {
        private readonly Entity _entity;
        private readonly float _speed;
        private readonly bool _useVel;

        public FollowEntity(Entity entity, float speed, bool changeVelocity = true)
        {
            _entity = entity;
            _speed = speed;
            _useVel = changeVelocity;
        }

        public void Invoke(Particle[] particles, int index)
        {
            if (_entity == null) return;
            //TODO: Check if the entity is dead or inactive? depends on our entity system.

            Vector2 between = new Vector2(_entity.position.X + _entity.width * 0.5f, _entity.position.Y + _entity.height * 0.5f);
            between.Normalize();
            between *= _speed;

            if (_useVel)
            {
                particles[index].Velocity += between * _speed;
            }
            else
            {
                particles[index].Center += between * _speed;
            }
        }
    }

    public class GroupModifier : IParticleModifier
    {
        public List<IParticleModifier> Modifiers;

        public GroupModifier(params IParticleModifier[] modifiers)
        {
            Modifiers = new List<IParticleModifier>();
            Modifiers.AddRange(modifiers);
        }

        public void Invoke(Particle[] particles, int index)
        {
            for (int i = 0; i < Modifiers.Count; i++)
            {
                Modifiers[i].Invoke(particles, index);
            }
        }
    }

    public class ConditionalModifier : IParticleModifier
    {
        private readonly IParticleModifier _true;
        private readonly IParticleModifier _false;
        private readonly Func<Particle[], int, bool> _condition;

        public ConditionalModifier(IParticleModifier t, IParticleModifier f, Func<Particle[], int, bool> cond)
        {
            _true = t;
            _false = f;
            _condition = cond;
        }

        public void Invoke(Particle[] particles, int index)
        {
            if (_condition.Invoke(particles, index))
            {
                _true.Invoke(particles, index);
                return;
            }

            _false.Invoke(particles, index);
        }
    }
}
