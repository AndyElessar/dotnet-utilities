namespace Utilities.Statistics;

public static partial class StatisticsExtensions
{
    /// <param name="data">The paired x and y data sequences. X is independent variable data sequence, Y is dependent variable data sequence.</param>
    extension(scoped in XYSeq<decimal> data)
    {
        /// <summary>
        /// Calculates the coefficient of determination (R²) for the given x and y data.
        /// </summary>
        /// <returns>The R² value, representing the proportion of variance explained by the linear relationship.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public decimal RSquared()
        {
            checked
            {
                var corr = Pearson(in data);
                return corr * corr;
            }
        }

        /// <summary>
        /// Calculates the Pearson correlation coefficient for the given x and y data.
        /// </summary>
        /// <returns>The Pearson correlation coefficient, ranging from -1 to 1.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public decimal Pearson()
        {
            if(data.Length < 2)
            {
                throw new ArgumentException($"A regression of the requested order requires at least 2 samples. " +
                    $"Only {data.Length} samples have been provided.");
            }

            checked
            {
                // Calculate mean values
                decimal meanX = 0m;
                decimal meanY = 0m;
                foreach(var (x, y) in data)
                {
                    meanX += x;
                    meanY += y;
                }
                meanX /= data.Length;
                meanY /= data.Length;

                // Calculate numerator and denominator
                decimal numerator = 0m;
                decimal sumSqX = 0m;
                decimal sumSqY = 0m;
                foreach(var (x, y) in data)
                {
                    decimal dx = x - meanX;
                    decimal dy = y - meanY;
                    numerator += dx * dy;
                    sumSqX += dx * dx;
                    sumSqY += dy * dy;
                }

                if(sumSqX == 0m || sumSqY == 0m)
                    return 0m;

                return numerator / (decimal)Math.Sqrt((double)(sumSqX * sumSqY));
            }
        }
    }

    /// <param name="data">The paired x and y data sequences. X is independent variable data sequence, Y is dependent variable data sequence.</param>
    extension(scoped in XYSeq<double> data)
    {
        /// <summary>
        /// Calculates the coefficient of determination (R²) for the given x and y data.
        /// </summary>
        /// <returns>The R² value, representing the proportion of variance explained by the linear relationship.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public double RSquared()
        {
            checked
            {
                var corr = Pearson(in data);
                return corr * corr;
            }
        }

        /// <summary>
        /// Calculates the Pearson correlation coefficient for the given x and y data.
        /// </summary>
        /// <returns>The Pearson correlation coefficient, ranging from -1 to 1.</returns>
        /// <exception cref="ArgumentException">Thrown when x and y have different lengths or fewer than 2 samples are provided.</exception>
        public double Pearson()
        {
            if(data.Length < 2)
            {
                throw new ArgumentException($"A regression of the requested order requires at least 2 samples. " +
                    $"Only {data.Length} samples have been provided.");
            }

            checked
            {
                // Calculate mean values
                double meanX = 0.0;
                double meanY = 0.0;
                foreach(var (x, y) in data)
                {
                    meanX += x;
                    meanY += y;
                }
                meanX /= data.Length;
                meanY /= data.Length;

                // Calculate numerator and denominator
                double numerator = 0.0;
                double sumSqX = 0.0;
                double sumSqY = 0.0;
                foreach(var (x, y) in data)
                {
                    double dx = x - meanX;
                    double dy = y - meanY;
                    numerator += dx * dy;
                    sumSqX += dx * dx;
                    sumSqY += dy * dy;
                }

                if(sumSqX == 0.0 || sumSqY == 0.0)
                    return 0.0;

                return numerator / Math.Sqrt(sumSqX * sumSqY);
            }
        }
    }
}
