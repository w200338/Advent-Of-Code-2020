using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day19
{
	public class OrderRule : IRule
	{
		/// <inheritdoc />
		public int RuleIndex { get; set; }

		public List<int> RuleIndexes { get; set; } = new List<int>();

		//public int RuleOne { get; set; }
		//public int RuleTwo { get; set; }

		/// <inheritdoc />
		public (bool, int) IsMatch(string input)
		{
			int totalLength = 0;
			foreach (int rule in RuleIndexes)
			{
				(bool, int) result = Day19.Rules.First(r => r.RuleIndex == rule).IsMatch(input.Substring(totalLength));
				if (!result.Item1)
				{
					return (false, 0);
				}

				totalLength += result.Item2;
			}

			return (true, totalLength);

			/*
			(bool, int) result1 = Rules.First(r => r.RuleIndex == RuleOne).IsMatch(input);

			if (result1.Item1)
			{
				(bool, int) result2 = Rules.First(r => r.RuleIndex == RuleTwo).IsMatch(input.Substring(result1.Item2));

				if (result2.Item1)
				{
					return (true, result1.Item2 + result2.Item2);
				}
			}

			return (false, 0);
			*/
		}
	}
}