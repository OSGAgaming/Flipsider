using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace FlipEngine
{
    internal class LightPrimitives : Primitive
    {
        private DirectionalLight light;
        public LightPrimitives(DirectionalLight light)
        {
            this.light = light;
        }
        public override void SetDefaults()
        {
            Alpha = 0.7f;
            Width = 1;
            PrimitiveCount = 1000;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            Color colour = light.colour*3;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                AddVertex(new Vector2(light.position.X, light.position.Y), colour, new Vector2(0.5f, 0.5f));
                AddVertex(light.points[i], colour, (light.points[i] - light.position)/300f + Vector2.One/2);
                AddVertex(light.points[i + 1], colour, (light.points[i + 1] - light.position) / 300f + Vector2.One / 2);
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EffectCache.PrimtiveShader ?? new BasicEffect(FlipGame.graphics.GraphicsDevice), "DirLight", TimeAlive / 40f);
        }
        public override void OnUpdate()
        {
            _points = light.points.ToList();
            VertexCount = _points.Count() * 6;
            if (PrimitiveCount < VertexCount / 6)
            {
                _points.RemoveAt(0);
            }
        }
        public override void OnDestroy()
        {
            Width *= 0.9f;
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}