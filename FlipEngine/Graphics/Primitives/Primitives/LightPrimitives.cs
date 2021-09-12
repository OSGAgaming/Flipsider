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
            _alphaValue = 0.7f;
            _width = 1;
            _cap = 1000;
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
            PrepareShader(EffectCache.PrimtiveShader ?? new BasicEffect(FlipGame.graphics.GraphicsDevice), "DirLight", _counter / 40f);
        }
        public override void OnUpdate()
        {
            _points = light.points.ToList();
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
        }
        public override void OnDestroy()
        {
            _width *= 0.9f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }
    }
}