﻿using System;
using System.Linq;

namespace AdventOfCode2020.Tools.Mathematics.Vectors
{
	public class VectorNInt
	{
		public int[] Values { get; }

		public float Length => MathF.Sqrt(Values.Select(v => MathF.Pow(v, 2)).Sum());

		public int this[int index]
		{
			get => Values[index];
			set => Values[index] = value;
		}

		public int Dimensions => Values.Length;

		public VectorNInt(int length)
		{
			Values = new int[length];
		}

		public static VectorNInt operator +(VectorNInt a, VectorNInt b)
		{
			VectorNInt output = new VectorNInt(a.Dimensions);

			for (int i = 0; i < a.Dimensions; i++)
			{
				output[i] = a[i] + b[i];
			}

			return output;
		}

		public static VectorNInt operator -(VectorNInt a, VectorNInt b)
		{
			VectorNInt output = new VectorNInt(a.Dimensions);

			for (int i = 0; i < a.Dimensions; i++)
			{
				output[i] = a[i] - b[i];
			}

			return output;
		}

		/// <summary>
		/// Dot product
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static VectorNInt operator *(VectorNInt a, VectorNInt b)
		{
			VectorNInt output = new VectorNInt(a.Dimensions);

			for (int i = 0; i < a.Dimensions; i++)
			{
				output[i] = a[i] * b[i];
			}

			return output;
		}

		public static VectorNInt operator *(VectorNInt a, float f)
		{
			VectorNInt output = new VectorNInt(a.Dimensions);

			for (int i = 0; i < a.Dimensions; i++)
			{
				output[i] = (int)(a[i] * f);
			}

			return output;
		}

		public static VectorNInt operator /(VectorNInt a, float f)
		{
			VectorNInt output = new VectorNInt(a.Dimensions);

			for (int i = 0; i < a.Dimensions; i++)
			{
				output[i] = (int)(a[i] / f);
			}

			return output;
		}

		public static bool operator ==(VectorNInt a, VectorNInt b)
		{
			if (a == null || b == null)
			{
				return false;
			}

			return a.Equals(b);
		}

		public static bool operator !=(VectorNInt a, VectorNInt b)
		{
			if (a == null || b == null)
			{
				return false;
			}

			return !a.Equals(b);
		}


		protected bool Equals(VectorNInt other)
		{
			if (Dimensions != other.Dimensions)
			{
				return false;
			}

			for (int i = 0; i < Values.Length; i++)
			{
				if (Math.Abs(Values[i] - other[i]) > 0.001f)
				{
					return false;
				}
			}

			return false;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((VectorNInt)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (Values != null ? Values.GetHashCode() : 0);
		}
	}
}