using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day06
{
	public class Day06 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			return Input.Split("\r\n\r\n")
				.Select(g => g.Replace("\r\n", ""))
				.Select(g => g.Distinct().Count())
				.Sum()
				.ToString();

			return "not implemented";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<string> groups = Input.Split("\r\n\r\n").ToList();
			int count = 0;

			foreach (string group in groups)
			{
				List<char> allContain = group.Split("\r\n")[0].ToCharArray().ToList();
				
				foreach (string line in group.Split("\r\n"))
				{
					List<char> removeList = new List<char>();
					foreach (char c in allContain)
					{
						if (!line.Contains(c))
						{
							removeList.Add(c);
						}
					}

					foreach (char c in removeList)
					{
						allContain.Remove(c);
					}
				}

				count += allContain.Count;
			}

			return count.ToString();
		}
	}
}