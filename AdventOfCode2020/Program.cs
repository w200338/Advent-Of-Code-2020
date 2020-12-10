using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	public class Program
	{
		static async Task Main(string[] args)
		{
			int day;

			// auto select day in december 2020
			if (DateTime.Now.Year == 2020 && DateTime.Now.Month == 12 && DateTime.Now.Day <= 25)
			{
				day = DateTime.Now.Day;
			}
			else
			{
				Console.WriteLine("Select a day");
				day = int.Parse(Console.ReadLine());
			}

			string dayName = $"Day{(day < 10 ? "0" + day : day.ToString())}";
			Type type = Type.GetType($"AdventOfCode2020.Days.{dayName}.{dayName}");
			Day currentDay = (Day)Activator.CreateInstance(type);

			// execute day
			await currentDay.ReadInput();

			Stopwatch stopwatch = Stopwatch.StartNew();
			Console.WriteLine("Part 1");
			Console.WriteLine(currentDay.Part1());
			Console.WriteLine($"took {stopwatch.Elapsed.TotalMilliseconds} ms");

			Console.WriteLine();

			stopwatch = Stopwatch.StartNew();
			Console.WriteLine("Part 2");
			Console.WriteLine(currentDay.Part2());
			Console.WriteLine($"took {stopwatch.Elapsed.TotalMilliseconds} ms");

			Console.Write("\nPress any key to quit");
			Console.ReadKey();
		}
	}
}
