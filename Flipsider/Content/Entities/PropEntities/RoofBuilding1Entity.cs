
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flipsider.Engine.Maths;

namespace Flipsider
{
    public class RoofBuildingEntity : PropEntity
    {
        public override string Prop => "RoofBuilding1";
        public override void PostLoad(Prop prop)
        {
            Chunk chunk = Main.tileManager.GetChunkToWorldCoords(prop.Position);
            chunk.Colliedables.AddCustomHitBox(true, false, new RectangleF(prop.Position + new Vector2(20,0), prop.Size - new Vector2(20,0)));
        }
        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("SunReflectionMap", (SpriteBatch sb) => { sb.Draw(Textures._Props_City_RoofBuilding1Front, prop.Center, Texture.Bounds, 
                Color.Black, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f); });

            Main.lighting.Maps.DrawToMap("SunReflectionMap", (SpriteBatch sb) => { sb.Draw(Textures._Props_City_RoofBuilding1BoxFront, prop.Center, Texture.Bounds, 
                new Color(30,30,30), 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f); });

            spriteBatch.Draw(Texture, prop.Center, Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

