using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider.Weapons
{
    internal class ShortSword : Sword
    {
        public override Texture2D swordSheet => TextureCache.GreenSlime;

        public ShortSword() : base(5, 108, 2)
        {
            SetInventoryIcon(TextureCache.GreenSlime);
        }
        Player player => Main.player;
        protected override void OnActivation()
        {
            if (player.onGround)
            {
                player.isAttacking = true;
                player.frameY = 0;
                ComboLag = 109;
                if(comboState == 1 || comboState == 0)
                {
                    Camera.screenShake += 2;
                }
            }
        }
        public override void UpdateActive()
        {

            if (player.isAttacking)
            {
                player.velocity.X *= 0.6f;

                switch (comboState)
                {
                    case 0:
                        player.isAttacking = !player.Animate(5, 6, 48, 4, false);
                        break;
                    case 1:
                        player.isAttacking = !player.Animate(5, 6, 48, 5, false);
                        break;
                    case 2:
                        player.isAttacking = !player.Animate(5, 11, 48, 6, false);
                        if(activeTimeLeft == delay - 30)
                        {
                            Camera.screenShake += 10;
                        }
                        break;
                }
            }
            if(!player.isAttacking)
            {
                activeTimeLeft = 0;
            }
        }
        public int ComboLag;
        public override void Update()
        {
            if (ComboLag > 0) ComboLag--;

            if (ComboLag == 0) comboState = 0;

        }
        public override void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            base.DrawInventory(spriteBatch, pos);
        }
    }

    public class ExampleProj : Projectile
    {
        public static Texture2D icon = TextureCache.Blob;
        protected override void SetDefaults()
        {
            damage = 50;
            width = 20;
            hostile = false;
            framewidth = width;
            height = 20;
            texture = TextureCache.magicPixel;
            Collides = true;
            noGravity = true;
            gravity = 0.1f;
            friction = 0.98f;
            noAirResistance = true;
            TileCollide = true;
        }

        protected override void OnAI()
        {
            Constraints();
            Animate(5, 1, 20, 0);
        }
    }
}
