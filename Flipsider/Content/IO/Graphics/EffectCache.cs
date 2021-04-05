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
        public static Effect LeavesEffect;
        public static Effect Pixelation;
        public static Effect BloomEffect;
        public static void LoadEffects(ContentManager content)
        {
            FGWaterMap = content.Load<Effect>(@"Effect/FGWaterMap");
            LeavesEffect = content.Load<Effect>(@"Effect/LeavesEffect");
            Pixelation = content.Load<Effect>(@"Effect/Pixelation");
            BloomEffect = content.Load<Effect>(@"Effect/BloomEffect");
        }
    }
}
