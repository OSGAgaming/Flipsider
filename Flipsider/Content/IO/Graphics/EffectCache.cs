using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class EffectCache
    {
        public static Effect FGWaterMap;
        public static void LoadEffects(ContentManager content)
        {
            FGWaterMap = content.Load<Effect>(@"Effect/FGWaterMap");

        }
    }
}
