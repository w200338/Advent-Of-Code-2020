using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day01
{
	public class Day01 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<int> numbers = Input.Split('\n').Select(s => int.Parse(s)).ToList();

			foreach (int number1 in numbers)
			{
				foreach (int number2 in numbers)
				{
					if (number1 + number2 == 2020)
					{
						return (number1 * number2).ToString();
					}
				}
			}

			return "none";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<int> numbers = Input.Split('\n').Select(s => int.Parse(s)).ToList();

			foreach (int number1 in numbers)
			{
				foreach (int number2 in numbers)
				{
					foreach (int number3 in numbers)
					{
						if (number1 + number2 + number3 == 2020)
						{
							return (number1 * number2 * number3).ToString();
						}
					}
				}
			}

			return "none";
		}
	}
}