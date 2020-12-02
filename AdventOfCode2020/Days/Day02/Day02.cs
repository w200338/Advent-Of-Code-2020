using System.Linq;

namespace AdventOfCode2020.Days.Day02
{
	public class Day02 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] passwords = Input.Split('\n');

			int valid = 0;
			foreach (string password in passwords)
			{
				string[] parts = password.Split(new char[] {'-', ' ', ':'}, System.StringSplitOptions.RemoveEmptyEntries);

				int min = int.Parse(parts[0]);
				int max = int.Parse(parts[1]);
				char character = parts[2][0];
				string passwordAttempt = parts[3];

				int countInPassword = passwordAttempt.Count(c => c == character);
				if (countInPassword >= min && countInPassword <= max)
				{
					valid++;
				}
			}

			return valid.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] passwords = Input.Split('\n');

			int valid = 0;
			foreach (string password in passwords)
			{
				string[] parts = password.Split(new char[] { '-', ' ', ':' }, System.StringSplitOptions.RemoveEmptyEntries);

				int pos1 = int.Parse(parts[0]);
				int pos2 = int.Parse(parts[1]);
				char character = parts[2][0];
				string passwordAttempt = parts[3];

				if (passwordAttempt[pos1 - 1] == character && passwordAttempt[pos2 - 1] != character ||
				    passwordAttempt[pos1 - 1] != character && passwordAttempt[pos2 - 1] == character)
				{
					valid++;
				}
			}

			return valid.ToString();
		}
	}
}