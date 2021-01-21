using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Flipsider.Engine.Maths
{
    public abstract class EaseFunction
    {
        public static readonly EaseFunction Linear = new PolynomialEase((float x) => { return x; });
        public static readonly EaseFunction ReverseLinear = new PolynomialEase((float x) => { return 1f - x; });

        public static readonly EaseFunction EaseQuadIn = new PolynomialEase((float x) => { return x * x; });
        public static readonly EaseFunction EaseQuadOut = new PolynomialEase((float x) => { return 1f - EaseQuadIn.Ease(1f - x); });
        public static readonly EaseFunction EaseQuadInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 2f * x * x : -2f * x * x + 4f * x - 1f; });

        public static readonly EaseFunction EaseCubicIn = new PolynomialEase((float x) => { return x * x * x; });
        public static readonly EaseFunction EaseCubicOut = new PolynomialEase((float x) => { return 1f - EaseCubicIn.Ease(1f - x); });
        public static readonly EaseFunction EaseCubicInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 4f * x * x * x : 4f * x * x * x - 12f * x * x + 12f * x - 3f; });

        public static readonly EaseFunction EaseQuarticIn = new PolynomialEase((float x) => { return x * x * x * x; });
        public static readonly EaseFunction EaseQuarticOut = new PolynomialEase((float x) => { return 1f - EaseQuarticIn.Ease(1f - x); });
        public static readonly EaseFunction EaseQuarticInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 8f * x * x * x * x : -8f * x * x * x * x + 32f * x * x * x - 48f * x * x + 32f * x - 7f; });

        public static readonly EaseFunction EaseQuinticIn = new PolynomialEase((float x) => { return x * x * x * x * x; });
        public static readonly EaseFunction EaseQuinticOut = new PolynomialEase((float x) => { return 1f - EaseQuinticIn.Ease(1f - x); });
        public static readonly EaseFunction EaseQuinticInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? 16f * x * x * x * x * x : 16f * x * x * x * x * x - 80f * x * x * x * x + 160f * x * x * x - 160f * x * x + 80f * x - 15f; });

        public static readonly EaseFunction EaseCircularIn = new PolynomialEase((float x) => { return 1f - (float)Math.Sqrt(1.0 - Math.Pow(x, 2)); });
        public static readonly EaseFunction EaseCircularOut = new PolynomialEase((float x) => { return (float)Math.Sqrt(1.0 - Math.Pow(x - 1.0, 2)); });
        public static readonly EaseFunction EaseCircularInOut = new PolynomialEase((float x) => { return (x < 0.5f) ? (1f - (float)Math.Sqrt(1.0 - Math.Pow(x * 2, 2))) * 0.5f : (float)((Math.Sqrt(1.0 - Math.Pow(-2 * x + 2, 2)) + 1) * 0.5); });

        public virtual float Ease(float time)
        {
            throw new NotImplementedException();
        }

        /*
        public  void DrawDebug(SpriteBatch spriteBatch, int x, int y) 
        {
            Vector2 topLeft = new Vector2(x, y);
            Vector2 bottomLeft = new Vector2(topLeft.X, topLeft.Y + 200);
            spriteBatch.DrawLine(topLeft, bottomLeft, Color.Red, 2f);
            spriteBatch.DrawLine(bottomLeft, bottomLeft + Vector2.UnitX * 200f, Color.Red, 2f);

            int points = 100;
            float perIteration = 1f / points;

            Vector2 previous = new Vector2(x, y + 200);

            for (float i = perIteration; i <= 1f; i += perIteration)
            {
                float value = Ease(i);
                float xOffset = i * 200f;
                float yOffset = value * -200f;

                Vector2 next = new Vector2(x + xOffset, y + 200 + yOffset);

                spriteBatch.DrawLine(previous, next, Color.White, 2f);

                previous = next;
            }
        }*/
    }

    public class PolynomialEase : EaseFunction
    {
        private readonly Func<float, float> _function;

        public PolynomialEase(Func<float, float> func)
        {
            _function = func;
        }

        public override float Ease(float time)
        {
            return _function(time);
        }

        /// <summary>
        /// Generates a polynomial ease function based on the factor and the type
        /// Types: 0->in  1->out  2->in/out
        /// </summary>
        public static EaseFunction Generate(int factor, int type)
        {
            switch (type)
            {
                default:
                case 0: //in
                    return new PolynomialEase((float x) =>
                    {
                        float y = x;
                        for (int i = 1; i < factor; i++)
                        {
                            x *= y;
                        }
                        return x;
                    });
                case 1: //out
                    return new PolynomialEase((float x) =>
                    {
                        float y = 1f - x;
                        float z = y;
                        for (int i = 1; i < factor; i++)
                        {
                            y *= z;
                        }
                        return 1f - y;
                    });
                case 2: //in out
                    return new PolynomialEase((float x) =>
                    {
                        double pow = FlipsiderMaths.IntPow(2f, factor - 1);
                        if (x < 0.5f)
                        {
                            float originalX = x;
                            for (int i = 1; i < factor; i++)
                            {
                                x *= originalX;
                            }
                            return (float)(pow * x);
                        }
                        double y = 0f;
                        if (factor <= 20) //faster method
                        {
                            for (int r = 0; r <= factor; r++)
                            {
                                double pascal = FlipsiderMaths.NCR(factor, r);
                                double sign = (factor + r) % 2 == 0 ? -1.0 : 1.0;
                                double value = pow * sign * pascal;
                                int num = factor - r;
                                for (int i = 0; i < num; i++)
                                {
                                    value *= x;
                                }
                                if (num == 0)
                                {
                                    value++;
                                }
                                y += value;
                            }
                        }
                        else //slower method but more accurate for a larger polynomial
                        {
                            decimal bigResult = 0;
                            for (int r = 0; r <= factor; r++)
                            {
                                decimal pascal = (decimal)FlipsiderMaths.NCRBig(factor, r);
                                decimal sign = (factor + r) % 2 == 0 ? -1 : 1;
                                decimal value = (decimal)pow * sign * pascal;
                                int num = factor - r;
                                for (int i = 0; i < num; i++)
                                {
                                    value *= (decimal)x;
                                }
                                if (num == 0)
                                {
                                    value++;
                                }
                                bigResult += value;
                            }
                            y = (double)bigResult;
                        }
                        return (float)y;
                    });
            }
        }
    }

    public class EaseBuilder : EaseFunction
    {
        private readonly List<EasePoint> _points;

        public EaseBuilder()
        {
            _points = new List<EasePoint>();
        }

        public void AddPoint(float x, float y, EaseFunction function) => AddPoint(new Vector2(x, y), function);

        public void AddPoint(Vector2 vector, EaseFunction function)
        {
            if (vector.X > 1f || vector.X < 0f) throw new ArgumentException("X value of point is not in valid range!");

            EasePoint newPoint = new EasePoint(vector, function);
            if (_points.Count == 0)
            {
                _points.Add(newPoint);
                return;
            }

            EasePoint last = _points[_points.Count - 1];
            if (last.Point.X > vector.X) throw new ArgumentException("New point has an x value less than the previous point when it should be greater or equal");

            _points.Add(newPoint);
        }

        public override float Ease(float time)
        {
            Vector2 prevPoint = Vector2.Zero;
            EasePoint usePoint = _points[0];
            for (int i = 0; i < _points.Count; i++)
            {
                usePoint = _points[i];
                if (time <= usePoint.Point.X)
                {
                    break;
                }
                prevPoint = usePoint.Point;
            }
            float dist = usePoint.Point.X - prevPoint.X;
            float progress = (time - prevPoint.X) / dist;
            return MathHelper.Lerp(prevPoint.Y, usePoint.Point.Y, usePoint.Function.Ease(progress));
        }

        private struct EasePoint
        {
            public Vector2 Point;
            public EaseFunction Function;

            public EasePoint(Vector2 p, EaseFunction func)
            {
                Point = p;
                Function = func;
            }
        }
    }
}
