
using Flipsider.Engine;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    internal class RainbowLightTrail : Primitive
    {
        public RainbowLightTrail()
        {

        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
            _width = 1;
            _cap = 100;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == 0)
                {

                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        MakePrimHelix(i, 20, 0.8f, default, 1, 2);
                    }
                }
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EffectCache.PrimtiveShader ?? new BasicEffect(Main.graphics.GraphicsDevice), "RainbowLightPass", _counter / 40f);
        }
        public override void OnUpdate()
        {
            _points.Add(Main.player.Center);
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