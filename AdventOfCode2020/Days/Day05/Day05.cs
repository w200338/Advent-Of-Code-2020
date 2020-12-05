using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day05
{
	public class Day05 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] boardingPasses = Input.Split('\n').Select(s => s.Trim()).ToArray();

			int max = 0;

			foreach (string boardingPass in boardingPasses)
			{
				char[] charArray = boardingPass.ToCharArray();
				int runningTotal = 0;
				foreach (char c in charArray)
				{
					if (c == 'B' || c == 'R')
					{
						runningTotal |= 1;
					}

					runningTotal <<= 1;
				}

				// counter last, unneeded shift
				runningTotal >>= 1;

				/* this just splits and recombines the number
				int row = (runningTotal >> 3);
				int column = (runningTotal & 0b0111);

				int combined = row * 8 + column;
				*/

				if (runningTotal > max)
				{
					max = runningTotal;
				}
			}

			return max.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] boardingPasses = Input.Split('\n').Select(s => s.Trim()).ToArray();

			List<int> existingPasses = new List<int>();

			foreach (string boardingPass in boardingPasses)
			{
				char[] charArray = boardingPass.ToCharArray();
				int runningTotal = 0;
				foreach (char c in charArray)
				{
					if (c == 'B' || c == 'R')
					{
						runningTotal |= 1;
					}

					runningTotal <<= 1;
				}

				runningTotal >>= 1;

				existingPasses.Add(runningTotal);
			}

			existingPasses.Sort();
			for (int i = 1; i < existingPasses.Count; i++)
			{
				if (existingPasses[i - 1] == existingPasses[i] - 2)
				{
					return (existingPasses[i - 1] + 1).ToString();
				}
			}

			return "No gaps found";
		}
	}
}