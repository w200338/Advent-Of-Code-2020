namespace AdventOfCode2020.Days.Day19
{
	public interface IRule
	{
		public int RuleIndex { get; set; }
		(bool, int) IsMatch(string input);
	}
}