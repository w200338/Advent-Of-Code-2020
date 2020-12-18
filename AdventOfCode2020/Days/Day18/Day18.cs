using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2020.Days.Day18
{
	public class Day18 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			BigInteger sum = 0;
			foreach (string line in lines)
			{
				sum += Calculate(line.Replace("(", "( ").Replace(")", " )", System.StringComparison.OrdinalIgnoreCase));
			}

			return sum.ToString();

			// not 10817561338
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			BigInteger sum = 0;
			foreach (string line in lines)
			{
				sum += CalculatePart2(line.Replace("(", "( ").Replace(")", " )", System.StringComparison.OrdinalIgnoreCase));
			}

			return sum.ToString();
			// not 28718426
		}

		public long CalculatePart2(string line)
		{
			List<string> lineParts = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).ToList();

			//long lastNumber = 0;
			//char mathOperator = '+';

			// handle brackets
			if (line.Contains('('))
			{
				int bracketDepth = 0;
				int bracketStart = 0;
				for (int i = lineParts.IndexOf("("); i < lineParts.Count; i++)
				{
					for (int j = i; j < lineParts.Count; j++)
					{
						if (lineParts[j].Trim()[0] == '(')
						{
							if (bracketDepth == 0)
							{
								bracketStart = j;
							}

							bracketDepth++;
						}
						else if (lineParts[j].Trim()[0] == ')')
						{
							bracketDepth--;

							if (bracketDepth == 0)
							{
								long betweenBrackets = CalculatePart2(lineParts.GetRange(bracketStart + 1, j - bracketStart - 1).Aggregate((concat, str) => $"{concat} {str}"));
								lineParts.RemoveRange(bracketStart, j - bracketStart + 1);
								lineParts.Insert(bracketStart, betweenBrackets.ToString());
								break;
							}
						}
					}
				}
			}

			// handle +
			if (line.Contains("+"))
			{
				for (int i = 1; i < lineParts.Count; i++)
				{
					if (lineParts[i] == "+")
					{
						lineParts[i + 1] = (long.Parse(lineParts[i - 1]) + long.Parse(lineParts[i + 1])).ToString();

						lineParts.RemoveAt(i);
						lineParts.RemoveAt(i - 1);
						i--;
					}
				}
			}

			if (lineParts.Count == 1)
			{
				return long.Parse(lineParts[0]);
			}

			long total = 1;
			for (int i = 0; i < lineParts.Count; i++)
			{
				if (lineParts[i] == "(" || lineParts[i] == ")" || lineParts[i] == "+")
				{
					Console.WriteLine("Uh oh");
				}
				else if (lineParts[i] != "*")
				{
					total *= long.Parse(lineParts[i]);
				}
			}

			return total;
			/*
			for (int i = 0; i < lineParts.Count; i++)
			{
				if (lineParts[i].Trim()[0] == '+')
				{
					mathOperator = '+';
				}
				else if (lineParts[i].Trim()[0] == '*')
				{
					mathOperator = '*';
				}
				else if (lineParts[i].Trim()[0] == '(')
				{
					// find corresponding closing bracket at depth 0
					int depth = 0;
					for (int j = i; j < lineParts.Count; j++)
					{
						if (lineParts[j].Trim()[0] == '(')
						{
							depth++;
						}
						else if (lineParts[j].Trim()[0] == ')')
						{
							depth--;
						}

						if (depth == 0)
						{
							long valueBetweenBrackets = Calculate(lineParts.GetRange(i + 1, j - i - 1).Aggregate((concat, str) => $"{concat} {str}"));
							lineParts.RemoveRange(i, j - i + 1);

							if (i + 1 >= lineParts.Count)
							{
								lineParts.Add(valueBetweenBrackets.ToString());
								i = lineParts.Count - 2;
							}
							else
							{
								lineParts.Insert(i, valueBetweenBrackets.ToString());
								i--;
							}
							break;
						}
					}
				}
				else
				{
					int currentNumber = int.Parse(lineParts[i].Trim());

					if (mathOperator == '+')
					{
						lastNumber += currentNumber;
					}
					else
					{
						if (lastNumber == 0)
						{
							lastNumber = 1;
						}

						lastNumber *= currentNumber;
					}
				}
			}

			return lastNumber;
			*/
		}

		public long Calculate(string line)
		{
			/*
			if (line.Contains('('))
			{
				int openingBracketPos = line.IndexOf('(');
				int closingBracketPos = line.LastIndexOf(')');

				int valueBetweenBrackets = Calculate(line.Substring(openingBracketPos + 1, closingBracketPos - openingBracketPos - 2));

				line = line.Remove(openingBracketPos, closingBracketPos - openingBracketPos);
				line = line.Insert(openingBracketPos, valueBetweenBrackets.ToString());
			}
			*/

			List<string> lineParts = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).ToList();

			long lastNumber = 0;
			char mathOperator = '+';
			for (int i = 0; i < lineParts.Count; i++)
			{
				if (lineParts[i].Trim()[0] == '+')
				{
					mathOperator = '+';
				} 
				else if (lineParts[i].Trim()[0] == '*')
				{
					mathOperator = '*';
				}
				else if (lineParts[i].Trim()[0] == '(')
				{
					// find corresponding closing bracket at depth 0
					int depth = 0;
					for (int j = i; j < lineParts.Count; j++)
					{
						if (lineParts[j].Trim()[0] == '(')
						{
							depth++;
						}
						else if (lineParts[j].Trim()[0] == ')')
						{
							depth--;
						}

						if (depth == 0)
						{
							long valueBetweenBrackets = Calculate(lineParts.GetRange(i + 1, j - i - 1).Aggregate((concat, str) => $"{concat} {str}"));
							lineParts.RemoveRange(i, j - i + 1);

							if (i + 1 >= lineParts.Count)
							{
								lineParts.Add(valueBetweenBrackets.ToString());
								i = lineParts.Count - 2;
							}
							else
							{
								lineParts.Insert(i, valueBetweenBrackets.ToString());
								i--;
							}
							break;
						}
					}
				}
				else
				{
					int currentNumber = int.Parse(lineParts[i].Trim());

					if (mathOperator == '+')
					{
						lastNumber += currentNumber;
					}
					else
					{
						if (lastNumber == 0)
						{
							lastNumber = 1;
						}

						lastNumber *= currentNumber;
					}
				}
			}

			return lastNumber;
		}
	}
}