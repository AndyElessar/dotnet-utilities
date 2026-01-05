using System.Runtime.CompilerServices;

namespace Utilities.Statistics;

public static partial class StatisticsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ThrowIfLengthIsZero(scoped in int length)
    {
        if(length == 0)
            throw new ArgumentException("Data length cannot be zero!");
    }

    extension(scoped in ReadOnlySpan<decimal> x)
    {
        /// <summary>
        /// Calculates the sum of squared deviations from the mean (Sxx).
        /// </summary>
        /// <returns>The sum of squared deviations.</returns>
        public decimal Sxx()
        {
            ThrowIfLengthIsZero(x.Length);

            checked
            {
                decimal x2 = 0;
                decimal xSum = 0;
                for(int i = 0; i < x.Length; i++)
                {
                    x2 += x[i] * x[i];
                    xSum += x[i];
                }
                return x2 - ((xSum * xSum) / x.Length);
            }
        }
    }

    extension(scoped in ReadOnlySpan<double> x)
    {
        /// <summary>
        /// Calculates the sum of squared deviations from the mean (Sxx).
        /// </summary>
        /// <returns>The sum of squared deviations.</returns>
        public double Sxx()
        {
            ThrowIfLengthIsZero(x.Length);

            checked
            {
                double x2 = 0;
                double xSum = 0;
                for(int i = 0; i < x.Length; i++)
                {
                    x2 += x[i] * x[i];
                    xSum += x[i];
                }
                return x2 - ((xSum * xSum) / x.Length);
            }
        }
    }

    extension(scoped in XYSeq<decimal> data)
    {
        /// <summary>
        /// Calculates the sum of products of deviations from the mean for x and y (Sxy).
        /// </summary>
        /// <returns>The sum of products of deviations.</returns>
        /// <exception cref="ArgumentException">Thrown when the lengths of x and y do not match.</exception>
        public decimal Sxy()
        {
            ThrowIfLengthIsZero(data.Length);

            checked
            {
                decimal xySum = 0;
                decimal xSum = 0;
                decimal ySum = 0;
                foreach(var (x, y) in data)
                {
                    xySum += x * y;
                    xSum += x;
                    ySum += y;
                }
                return xySum - ((xSum * ySum) / data.Length);
            }
        }
    }

    extension(scoped in XYSeq<double> data)
    {
        /// <summary>
        /// Calculates the sum of products of deviations from the mean for x and y (Sxy).
        /// </summary>
        /// <returns>The sum of products of deviations.</returns>
        /// <exception cref="ArgumentException">Thrown when the lengths of x and y do not match.</exception>
        public double Sxy()
        {
            ThrowIfLengthIsZero(data.Length);

            checked
            {
                double xySum = 0;
                double xSum = 0;
                double ySum = 0;
                foreach(var (x, y) in data)
                {
                    xySum += x * y;
                    xSum += x;
                    ySum += y;
                }
                return xySum - ((xSum * ySum) / data.Length);
            }
        }
    }
}
