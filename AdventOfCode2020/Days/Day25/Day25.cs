using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day25
{
	public class Day25 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			int cardPublicKey = int.Parse(lines[0]);
			int doorPublicKey = int.Parse(lines[1]);

			int cardSubjectNumber = 7;
			int cardLoopSize = 0;

			long value = 1;

			while (value != cardPublicKey)
			{
				value *= cardSubjectNumber;
				value %= 20201227;

				cardLoopSize++;
			}

			int doorSubjectNumber = 7;
			int doorLoopSize = 0;

			value = 1;

			while (value != doorPublicKey)
			{
				value *= doorSubjectNumber;
				value %= 20201227;

				doorLoopSize++;
			}

			value = 1;
			for (int i = 0; i < doorLoopSize; i++)
			{
				value *= cardPublicKey;
				value %= 20201227;
			}

			return value.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			return "not implemented";
		}
	}
}