
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropInteraction;
using static Flipsider.PropManager;

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
