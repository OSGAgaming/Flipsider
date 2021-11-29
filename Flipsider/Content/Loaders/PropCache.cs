using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class PropCache : ILoadable
    {
        public void Load()
        {
            AutoloadTextures.ExecuteWithAllPaths(Utils.AssetDirectory + "/Props", (s) =>
            {
                string PropName = s.Replace(@"\", "_").Replace("Props_", "");
                PropManager.AddPropType(PropName, AutoloadTextures.Assets[s]);

                Debug.WriteLine(PropName);
            });
        }
    }
}
