
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using static Flipsider.TileManager;

namespace Flipsider
{
    public static class PropInteractions
    {
       public static void BlobInteractable()
       {
            Debug.Write("GraydeeIsDumb");
       }

        public static void UpdatePropInteractions()
        {
            foreach(PropInfo PI in props)
            {
                if((Main.MouseScreen.ToVector2() - PI.position).Length() < PI.interactRange)
                {
                    if(Keyboard.GetState().IsKeyDown(Keys.E))
                    PI.tileInteraction?.Invoke();
                }
            }
        }

    }
}
