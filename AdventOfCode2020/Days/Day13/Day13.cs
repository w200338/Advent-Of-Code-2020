using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2020.Days.Day13
{
	public class Day13 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			int departTimestamp = int.Parse(lines[0]);
			List<int> buses = lines[1].Split(',').Where(s => s != "x").Select(int.Parse).ToList();

			int lowestTime = int.MaxValue;
			int lowestBus = 0;
			foreach (int bus in buses)
			{
				int deltaTime = ((departTimestamp / bus) + 1) * bus - departTimestamp;

				if (deltaTime < lowestTime)
				{
					lowestTime = deltaTime;
					lowestBus = bus;
				}
			}

			return (lowestBus * lowestTime).ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			int departTimestamp = int.Parse(lines[0]);
			List<int> buses = lines[1].Split(',')
				.Select(s => s == "x" ? "1" : s)
				.Select(int.Parse)
				.ToList();

			/* eta: not in my lifetime
			for (long i = 1; i < long.MaxValue; i++)
			{
				long target = buses[0] * i;
				bool found = true;

				for (int j = 1; j < buses.Count; j++)
				{
					long nextValue = NextValueOver(target + j - 1, buses[j]);
					if (nextValue - target != j)
					{
						found = false;
						break;
					}
				}

				if (found)
				{
					return target.ToString();
				}
			}
			*/

			BigInteger[] a = new BigInteger[buses.Count];
			a[0] = BigInteger.Zero;

			for (int i = 1; i < buses.Count; i++)
			{
				a[i] = Modulo(-i, buses[i]);
			}

			return ChineseRemainderTheorem(buses.Select(b => new BigInteger(b)).ToArray(), a).ToString();
		}

		public long NextValueOver(long target, long baseValue)
		{
			return ((target / baseValue) + 1) * baseValue;
		}

		public int GreatestCommonDivider(int a, int b)
		{
			if (b == 0)
			{
				return a;
			}

			return GreatestCommonDivider(b, a % b);
		}

		public int LeastCommonMultiple(int a, int b)
		{
			return Math.Abs(a * b) / GreatestCommonDivider(a, b);
		}

		// https://en.wikipedia.org/wiki/Chinese_remainder_theorem
		// shamelessly adapted from: https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
		public BigInteger ChineseRemainderTheorem(BigInteger[] divisors, BigInteger[] a)
		{
			BigInteger totalProduct = divisors.Aggregate(1, (BigInteger i, BigInteger j) => i * j);
			BigInteger p;
			BigInteger sm = 0;

			for (int i = 0; i < divisors.Length; i++)
			{
				p = totalProduct / divisors[i];
				sm += a[i] * ModularMultiplicativeInverse(p, divisors[i]) * p;
			}

			return sm % totalProduct;
		}

		public BigInteger ModularMultiplicativeInverse(BigInteger a, BigInteger mod)
		{
			BigInteger b = a % mod;

			for (BigInteger x = 1; x < mod; x++)
			{
				if ((b * x) % mod == 1)
				{
					return x;
				}
			}

			return 1;
		}

		public int Modulo(int x, int m)
		{
			return (x % m + m) % m;
		}
	}
}