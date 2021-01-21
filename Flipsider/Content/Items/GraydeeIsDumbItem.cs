namespace Flipsider
{
    public class GraydeeIsDumbItem : Item
    {
        protected override void OnLoad()
        {
            SetInventoryIcon(TextureCache.Birb);
            texture = TextureCache.BigBusStop;
            maxStack = 5;
        }
    }
    public class GraydeeIsDumbItem2 : Item
    {
        protected override void OnLoad()
        {
            SetInventoryIcon(TextureCache.Blob);
            texture = TextureCache.BigBusStop;
            maxStack = 5;
        }
    }
}
