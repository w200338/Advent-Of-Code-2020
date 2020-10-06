using System;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	public class Program
	{
		static async Task Main(string[] args)
		{
			Day[] days = new Day[25];
			for (int i = 1; i <= 25; i++)
			{
				string dayName = $"Day{(i < 10 ? "0" + i : i.ToString())}";
				Type type = Type.GetType($"AdventOfCode2020.Days.{dayName}.{dayName}");
				days[i - 1] = (Day) Activator.CreateInstance(type);
			}

			Day currentDay;

			// auto select day in december 2020
			if (DateTime.Now.Year == 2020 && DateTime.Now.Month == 12)
			{
				currentDay = days[DateTime.Now.Day];
			}
			else
			{
				Console.WriteLine("Select a day");
				currentDay = days[int.Parse(Console.ReadLine())];
			}

			// execute day
			await currentDay.ReadInput();

			Console.WriteLine("Part 1");
			Console.WriteLine(currentDay.Part1());

			Console.WriteLine("Part 2");
			Console.WriteLine(currentDay.Part2());

			Console.Write("\nPress any key to quit");
			Console.ReadKey();
		}
	}
}
