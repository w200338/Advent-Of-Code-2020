using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day10
{
	public class Day10 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<int> adapters = Input.Split("\r\n").Select(int.Parse).ToList();
			adapters.Add(adapters.Max() + 3);

			adapters.Sort();

			int current = 0;
			int difference1 = 0;
			int difference3 = 0;

			for (int i = 0; i < adapters.Count; i++)
			{
				if (adapters[i] == current + 1)
				{
					difference1++;
					current = adapters[i];
				}
				else if (adapters[i] == current + 3)
				{
					difference3++;
					current = adapters[i];
				}
			}

			return $"end joltage: {current}\n1 diff: {difference1}\n3 diff: {difference3}\n{difference1 * difference3}";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<int> adapters = Input.Split("\r\n").Select(int.Parse).ToList();
			adapters.Add(adapters.Max() + 3);
			adapters.Add(0);

			adapters.Sort();

			List<long> ways = new List<long>();
			ways.Add(1);
			for (int i = 1; i < adapters.Count; i++)
			{
				ways.Add(0);
			}

			for (int i = 0; i < adapters.Count; i++)
			{
				for (int j = i + 1; j < adapters.Count; j++)
				{
					if (adapters[j] - adapters[i] > 3)
					{
						break;
					}

					ways[j] += ways[i];
				}
			}

			return ways.Last().ToString();
		}
	}
}