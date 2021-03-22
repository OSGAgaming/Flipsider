using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider.Weapons
{
    internal class Crowbar : Sword
    {
        public override Texture2D swordSheet => TextureCache.GreenSlime;

        public Crowbar() : base(5, 108, 2)
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
                ComboLag = 20;
                if(comboState == 1 || comboState == 0)
                {
                    Camera.screenShake += 4;
                  //  player.velocity.X += 6f;
                }
            }
        }
        
        Texture2D crowbar => TextureCache.CrowBar;
        public override void UpdateActive()
        {
            if (player.isAttacking)
            {
                
                switch (comboState)
                {
                    case 0:
                        player.isAttacking = !player.Animate(5, 6, 48, 4, false);
                        player.velocity.X += 0.005f * (delay - activeTimeLeft);
                        break;
                    case 1:
                        player.isAttacking = !player.Animate(5, 6, 48, 5, false);
                        player.velocity.X += 0.01f * (delay - activeTimeLeft);
                        break;
                    case 2:
                        player.isAttacking = !player.Animate(5, 11, 48, 6, false);
                        if(activeTimeLeft == delay - 30)
                        {
                            Camera.screenShake += 10;
                        }
                        if(activeTimeLeft >= delay - 30)
                        {
                            player.velocity.X += 0.015f * (delay - activeTimeLeft);
                        }
                        else
                        {
                            player.velocity.X *= 0.5f;
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
            if (ComboLag > 0 && !player.isAttacking) ComboLag--;

            if (ComboLag == 0) comboState = 2;

        }
        public override void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            Rectangle frame = new Rectangle(0, 50, 69, 50);
            spriteBatch.Draw(crowbar,player.position,frame,Color.White);
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
