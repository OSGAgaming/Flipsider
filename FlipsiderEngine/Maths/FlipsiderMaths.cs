using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Flipsider.Maths
{
    public static class FlipsiderMaths
    {
        public const float FourPi = 12.5663706144f;
        public const float PiSquared = 9.86960440109f;
        public const float TauSquared = 39.4784176044f;

        public static double IntPow(double value, int power)
        {
            double result = value;
            for (int i = 1; i < power; i++)
            {
                result *= value;
            }
            return result;
        }

        public static long Factorial(int num)
        {
            if (num < 0) throw new ArgumentException();

            long result = 1;

            for (int i = 1; i <= num; i++)
            {
                result *= i;
            }

            return result;
        }

        public static BigInteger FactorialBig(int num)
        {
            if (num < 0) throw new ArgumentException();

            BigInteger result = 1;

            for (int i = 1; i <= num; i++)
            {
                result *= i;
            }

            return result;
        }

        public static long NCR(int n, int r)
        {
            if (r < 0 || r > n) throw new ArgumentException("R must be between 0 and n inclusive.");

            return Factorial(n) / (Factorial(r) * Factorial(n - r));
        }

        public static BigInteger NCRBig(int n, int r)
        {
            if (r < 0 || r > n) throw new ArgumentException("R must be between 0 and n inclusive.");

            return FactorialBig(n) / (FactorialBig(r) * FactorialBig(n - r));
        }
    }
}
