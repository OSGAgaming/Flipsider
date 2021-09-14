using FlipEngine;

namespace Flipsider
{
    public class GraydeeIsDumbItem : Item
    {
        protected override void OnLoad()
        {
            SetInventoryIcon(TextureCache.Birb);
            Texture = TextureCache.BigBusStop;
            maxStack = 5;
        }
    }
    public class GraydeeIsDumbItem2 : Item
    {
        protected override void OnLoad()
        {
            SetInventoryIcon(TextureCache.Blob);
            Texture = TextureCache.BigBusStop;
            maxStack = 5;
        }
    }
}
