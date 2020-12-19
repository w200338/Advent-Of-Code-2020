using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day19
{
	public class Rule8 : IRule
	{
		/// <inheritdoc />
		public int RuleIndex { get; set; } = 8;

		/// <inheritdoc />
		public (bool, int) IsMatch(string input)
		{
			// try to match rule 42 over and over
			List<(bool, int)> matches = new List<(bool, int)>();
			int offset = 0;
			while (offset < input.Length)
			{
				(bool, int) result = Day19.Rules.First(r => r.RuleIndex == 42).IsMatch(input.Substring(offset));
				if (result.Item1)
				{
					matches.Add(result);
					offset += result.Item2;
				}
				else
				{
					break;
				}
			}

			if (matches.Count == 0)
			{
				return (false, 0);
			}

			return (true, offset);
		}
	}
}