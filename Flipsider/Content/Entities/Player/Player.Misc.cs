using Flipsider.Engine.Input;
using Flipsider.Weapons;
using Flipsider.Weapons.Ranged.Pistol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;

#nullable enable
// TODO fix this..
namespace Flipsider
{
    public partial class Player : LivingEntity
    {
        internal float TimeOutsideOfCombat = 0f;
    }
}
