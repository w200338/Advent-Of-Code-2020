namespace AdventOfCode2020.Days.Day19
{
	public class ChoiceRule : IRule
	{
		/// <inheritdoc />
		public int RuleIndex { get; set; }

		public OrderRule RuleOne { get; set; }
		public OrderRule RuleTwo { get; set; }

		/// <inheritdoc />
		public (bool, int) IsMatch(string input)
		{
			(bool, int) result1 = RuleOne.IsMatch(input);

			if (result1.Item1)
			{
				return (true, result1.Item2);
			}

			(bool, int) result2 = RuleTwo.IsMatch(input.Substring(result1.Item2));

			if (result2.Item1)
			{
				return (true, result2.Item2);
			}

			return (false, 0);
		}
	}
}