using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day19
{
	public class Rule11 : IRule
	{
		/// <inheritdoc />
		public int RuleIndex { get; set; } = 11;

		/// <inheritdoc />
		public (bool, int) IsMatch(string input)
		{
			// match rule 42 as much as possible first
			List<(bool, int)> matches42 = new List<(bool, int)>();
			int offset = 0;
			while (offset < input.Length)
			{
				(bool, int) result = Day19.Rules.First(r => r.RuleIndex == 42).IsMatch(input.Substring(offset));
				if (result.Item1)
				{
					matches42.Add(result);
					offset += result.Item2;
				}
				else
				{
					break;
				}
			}

			// match rule 31 as much as possible
			List<(bool, int)> matches31 = new List<(bool, int)>();
			while (offset < input.Length)
			{
				(bool, int) result = Day19.Rules.First(r => r.RuleIndex == 31).IsMatch(input.Substring(offset));
				if (result.Item1)
				{
					matches31.Add(result);
					offset += result.Item2;
				}
				else
				{
					break;
				}
			}

			if (matches31.Count != matches42.Count)
			{
				return (false, 0);
			}

			return (true, offset);
		}
	}
}