using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day09
{
	public class Day09 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			int preamble = 25;
			//int preamble = 5;

			List<long> lines = Input.Split("\r\n").Select(long.Parse).ToList();

			for (int i = preamble; i < lines.Count; i++)
			{
				bool found = false;

				for (int j = 1; j <= preamble; j++)
				{
					for (int k = 1; k <= preamble; k++)
					{
						if (j != k && lines[i - j] + lines[i - k] == lines[i])
						{
							found = true;
							break;
						}
					}

					if (found)
					{
						break;
					}
				}

				if (!found)
				{
					return lines[i].ToString();
				}
			}

			return "not implemented";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<long> lines = Input.Split("\r\n").Select(long.Parse).ToList();

			long target = 530627549;
			//long target = 127;

			long lowest = 0;
			long highest = 0;
			int longestStreak = 0;

			for (int i = 0; i < lines.Count; i++)
			{
				long accumulator = 0;
				int currentStreak = 1;

				for (int j = 0; j < lines.Count - i; j++)
				{
					if (accumulator + lines[i + j] < target)
					{
						accumulator += lines[i + j];
						currentStreak++;
					}
					else if (accumulator + lines[i + j] > target)
					{
						break;
					}
					else
					{
						if (currentStreak > longestStreak)
						{
							lowest = lines.Skip(i).Take(currentStreak).Min();
							highest = lines.Skip(i).Take(currentStreak).Max();

							longestStreak = currentStreak;
						}
					}
				}
			}

			Console.WriteLine($"Found {lowest} and {highest} in streak of {longestStreak}");
			return (lowest + highest).ToString();
		}
	}
}