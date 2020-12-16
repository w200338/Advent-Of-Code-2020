using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day16
{
	public class Day16 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			int inputIndex = 0;

			// read ranges
			List<DoubleRange> validNumberRanges = new List<DoubleRange>();
			while(!string.IsNullOrWhiteSpace(lines[inputIndex]))
			{
				string[] lineParts = lines[inputIndex].Split(' ');

				string secondRange = lineParts[lineParts.Length - 3];
				string firstRange = lineParts[lineParts.Length - 1];

				string[] secondRangeParts = secondRange.Split('-');
				string[] firstRangeParts = firstRange.Split('-');

				validNumberRanges.Add(new DoubleRange()
				{
					Range1 = new Range()
					{
						Minimum = int.Parse(firstRangeParts[0]),
						Maximum = int.Parse(firstRangeParts[1])
					},
					Range2 = new Range()
					{
						Minimum = int.Parse(secondRangeParts[0]),
						Maximum = int.Parse(secondRangeParts[1])
					}
				});

				inputIndex++;
			}

			// skip over own ticket
			inputIndex += 5;

			return lines.Skip(inputIndex)
				.Select(l => l.Split(','))
				.SelectMany(s => s)
				.Select(int.Parse)
				.Where(i => validNumberRanges.All(vr => !vr.IsInRange(i)))
				.Sum().ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			int inputIndex = 0;

			// read ranges
			List<DoubleRange> validNumberRanges = new List<DoubleRange>();
			while (!string.IsNullOrWhiteSpace(lines[inputIndex]))
			{
				string[] lineParts = lines[inputIndex].Split(' ');

				string secondRange = lineParts[lineParts.Length - 3];
				string firstRange = lineParts[lineParts.Length - 1];

				string[] secondRangeParts = secondRange.Split('-');
				string[] firstRangeParts = firstRange.Split('-');

				validNumberRanges.Add(new DoubleRange()
				{
					Name = lines[inputIndex].Split(':')[0],

					Range1 = new Range()
					{
						Minimum = int.Parse(firstRangeParts[0]),
						Maximum = int.Parse(firstRangeParts[1])
					},
					Range2 = new Range()
					{
						Minimum = int.Parse(secondRangeParts[0]),
						Maximum = int.Parse(secondRangeParts[1])
					}
				});

				inputIndex++;
			}

			// get own ticket
			inputIndex += 2;
			List<int> ownTicket = lines[inputIndex].Split(',').Select(int.Parse).ToList();
			inputIndex += 3;

			// find all tickets with valid fields
			List<List<int>> validTickets = new List<List<int>>();

			for (int i = inputIndex; i < lines.Length; i++)
			{
				List<int> fields = lines[i].Split(',').Select(int.Parse).ToList();
				if (fields.Count(integer => validNumberRanges.All(vr => !vr.IsInRange(integer))) == 0)
				{
					validTickets.Add(fields);
				}
			}

			// attempt to figure out which field is which
			// column index and all of their possible fields
			Dictionary<int, List<string>> possibleFieldsOfAll = new Dictionary<int, List<string>>();
			for (int i = 0; i < validTickets[0].Count; i++) // iterate columns
			{
				List<string> possibleFields = validNumberRanges.Select(vr => vr.Name).ToList();

				foreach (List<int> validTicket in validTickets)
				{
					foreach (DoubleRange range in validNumberRanges)
					{
						if (!range.IsInRange(validTicket[i]))
						{
							possibleFields.Remove(range.Name);
						}
					}
				}

				possibleFieldsOfAll.Add(i, possibleFields);
			}

			// sift through results to find only matching field for each column
			Dictionary<int, string> fieldOfIndex = new Dictionary<int, string>();

			int columns = possibleFieldsOfAll.Count;
			for (int i = 0; i < columns; i++)
			{
				// find pair with only 1 possibility
				KeyValuePair<int, List<string>> singlePair = possibleFieldsOfAll.First(kv => kv.Value.Count == 1);

				// it's guaranteed to be correct
				fieldOfIndex.Add(singlePair.Key, singlePair.Value[0]);

				// remove it as a possibility from all others
				foreach (KeyValuePair<int, List<string>> removePair in possibleFieldsOfAll.Where(kv => kv.Value.Count > 1))
				{
					removePair.Value.Remove(singlePair.Value[0]);
				}

				// remove itself from the list to check
				possibleFieldsOfAll.Remove(singlePair.Key);
			}

			/*
			Dictionary<int, string> fieldOfIndex = new Dictionary<int, string>();
			foreach (KeyValuePair<int, List<string>> keyValuePair in possibleFieldsOfAll)
			{
				string unique = keyValuePair.Value
					.Except(possibleFieldsOfAll.Except(new[] {keyValuePair})
						.Select(kv => kv.Value).SelectMany(v => v))
					.First();

				fieldOfIndex.Add(keyValuePair.Key, unique);
			}
			*/

			// check if there are any duplicates
			long total = 1;
			foreach (KeyValuePair<int, string> keyValuePair in fieldOfIndex)
			{
				if (keyValuePair.Value.ToLower().Contains("departure"))
				{
					total *= ownTicket[keyValuePair.Key];
				}
			}

			return total.ToString();

			// not 105540553
		}
	}

	public class Range
	{
		public int Minimum { get; set; }
		public int Maximum { get; set; }

		public bool IsInRange(int input)
		{
			return input >= Minimum && input <= Maximum;
		}
	}

	public class DoubleRange
	{
		public string Name { get; set; }

		public Range Range1 { get; set; }
		public Range Range2 { get; set; }

		public bool IsInRange(int input)
		{
			return Range1.IsInRange(input) || Range2.IsInRange(input);
		}
	}
}