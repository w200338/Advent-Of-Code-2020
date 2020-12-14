using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2020.Days.Day14
{
	public class Day14 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			List<MaskBit> maskBits = new List<MaskBit>();
			Dictionary<int, long> memory = new Dictionary<int, long>();

			foreach (string line in lines)
			{
				// new mask
				if (line.Substring(0, 4) == "mask")
				{
					string mask = line.Split(" = ")[1];
					foreach (char c in mask)
					{
						switch (c)
						{
							case '0':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.Zero
								});
								break;

							case '1':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.One
								});
								break;

							case 'X':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.None
								});
								break;
						}
					}

					maskBits.Reverse();
				}
				else
				{
					int index = int.Parse(line.Substring(4, line.IndexOf(']') - 4));
					long givenValue = long.Parse(line.Split(" = ")[1]);

					long output = 0;

					for (int i = 35; i >= 0; i--)
					{
						switch (maskBits[i].Value)
						{
							case MaskBitValue.None:
								output += givenValue & ((long)0b1 << i);
								break;
							case MaskBitValue.Zero:
								// add nothing
								break;
							case MaskBitValue.One:
								output += 1L << i;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						//givenValue >>= 1;
						//output <<= 1;
					}

					if (memory.ContainsKey(index))
					{
						memory[index] = output;
					}
					else
					{
						memory.Add(index, output);
					}
				}
			}

			long total = 0;
			foreach (KeyValuePair<int, long> kv in memory)
			{
				total += kv.Value;
			}

			return total.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			List<MaskBit> maskBits = new List<MaskBit>();
			Dictionary<int, long> memory = new Dictionary<int, long>();

			foreach (string line in lines)
			{
				// new mask
				if (line.Substring(0, 4) == "mask")
				{
					string mask = line.Split(" = ")[1];
					foreach (char c in mask)
					{
						switch (c)
						{
							case '0':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.Zero
								});
								break;

							case '1':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.One
								});
								break;

							case 'X':
								maskBits.Add(new MaskBit()
								{
									Value = MaskBitValue.None
								});
								break;
						}
					}

					maskBits.Reverse();
				}
				else
				{
					int index = int.Parse(line.Substring(4, line.IndexOf(']') - 4));
					long givenValue = long.Parse(line.Split(" = ")[1]);

					// find all memory addresses
					List<MaskBit> memoryBits = new List<MaskBit>();
					for (int i = 35; i >= 0; i--)
					{
						switch (maskBits[i].Value)
						{
							case MaskBitValue.None:
								memoryBits.Add(new MaskBit()
								{
									Value = MaskBitValue.None
								});

								break;
							case MaskBitValue.Zero:
								memoryBits.Add(new MaskBit()
								{
									Value = ((index & (0b1 << i)) >> i) == 1 ? MaskBitValue.One : MaskBitValue.Zero
								});

								// add nothing
								break;
							case MaskBitValue.One:
								memoryBits.Add(new MaskBit()
								{
									Value = MaskBitValue.One
								});
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}

					foreach (List<MaskBit> bits in CreatePermutations(memoryBits))
					{
						int currentIndex = MaskToInt(bits);

						if (memory.ContainsKey(currentIndex))
						{
							memory[currentIndex] = givenValue;
						}
						else
						{
							memory.Add(currentIndex, givenValue);
						}
					}
				}
			}

			// count total
			BigInteger total = 0;
			foreach (KeyValuePair<int, long> kv in memory)
			{
				total += kv.Value;
			}

			return total.ToString();
			// higher than 287181563454
		}

		public class MaskBit
		{
			public MaskBitValue Value { get; set; } = MaskBitValue.None;

			public override string ToString()
			{
				return Value.ToString();
			}
		}

		public enum MaskBitValue : byte
		{
			None,
			Zero,
			One
		}

		public int MaskToInt(List<MaskBit> maskBits)
		{
			long output = 0;
			maskBits.Reverse();

			for (int i = 0; i < maskBits.Count; i++)
			{
				output += maskBits[i].Value == MaskBitValue.One ? 1 : 0;
				output <<= 1;
			}

			output >>= 1;

			return (int)output;
		}

		public List<List<MaskBit>> CreatePermutations(List<MaskBit> maskBits)
		{
			List<List<MaskBit>> output = new List<List<MaskBit>>();

			for (int i = maskBits.Count - 1; i >= 0; i--)
			{
				if (maskBits[i].Value == MaskBitValue.None)
				{
					List<MaskBit> copyOfMask1 = maskBits.Select(m => new MaskBit() { Value = m.Value }).ToList();
					List<MaskBit> copyOfMask2 = maskBits.Select(m => new MaskBit() { Value = m.Value }).ToList();
					copyOfMask1[i].Value = MaskBitValue.One;
					copyOfMask2[i].Value = MaskBitValue.Zero;

					output.AddRange(CreatePermutations(copyOfMask1));
					output.AddRange(CreatePermutations(copyOfMask2));
				}
			}

			if (maskBits.All(m => m.Value != MaskBitValue.None))
			{
				output.Add(maskBits);
			}

			return output;
		}
	}
}