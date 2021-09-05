using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public class DirectionalLight : LightSource
    {
        private float angularCoverage;
        private int Accuracy => 10;
        public Vector2[] points;
        public DirectionalLight(float str, Vector2 pos, Color col, float angularCoverage, float rotation) : base(str, pos, col)
        {
            this.rotation = rotation;
            this.angularCoverage = angularCoverage;
            Layer = LayerHandler.CurrentLayer;
            points = new Vector2[Accuracy];
            //Mesh = new LightPrimitives(this);
            Main.Primitives.AddComponent(Mesh);
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = position;
            Mesh?.Draw(spriteBatch);
            for (int i = -Accuracy / 2; i < Accuracy / 2; i++)
            {
                Vector2 diffVec = (Vector2.UnitX * strength).RotatedBy(rotation);
                Vector2 secondPos = origin + diffVec.RotatedBy(i / (Accuracy / angularCoverage));
                Vector2 intersection = Utils.ReturnIntersectionTile(Main.World, origin.ToPoint(), secondPos.ToPoint());
                bool intersectionState = Utils.LineIntersectsTile(Main.World, origin.ToPoint(), secondPos.ToPoint());
                if (intersectionState)
                {
                    points[i + Accuracy / 2] = intersection;
                }
                else
                {
                    points[i + Accuracy / 2] = secondPos;
                }

            }
        }
    }
}