
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using FlipEngine;

namespace Flipsider.Scenes
{
    public class WhiteFadeInFadeOut : SceneTransition
    {
        public override float Length => 180;

        public override float SwitchPoint => 90;

        public override void DrawUI(SpriteBatch spriteBatch, float transitionProgress)
        {
            Utils.DrawBoxFill(new Rectangle(0,0, (int)Main.ActualScreenSize.X, (int)Main.ActualScreenSize.Y), Color.White * (float)Math.Sin(transitionProgress * (float)Math.PI), 0f);
        }
    }
}
