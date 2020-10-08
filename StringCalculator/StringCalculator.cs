using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeTechnologies
{
    public class StringCalculator
    {
        private readonly List<string> _defaultDelimiters = new List<string> { ",", "\n" };
        private const string CustomDelimiterIdentifier = "//";
        private const int StartIndexOfCustomDelimiter = 2;
        private const int StartIndexOfNumbersWithCustomDelimiter = 3;
        private const int MaxNumberLimit = 1000;

        public int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers)) return 0;

            if (numbers.StartsWith(CustomDelimiterIdentifier))
            {
                numbers = GetNumbersExcludingCustomDelimiter(numbers);
            }

            return SumOfNumbers(numbers);
        }

        private int SumOfNumbers(string numbers)
        {
            var convertedNumbers = numbers.Split(_defaultDelimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            ValidateNumbersArePositive(convertedNumbers);

            return convertedNumbers.Where(x => x <= MaxNumberLimit).Sum();
        }

        private string GetNumbersExcludingCustomDelimiter(string numbers)
        {
            var startIndexOfString = AssignCustomDelimiterAndReturnStartIndexOfNumbers(numbers);

            numbers = numbers.Substring(startIndexOfString);
            return numbers;
        }

        private int AssignCustomDelimiterAndReturnStartIndexOfNumbers(string numbers)
        {
            var customDelimiters = GetCustomDelimiter(numbers);
            _defaultDelimiters.AddRange(customDelimiters);

            var hasMultipleDelimiters = customDelimiters.Count > 1;
            var multipleDelimiterLength = hasMultipleDelimiters ? (customDelimiters.Count * 2) : 0;

            return StartIndexOfNumbersWithCustomDelimiter + customDelimiters.Sum(x => x.Length) + multipleDelimiterLength;
        }

        private static void ValidateNumbersArePositive(IReadOnlyCollection<int> convertedNumbers)
        {
            if (!convertedNumbers.Any(x => x < 0)) return;

            var negativeNumbers = string.Join(",", convertedNumbers.Where(x => x < 0).Select(x => x.ToString()).ToArray());
            throw new FormatException($"negatives not allowed '{negativeNumbers}'");
        }

        private static IList<string> GetCustomDelimiter(string numbers)
        {
            var allDelimiters = numbers.Substring(StartIndexOfCustomDelimiter, numbers.IndexOf('\n') - StartIndexOfCustomDelimiter);

            var splitDelimiters = allDelimiters.Split('[').Select(x => x.TrimEnd(']')).ToList();

            if (splitDelimiters.Contains(string.Empty))
            {
                splitDelimiters.Remove(string.Empty);
            }

            return splitDelimiters;
        }
    }
}
