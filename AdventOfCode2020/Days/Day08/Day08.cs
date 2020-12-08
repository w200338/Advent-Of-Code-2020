using System.Collections.Generic;

namespace AdventOfCode2020.Days.Day08
{
	public class Day08 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			List<Instruction> instructions = new List<Instruction>();
			foreach (string line in lines)
			{
				string[] lineParts = line.Split(' ');
				instructions.Add(new Instruction()
				{
					Name = lineParts[0],
					Value = int.Parse(lineParts[1])
				});
			}

			int instructionPointer = 0;
			int accumulator = 0;

			while (true)
			{
				Instruction current = instructions[instructionPointer];

				if (current.AmountExecuted == 1)
				{
					return accumulator.ToString();
				}

				switch (current.Name)
				{
					case "nop":
						instructionPointer++;
						break;
					case "acc":
						accumulator += current.Value;
						instructionPointer++;
						break;
					case "jmp":
						instructionPointer += current.Value;
						break;
				}

				current.AmountExecuted++;
			}
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			List<Instruction> instructions = new List<Instruction>();
			foreach (string line in lines)
			{
				string[] lineParts = line.Split(' ');
				instructions.Add(new Instruction()
				{
					Name = lineParts[0],
					Value = int.Parse(lineParts[1])
				});
			}

			int instructionPointer = 0;
			int accumulator = 0;

			for (int i = 0; i < instructions.Count; i++)
			{
				// skip acc
				if (instructions[i].Name == "acc")
				{
					continue;
				}

				// reset
				instructionPointer = 0;
				accumulator = 0;

				// run
				while (true)
				{
					Instruction current = instructions[instructionPointer];

					// failed
					if (current.AmountExecuted == 1)
					{
						foreach (Instruction instruction in instructions)
						{
							instruction.AmountExecuted = 0;
						}

						break;
					}

					// swap operations when needed
					if (instructionPointer != i)
					{
						switch (current.Name)
						{
							case "nop":
								instructionPointer++;
								break;
							case "acc":
								accumulator += current.Value;
								instructionPointer++;
								break;
							case "jmp":
								instructionPointer += current.Value;
								break;
						}
					}
					else
					{
						switch (current.Name)
						{
							case "jmp":
								instructionPointer++;
								break;
							case "acc":
								accumulator += current.Value;
								instructionPointer++;
								break;
							case "nop":
								instructionPointer += current.Value;
								break;
						}
					}

					// terminates regularly
					if (instructionPointer >= instructions.Count)
					{
						return accumulator.ToString();
					}

					current.AmountExecuted++;
				}

			}

			return "nothing found";
		}

		public class Instruction
		{
			public string Name { get; set; }
			public int Value { get; set; }
			public int AmountExecuted { get; set; }
		}
	}
}