using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4_Timer
{
    public static class StatisticalCalculations
    {
        public static long Max(List<long> times)
        {
            return times.Max();
        }

        public static long Min(List<long> times)
        {
            return times.Min();
        }

        public static double Average(List<long> times)
        {
            return times.Average();
        }

        public static double StandardDevaition(List<long> times)
        {
            return ComputeStandartDeviation(times);
        }

        public static double ComputeMathExpectation(List<long> times, int exp = 1)
        {
            var mathExp = 0.0;

            foreach (var elem in times)
            {
                mathExp += Math.Pow(elem, exp) * CountRelativeFrequency(times, elem);
            }

            return mathExp;
        }

        public static int CountFrequency(List<long> times, long elem)
        {
            return times.Count(s => elem.Equals(s));
        }

        public static double CountRelativeFrequency(List<long> times, long elem)
        {
            return (double)CountFrequency(times, elem) / times.Count;
        }

        public static double ComputeDispersion(List<long> times)
        {
            return ComputeMathExpectation(times, 2) - Math.Pow(ComputeMathExpectation(times), 2);
        }

        public static double ComputeStandartDeviation(List<long> times)
        {
            return Math.Sqrt(Math.Abs(ComputeDispersion(times)));
        }
    }
}
