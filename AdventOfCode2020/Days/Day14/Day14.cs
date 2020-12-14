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
			Dictionary<long, long> memory = new Dictionary<long, long>();

			foreach (string line in lines)
			{
				// new mask
				if (line.Substring(0, 4) == "mask")
				{
					maskBits = new List<MaskBit>();

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
					List<MaskBitStruct> memoryBits = new List<MaskBitStruct>();
					for (int i = maskBits.Count - 1; i >= 0; i--)
					{
						switch (maskBits[i].Value)
						{
							case MaskBitValue.None:
								memoryBits.Add(new MaskBitStruct(MaskBitValue.None));

								break;
							case MaskBitValue.Zero:
								memoryBits.Add(new MaskBitStruct(((index & ((long)0b1 << i)) >> i) == 1 ? MaskBitValue.One : MaskBitValue.Zero));

								// add nothing
								break;
							case MaskBitValue.One:
								memoryBits.Add(new MaskBitStruct(MaskBitValue.One));
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}

					//long totalExpectedAddresses = 1 << memoryBits.Count(memoryBits => memoryBits.Value == MaskBitValue.None);

					//HashSet<long> visitedAddresses = new HashSet<long>();
					foreach (List<MaskBitStruct> bits in CreatePermutations(memoryBits))
					{
						long currentIndex = MaskToInt(bits);
						//visitedAddresses.Add(currentIndex);

						if (memory.ContainsKey(currentIndex))
						{
							memory[currentIndex] = givenValue;
						}
						else
						{
							memory.Add(currentIndex, givenValue);
						}

						/*
						if (visitedAddresses.Count == totalExpectedAddresses)
						{
							break;
						}
						*/
					}
				}
			}

			// count total
			BigInteger total = 0;
			foreach (KeyValuePair<long, long> kv in memory)
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

		public struct MaskBitStruct
		{
			public MaskBitValue Value { get; set; }

			public MaskBitStruct(MaskBitValue maskBitValue)
			{
				Value = maskBitValue;
			}

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

		public long MaskToInt(List<MaskBitStruct> maskBits)
		{
			long output = 0;
			//maskBits.Reverse();

			for (int i = 0; i < maskBits.Count; i++)
			{
				output += maskBits[i].Value == MaskBitValue.One ? 1 : 0;
				output <<= 1;
			}

			output >>= 1;

			return output;
		}

		public IEnumerable<List<MaskBitStruct>> CreatePermutations(List<MaskBitStruct> maskBits)
		{
			if (maskBits.All(m => m.Value != MaskBitValue.None))
			{
				yield return maskBits;
				yield break;
				//output.Add(maskBits);
			}

			for (int i = maskBits.Count - 1; i >= 0; i--)
			{
				if (maskBits[i].Value == MaskBitValue.None)
				{
					List<MaskBitStruct> copyOfMask1 = maskBits.Select(m => new MaskBitStruct(m.Value)).ToList();
					List<MaskBitStruct> copyOfMask2 = maskBits.Select(m => new MaskBitStruct(m.Value)).ToList();
					copyOfMask1[i] = new MaskBitStruct(MaskBitValue.Zero);
					copyOfMask2[i] = new MaskBitStruct(MaskBitValue.One);

					foreach (List<MaskBitStruct> permutation in CreatePermutations(copyOfMask1))
					{
						yield return permutation;
					}

					foreach (List<MaskBitStruct> permutation in CreatePermutations(copyOfMask2))
					{
						yield return permutation;
					}

					yield break;
				}
			}

			//return output;
		}
	}
}