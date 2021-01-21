namespace Flipsider
{
    public class EntityManager : Manager<Entity>
    {
        public override void Update()
        {
            if (!Main.Editor.IsActive)
            {
                foreach (Entity foo in Components.ToArray())
                {
                    // if (foo != null)
                    foo.Update();
                }
            }
            else
            {
                foreach (Entity foo in Components.ToArray())
                {
                    //    if (foo != null)
                    //    foo.UpdateInEditor();
                }
            }
        }
    }
}
