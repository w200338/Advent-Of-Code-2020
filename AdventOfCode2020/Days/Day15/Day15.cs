using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day15
{
	public class Day15 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<int> inputs = Input.Split(',').Select(int.Parse).ToList();
			int starting = inputs.Count;

			for (int i = 0; i < 2020; i++)
			{
				if (i < starting)
				{
					Console.WriteLine(inputs[i]);
				}
				else
				{
					if (inputs.Count(i => i == inputs.Last()) > 1)
					{
						int lastIndexOf = inputs.LastIndexOf(inputs.Last(), inputs.Count - 2);
						inputs.Add(i - lastIndexOf - 1);
					}
					// first time
					else
					{
						inputs.Add(0);
					}

					//Console.WriteLine(inputs.Last());
				}
			}

			return inputs.Last().ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<int> inputs = Input.Split(',').Select(int.Parse).ToList();
			int starting = inputs.Count;

			/*
			List<LastOccurrence> lastOccurrences = new List<LastOccurrence>();
			for (int i = 0; i < inputs.Count; i++)
			{
				lastOccurrences.Add(new LastOccurrence()
				{
					Position = i,
					Value = inputs[i]
				});
			}
			*/

			Dictionary<int, int> lastOccurence = new Dictionary<int, int>();
			for (int i = 0; i < inputs.Count; i++)
			{
				lastOccurence.Add(inputs[i], i);
			}

			int last = inputs.Last();
			for (int i = 0; i < 30000000; i++)
			{
				if (i < starting)
				{
					last = inputs[i];
					lastOccurence[last] = i;
				}
				else
				{
					int current;
					if (lastOccurence.TryGetValue(last, out int lastIndex))
					{
						current = i - lastIndex - 1;
					}
					else
					{
						current = 0;
					}

					lastOccurence[last] = i - 1;
					last = current;

					/*
					// number exists
					LastOccurrence lastOccurrence = lastOccurrences.SkipLast(1).FirstOrDefault(l => l.Value == lastOccurrences.Last().Value);
					if (lastOccurrence != null)
					{
						lastOccurrences.Add(new LastOccurrence()
						{
							Position = i,
							Value = i - lastOccurrence.Position - 1
						});

						//lastOccurrences.Remove(lastOccurrence);
					}
					else
					{
						// first time
						lastOccurrences.Add(new LastOccurrence()
						{
							Position = i,
							Value = 0,
						});
					}
					*/

					/*
					if (inputs.Count(i => i == inputs.Last()) > 1)
					{
						int lastIndexOf = inputs.LastIndexOf(inputs.Last(), inputs.Count - 2);
						inputs.Add(i - lastIndexOf - 1);
					}
					// first time
					else
					{
						inputs.Add(0);
					}
					*/
				}

				if (i % 1_000_000 == 0)
				{
					Console.WriteLine($"{i}/30000000");
				}

				//Console.WriteLine(last);
			}

			return last.ToString();
		}

		public class LastOccurrence
		{
			public int Value { get; set; }
			public int Position { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{nameof(Value)}: {Value}, {nameof(Position)}: {Position}";
			}
		}
	}
}