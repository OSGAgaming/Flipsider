using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    // TODO dude.
#nullable disable
    public static class Fonts
    {
        public static BitmapFont Calibri;

        public static void LoadFonts(ContentManager content)
        {
            Calibri = content.Load<BitmapFont>("Font/BMFCalibri");
        }
    }
}
