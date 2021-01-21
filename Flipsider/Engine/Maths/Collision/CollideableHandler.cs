using System.Collections.Generic;

namespace Flipsider.Engine.Maths
{
    public class CollideableHanlder
    {
        private static readonly int G = 0;
        public List<Collideable> collideables = new List<Collideable>();
        //Implement lol
        public CollideableHanlder()
        {
            // Main.Updateables.Add(this);
        }
        public void RemoveThroughEntity(Entity entity)
        {
            for (int i = 0; i < collideables.Count; i++)
            {
                if (entity.GetHashCode() == collideables[i].BindableEntity.GetHashCode())
                {
                    collideables.RemoveAt(i);
                }
            }
        }
        public void Update()
        {


        }
    }
}
