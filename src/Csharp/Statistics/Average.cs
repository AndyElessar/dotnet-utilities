namespace Utilities.Statistics;

public static partial class StatisticsExtensions
{
    extension(scoped in ReadOnlySpan<decimal> data)
    {
        /// <summary>
        /// Calculates the arithmetic mean (average) of the data.
        /// </summary>
        /// <returns>The average value of the data.</returns>
        public decimal Average()
        {
            checked
            {
                decimal sum = 0m;
                for(int i = 0; i < data.Length; i++)
                {
                    sum += data[i];
                }
                return sum / data.Length;
            }
        }
    }

    extension(scoped in ReadOnlySpan<double> data)
    {
        /// <summary>
        /// Calculates the arithmetic mean (average) of the data.
        /// </summary>
        /// <returns>The average value of the data.</returns>
        public double Average()
        {
            checked
            {
                double sum = 0.0;
                for(int i = 0; i < data.Length; i++)
                {
                    sum += data[i];
                }
                return sum / data.Length;
            }
        }
    }
}
