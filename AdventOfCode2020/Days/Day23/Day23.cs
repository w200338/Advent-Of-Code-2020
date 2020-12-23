using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day23
{
	public class Day23 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<int> cups = Input.Select(c => int.Parse(c.ToString())).ToList();

			int currentIndex = 0;
			for (int i = 0; i < 100; i++)
			{
				int current = cups[currentIndex];
				int destination = Modulo(current - 1, cups.Count);

				List<int> pickUp = new List<int>();
				for (int j = 0; j < 3; j++)
				{
					pickUp.Add(cups[(currentIndex + 1 + j) % cups.Count]);
				}

				while (pickUp.Contains(destination) || destination == 0)
				{
					destination = (destination + cups.Count) % (cups.Count + 1);

					//Modulo(--destination, cups.Count);
				}

				cups.RemoveAll(item => pickUp.Contains(item));
				cups.InsertRange(cups.IndexOf(destination) + 1, pickUp);

				// shift list around till current is back in its proper index
				while (cups[currentIndex] != current)
				{
					int temp = cups[0];
					cups.RemoveAt(0);
					cups.Add(temp);
				}

				currentIndex++;
				currentIndex %= cups.Count;
			}

			string output = "";
			for (int i = 0; i < cups.Count - 1; i++)
			{
				output += cups[(cups.IndexOf(1) + 1 + i) % cups.Count];
			}

			return output;
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<int> cups = Input.Select(c => int.Parse(c.ToString())).ToList();
			while (cups.Count < 1000000)
			{
				cups.Add(cups.Count + 1);
			}

			int cupCount = cups.Count;

			int currentIndex = 0;
			for (int i = 0; i < 10_000_000; i++)
			{
				int current = cups[currentIndex];
				int destination = Modulo(current - 1, cupCount);

				List<int> pickUp = new List<int>();
				for (int j = 0; j < 3; j++)
				{
					pickUp.Add(cups[(currentIndex + 1 + j) % cupCount]);
				}

				while (pickUp.Contains(destination) || destination == 0)
				{
					destination = (destination + cupCount) % (cupCount + 1);
				}

				// remove selected cups and place them in their new place
				for (int j = 0; j < 3; j++)
				{
					cups.Remove(pickUp[j]);
				}
				cups.InsertRange(cups.IndexOf(destination) + 1, pickUp);

				// shift list around till current is back in its proper index
				while (cups[currentIndex] != current)
				{
					int temp = cups[0];
					cups.RemoveAt(0);
					cups.Add(temp);
				}
				
				currentIndex++;
				currentIndex %= cups.Count;

				if (i % 10000 == 0)
				{
					Console.WriteLine($"{i}/10000000");
				}
			}

			long output = cups[(cups.IndexOf(1) + 1) % cups.Count] * cups[(cups.IndexOf(1) + 2) % cups.Count];

			return output.ToString();
		}

		public int Modulo(int input, int mod)
		{
			return input < 0 ? (input % mod + mod) % mod : input % mod;
		}
	}
}