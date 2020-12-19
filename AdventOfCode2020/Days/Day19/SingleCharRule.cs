namespace AdventOfCode2020.Days.Day19
{
	public class SingleCharRule : IRule
	{
		/// <inheritdoc />
		public int RuleIndex { get; set; }

		public char Character { get; set; }

		/// <inheritdoc />
		public (bool, int) IsMatch(string input)
		{
			if (input.Length == 0)
			{
				return (false, 0);
			}

			if (input[0] == Character)
			{
				return (true, 1);
			}

			return (false, 0);
		}
	}
}