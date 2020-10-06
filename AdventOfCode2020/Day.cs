using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
	public abstract class Day
	{
		protected string Input;

		public async Task ReadInput()
		{
			using (StreamReader reader = new StreamReader($"Days/{GetType().Name}/input.txt"))
			{
				Input = await reader.ReadToEndAsync();
			}
		}

		public abstract string Part1();
		public abstract string Part2();
	}
}
