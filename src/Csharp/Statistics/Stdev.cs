namespace Utilities.Statistics;

public static partial class StatisticsExtensions
{
    extension(scoped in ReadOnlySpan<decimal> x)
    {
        /// <summary>
        /// Calculates the sample standard deviation.
        /// </summary>
        /// <returns>The sample standard deviation of the data.</returns>
        /// <exception cref="ArgumentException">Thrown when there are fewer than 2 data points.</exception>
        public decimal Stdev()
        {
            if(x.Length < 2)
                throw new ArgumentException($"At least 2 data points are required to calculate standard deviation. Only {x.Length} provided!");

            checked
            {
                decimal avg = Average(x);
                decimal sum = 0m;
                for(int i = 0; i < x.Length; i++)
                {
                    decimal diff = x[i] - avg;
                    sum += diff * diff;
                }
                return (decimal)Math.Sqrt((double)(sum / (x.Length - 1)));
            }
        }
    }

    extension(scoped in ReadOnlySpan<double> x)
    {
        /// <summary>
        /// Calculates the sample standard deviation.
        /// </summary>
        /// <returns>The sample standard deviation of the data.</returns>
        /// <exception cref="ArgumentException">Thrown when there are fewer than 2 data points.</exception>
        public double Stdev()
        {
            if(x.Length < 2)
                throw new ArgumentException($"At least 2 data points are required to calculate standard deviation. Only {x.Length} provided!");

            checked
            {
                double avg = Average(x);
                double sum = 0.0;
                for(int i = 0; i < x.Length; i++)
                {
                    double diff = x[i] - avg;
                    sum += diff * diff;
                }
                return Math.Sqrt(sum / (x.Length - 1));
            }
        }
    }
}
