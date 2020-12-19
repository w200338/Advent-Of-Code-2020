using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2020.Days.Day19
{
	public class Day19 : Day
	{
		public static List<IRule> Rules;

		/// <inheritdoc />
		public override string Part1()
		{
			string[] inputParts = Input.Split("\r\n\r\n");

			List<string> ruleLines = inputParts[0].Split("\r\n").ToList();

			Rules = new List<IRule>();
			foreach (string ruleLine in ruleLines)
			{
				string[] ruleParts = ruleLine.Split(':');
				
				int ruleIndex = int.Parse(ruleParts[0]);

				// single char
				if (ruleParts[1].Contains('"'))
				{
					Rules.Add(new SingleCharRule()
					{
						RuleIndex = ruleIndex,
						Character = ruleParts[1][ruleParts[1].IndexOf('"') + 1]
					});
				}
				// choice
				else if (ruleParts[1].Contains('|'))
				{
					string[] options = ruleParts[1].Split('|');
					List<int> options1 = options[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
					List<int> options2 = options[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

					Rules.Add(new ChoiceRule()
					{
						RuleIndex = ruleIndex,
						RuleOne = new OrderRule()
						{
							RuleIndex = -1,
							RuleIndexes = options1
						},
						RuleTwo = new OrderRule()
						{
							RuleIndex = -1,
							RuleIndexes = options2
						}
					});
				}
				// order
				else
				{
					int[] ruleIndexes = ruleParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
					Rules.Add(new OrderRule()
					{
						RuleIndex = ruleIndex,
						RuleIndexes = ruleIndexes.ToList()
					});
				}
			}

			string[] messages = inputParts[1].Split("\r\n");
			int correct = 0;
			foreach (string message in messages)
			{
				(bool, int) result = Rules.First(r => r.RuleIndex == 0).IsMatch(message);
				
				if (result.Item1 && result.Item2 == message.Length)
				{
					correct++;
					Console.WriteLine(message);
				}
			}

			return correct.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] inputParts = Input.Split("\r\n\r\n");

			List<string> ruleLines = inputParts[0]
				.Replace("\r\n8: 42", "")
				.Replace("\r\n11: 42 31", "")
				.Split("\r\n")
				.ToList();

			Rules = new List<IRule>() {new Rule8(), new Rule11()};
			foreach (string ruleLine in ruleLines)
			{
				string[] ruleParts = ruleLine.Split(':');

				int ruleIndex = int.Parse(ruleParts[0]);

				// single char
				if (ruleParts[1].Contains('"'))
				{
					Rules.Add(new SingleCharRule()
					{
						RuleIndex = ruleIndex,
						Character = ruleParts[1][ruleParts[1].IndexOf('"') + 1]
					});
				}
				// choice
				else if (ruleParts[1].Contains('|'))
				{
					string[] options = ruleParts[1].Split('|');
					List<int> options1 = options[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
					List<int> options2 = options[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

					Rules.Add(new ChoiceRule()
					{
						RuleIndex = ruleIndex,
						RuleOne = new OrderRule()
						{
							RuleIndex = -1,
							RuleIndexes = options1
						},
						RuleTwo = new OrderRule()
						{
							RuleIndex = -1,
							RuleIndexes = options2
						}
					});
				}
				// order
				else
				{
					int[] ruleIndexes = ruleParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
					Rules.Add(new OrderRule()
					{
						RuleIndex = ruleIndex,
						RuleIndexes = ruleIndexes.ToList()
					});
				}
			}

			string[] messages = inputParts[1].Split("\r\n");
			int correct = 0;
			IRule rule42 = Rules.First(r => r.RuleIndex == 42);
			IRule rule31 = Rules.First(r => r.RuleIndex == 31);

			foreach (string message in messages)
			{
				// rule 8
				List<(bool, int)> rule8Matches = new List<(bool, int)>();
				int offset = 0;
				while (offset < message.Length)
				{
					(bool, int) result = rule42.IsMatch(message.Substring(offset));
					if (result.Item1)
					{
						rule8Matches.Add(result);
						offset += result.Item2;
					}
					else
					{
						break;
					}
				}

				if (rule8Matches.Count == 0)
				{
					continue;
				}

				// rule 11
				offset = 0;
				foreach ((bool, int) match8 in rule8Matches)
				{
					offset += match8.Item2;

					for (int i = 1; i < 10; i++)
					{
						int internalOffset = offset;
						int amountMatch42 = 0;
						int amountMatch31 = 0;

						for (int j = 1; j <= i; j++)
						{
							(bool, int) result = rule42.IsMatch(message.Substring(internalOffset));

							if (!result.Item1)
							{
								break;
							}

							internalOffset += result.Item2;
							amountMatch42++;
						}

						if (amountMatch42 != i)
						{
							break;
						}

						for (int j = 1; j <= i; j++)
						{
							(bool, int) result = rule31.IsMatch(message.Substring(internalOffset));

							if (!result.Item1)
							{
								break;
							}

							internalOffset += result.Item2;
							amountMatch31++;
						}

						if (amountMatch31 != i)
						{
							continue;
						}

						if (internalOffset == message.Length)
						{
							correct++;
							break;
						}
					}
				}
			}

			return correct.ToString();

			// 167 too low
		}
	}
}