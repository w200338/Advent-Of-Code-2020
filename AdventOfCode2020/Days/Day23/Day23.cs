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
			LinkedList<int> cups = new LinkedList<int>(Input.Select(c => int.Parse(c.ToString())));
			while (cups.Count < 1000000)
			{
				cups.AddLast(cups.Count + 1);
			}

			LinkedListNode<int> currentNode = cups.First;

			Dictionary<int, LinkedListNode<int>> lookUp = new Dictionary<int, LinkedListNode<int>>();
			while (currentNode != null)
			{
				lookUp.Add(currentNode.Value, currentNode);
				currentNode = currentNode.Next;
			}

			currentNode = cups.First;

			for (int i = 0; i < 10_000_000; i++)
			{
				List<LinkedListNode<int>> pickUp = new List<LinkedListNode<int>>();
				pickUp.Add(currentNode.Next ?? cups.First);
				pickUp.Add(pickUp[0].Next);
				pickUp.Add(pickUp[1].Next);

				LinkedListNode<int> destination = lookUp[(currentNode.Value - 1 + cups.Count) % cups.Count + 1];

				while (pickUp.Contains(destination))
				{
					destination = lookUp[(destination.Value - 1 + cups.Count) % cups.Count + 1];
				}

				cups.Remove(pickUp[0]);
				cups.Remove(pickUp[1]);
				cups.Remove(pickUp[2]);
				cups.AddAfter(destination, pickUp[0]);
				cups.AddAfter(pickUp[0], pickUp[1]);
				cups.AddAfter(pickUp[1], pickUp[2]);

				currentNode = currentNode.Next ?? cups.First;
				
				if (i % 10000 == 0)
				{
					Console.WriteLine($"{i}/10000000");
				}

				// make sure it's round
				//cups.AddBefore(cups.First, cups.Last);
				//cups.AddAfter(cups.Last, cups.First);
			}

			long output = lookUp[1].Next.Value * lookUp[1].Next.Next.Value;

			return output.ToString();
		}

		public int Modulo(int input, int mod)
		{
			return input < 0 ? (input % mod + mod) % mod : input % mod;
		}
	}
}