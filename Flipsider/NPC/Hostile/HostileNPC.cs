using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.NPC.Hostile
{
    class HostileNPC : Entity
    {
        public int life;
        public int lifeMax;
        public float percentLife => life / (float)lifeMax;

        public int damage;

        public virtual void AI() { }

        public virtual void HitEnemy(int damage)
        {
            life -= damage;
            if (life <= 0) Kill();
        }

        protected sealed override void OnUpdate()
        {
            //TODO: Put all the physics stuff here OS!!!
            AI();
        }
    }
}
