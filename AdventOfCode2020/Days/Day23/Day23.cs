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
			List<int> cupValues = new List<int>(Input.Select(c => int.Parse(c.ToString())));
			while (cupValues.Count < 1_000_000)
			{
				cupValues.Add(cupValues.Count + 1);
			}

			CircularLinkedList<int> cups = new CircularLinkedList<int>(cupValues);

			Dictionary<int, CircularLinkedListNode<int>> lookUp = new Dictionary<int, CircularLinkedListNode<int>>();
			foreach (CircularLinkedListNode<int> node in cups.Nodes)
			{
				lookUp.Add(node.Value, node);
			}

			CircularLinkedListNode<int> currentNode = cups.First;
			for (int i = 0; i < 10_000_000; i++)
			{
				List<CircularLinkedListNode<int>> pickUp = new List<CircularLinkedListNode<int>>
				{
					currentNode.Next,
					currentNode.Next.Next,
					currentNode.Next.Next.Next,
				};

				int destinationValue = currentNode.Value == 1 ? cups.Nodes.Count : currentNode.Value - 1;
				
				while (pickUp[0].Value == destinationValue ||
				       pickUp[1].Value == destinationValue ||
				       pickUp[2].Value == destinationValue)
				{
					destinationValue--;
					if (destinationValue == 0)
					{
						destinationValue = cups.Nodes.Count;
					}
				}

				CircularLinkedListNode<int> destination = lookUp[destinationValue];

				cups.Remove(pickUp[0]);
				cups.Remove(pickUp[1]);
				cups.Remove(pickUp[2]);
				cups.AddAfter(destination, pickUp[0]);
				cups.AddAfter(pickUp[0], pickUp[1]);
				cups.AddAfter(pickUp[1], pickUp[2]);

				currentNode = currentNode.Next;
			}

			long output = (long)lookUp[1].Next.Value * lookUp[1].Next.Next.Value;

			return output.ToString();
		}

		public int Modulo(int input, int mod)
		{
			return input < 0 ? (input % mod + mod) % mod : input % mod;
		}
	}
}