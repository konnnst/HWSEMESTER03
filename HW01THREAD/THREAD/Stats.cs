namespace MultiThread;

static class ListStatsExtension
{
    /// <summary>
    /// Counts median of list
    /// </summary>
    /// <param name="numbers">List of double numbers</param>
    /// <returns>Median of list</returns>
    public static double Median(this List<double> numbers)
    {
        if (numbers.Count == 0)
            return 0;

        numbers = numbers.OrderBy(n => n).ToList();

        var halfIndex = numbers.Count() / 2;

        if (numbers.Count() % 2 == 0)
            return (numbers[halfIndex] + numbers[halfIndex - 1]) / 2.0;

        return numbers[halfIndex];
    }

    /// <summary>
    /// Counts standart deviation of list elements
    /// </summary>
    /// <param name="numbers">List of double numbers</param>
    /// <returns>Standart deviation of list elements</returns>
    public static double StandardDeviation(this List<double> numbers)
    {
        if (numbers.Count == 0)
            return 0;

        var avg = numbers.Average();
        var sum = numbers.Sum(d => Math.Pow(d - avg, 2));
        return Math.Sqrt(sum / (numbers.Count() - 1));
    }
}