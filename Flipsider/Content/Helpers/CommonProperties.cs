using FlipEngine;
using Flipsider.Content.IO.Graphics;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    // TODO holy shit this hurts
#nullable disable
    public partial class Main : FlipGame
    {
        private void GetAllTypes()
        {
            Type[] NPCTypes = Utils.GetInheritedClasses(typeof(NPC));

            NPC.NPCTypes = new NPC.NPCInfo[NPCTypes.Length];
            for (int i = 0; i < NPCTypes.Length; i++)
                NPC.NPCTypes[i].type = NPCTypes[i];

            Type[] StoreableTypes = Utils.GetInheritedClasses(typeof(IStoreable));

            Item.ItemTypes = new Type[StoreableTypes.Length];
            for (int i = 0; i < StoreableTypes.Length; i++)
                Item.ItemTypes[i] = StoreableTypes[i];

            Type[] PropEntityTypes = Utils.GetInheritedClasses(typeof(PropEntity));

            for (int i = 0; i < PropEntityTypes.Length; i++)
            {
                PropEntity PE = (PropEntity)Activator.CreateInstance(PropEntityTypes[i]);
                PropEntity.keyValuePairs.Add(PE.Prop, PE);
            }
        }
        public static EditorMode Editor => EditorMode.Instance;
        public static float targetScale => Gamecamera.targetScale;
        public static FlipsiderWorld FlipWorld => (World as FlipsiderWorld);
        public static Player player => FlipWorld.MainPlayer;
        public static GameCamera Gamecamera => Camera as GameCamera;

    }
}
