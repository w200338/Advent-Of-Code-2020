using System;

namespace AdventOfCode2020.Days.Day03
{
	public class Day03 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");
			Console.WriteLine("found " + lines.Length + " lines");

			int lineWidth = lines[0].Length;
			int encounteredTrees = 0;
			int xCoord = 0;
			for (int i = 0; i < lines.Length; i++)
			{
				encounteredTrees += lines[i][xCoord] == '#' ? 1 : 0;
				xCoord += 3;

				if (xCoord >= lineWidth)
				{
					xCoord -= lineWidth;
				}
			}

			return encounteredTrees.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			int[] xSlopes = {1, 3, 5, 7};

			string[] lines = Input.Split("\r\n");
			Console.WriteLine("found " + lines.Length + " lines");

			int lineWidth = lines[0].Length;
			int[] encounteredTrees = {0, 0, 0, 0, 0};

			int xCoord = 0;
			for (int j = 0; j < xSlopes.Length; j++)
			{
				xCoord = 0;
				for (int i = 0; i < lines.Length; i++)
				{
					encounteredTrees[j] += lines[i][xCoord] == '#' ? 1 : 0;
					xCoord += xSlopes[j];

					if (xCoord >= lineWidth)
					{
						xCoord -= lineWidth;
					}
				}
			}
			xCoord = 0;
			for (int i = 0; i < lines.Length; i += 2)
			{
				encounteredTrees[4] += lines[i][xCoord] == '#' ? 1 : 0;
				xCoord += 1;

				if (xCoord >= lineWidth)
				{
					xCoord -= lineWidth;
				}
			}

			long total = 1;
			for (int i = 0; i < encounteredTrees.Length; i++)
			{
				total *= encounteredTrees[i];
			}

			return total.ToString();
		}
	}
}