using System.Numerics;

namespace Utilities.Statistics;

/// <summary>
/// Represents the result of a linear regression calculation.
/// </summary>
/// <typeparam name="T">The numeric type of the values.</typeparam>
/// <param name="Intercept">The y-intercept of the regression line.</param>
/// <param name="Slope">The slope of the regression line.</param>
public readonly record struct LineInfo<T>(
    T Intercept,
    T Slope
) where T : struct, INumber<T>;

public static partial class StatisticsExtensions
{
    /// <param name="data">The paired x and y data sequences. X is independent variable data sequence, Y is dependent variable data sequence.</param>
    extension(XYSeq<decimal> data)
    {
        /// <summary>
        /// Performs simple linear regression on the provided x and y data.
        /// </summary>
        /// <returns>A <see cref="LineInfo{T}"/> containing the intercept and slope of the regression line.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public LineInfo<decimal> LinearRegression()
        {
            if(data.Length < 2)
            {
                throw new ArgumentException($"A regression of the requested order requires at least 2 samples. " +
                    $"Only {data.Length} samples have been provided.");
            }

            checked
            {
                // First Pass: Mean
                decimal mx = 0m;
                decimal my = 0m;
                foreach(var (x, y) in data)
                {
                    mx += x;
                    my += y;
                }
                mx /= data.Length;
                my /= data.Length;

                // Second Pass: Covariance/Variance
                decimal covariance = 0.0m;
                decimal variance = 0.0m;
                foreach(var (x, y) in data)
                {
                    decimal diff = x - mx;
                    covariance += diff * (y - my);
                    variance += diff * diff;
                }
                decimal b = covariance / variance;

                return new(Intercept: my - b * mx, Slope: b);
            }
        }
    }

    /// <param name="data">The paired x and y data sequences. X is independent variable data sequence, Y is dependent variable data sequence.</param>
    extension(XYSeq<double> data)
    {
        /// <summary>
        /// Performs simple linear regression on the provided x and y data.
        /// </summary>
        /// <returns>A <see cref="LineInfo{T}"/> containing the intercept and slope of the regression line.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public LineInfo<double> LinearRegression()
        {
            if(data.Length < 2)
            {
                throw new ArgumentException($"A regression of the requested order requires at least 2 samples. " +
                    $"Only {data.Length} samples have been provided.");
            }

            checked
            {
                // First Pass: Mean
                double mx = 0.0;
                double my = 0.0;
                foreach(var (x, y) in data)
                {
                    mx += x;
                    my += y;
                }
                mx /= data.Length;
                my /= data.Length;

                // Second Pass: Covariance/Variance
                double covariance = 0.0;
                double variance = 0.0;
                foreach(var (x, y) in data)
                {
                    double diff = x - mx;
                    covariance += diff * (y - my);
                    variance += diff * diff;
                }
                double b = covariance / variance;

                return new(Intercept: my - b * mx, Slope: b);
            }
        }
    }
}
