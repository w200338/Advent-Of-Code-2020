using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Days.Day04
{
	public class Day04 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> passPorts = Input.Split("\r\n\r\n").Select(p => p.Replace("\r\n", " ")).ToList();

			int valid = 0;
			foreach (string passPort in passPorts)
			{
				if (passPort.Contains("byr") && passPort.Contains("iyr") && passPort.Contains("eyr") && passPort.Contains("hgt") && passPort.Contains("hcl") && passPort.Contains("ecl") && passPort.Contains("pid")) // && passPort.Contains("cid")
				{
					valid++;
				}
			}

			return valid.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			Regex colorRegex = new Regex(@"^#([0-9a-f]){6}$");
			string[] validColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

			List<string> passPorts = Input.Split("\r\n\r\n").Select(p => p.Replace("\r\n", " ")).ToList();

			int valid = 0;
			foreach (string passPort in passPorts)
			{
				if (passPort.Contains("byr") && passPort.Contains("iyr") && passPort.Contains("eyr") && passPort.Contains("hgt") && passPort.Contains("hcl") && passPort.Contains("ecl") && passPort.Contains("pid"))
				{
					string[] fields = passPort.Split(' ');

					try
					{
						int birthYear = int.Parse(fields.First(fields => fields.Contains("byr")).Split(':')[1]);
						if (birthYear >= 1920 && birthYear <= 2002)
						{
							int issueYear = int.Parse(fields.First(fields => fields.Contains("iyr")).Split(':')[1]);
							if (issueYear >= 2010 && issueYear <= 2020)
							{
								int exprYear = int.Parse(fields.First(fields => fields.Contains("eyr")).Split(':')[1]);
								if (exprYear >= 2020 && exprYear <= 2030)
								{
									string height = fields.First(fields => fields.Contains("hgt")).Split(':')[1];
									int heightAmount = int.Parse(new string(height.TakeWhile(c => char.IsDigit(c)).ToArray()));
									string unit = new string(height.TakeLast(2).ToArray());

									if (unit == "cm")
									{
										if (heightAmount < 150 || heightAmount > 193)
										{
											continue;
										}
									}
									else if (unit == "in")
									{
										if (heightAmount < 53 || heightAmount > 76)
										{
											continue;
										}
									}
									else
									{
										continue;
									}

									if (colorRegex.IsMatch(fields.First(fields => fields.Contains("hcl")).Split(':')[1]))
									{
										if (validColors.Contains(fields.First(fields => fields.Contains("ecl")).Split(':')[1]))
										{
											string passportId = fields.First(fields => fields.Contains("pid")).Split(':')[1];
											if (passportId.Length == 9 && passportId.All(char.IsDigit))
											{
												valid++;
											}
										}
									}
								}
							}
						}
					}
					catch (Exception)
					{

					}
				}
			}

			return valid.ToString();
		}
	}
}