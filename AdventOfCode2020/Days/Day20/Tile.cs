using System.Text;

namespace AdventOfCode2020.Days.Day20
{
	public class Tile
	{
		public int Rotation { get; set; }
		public bool FlippedHorizontal { get; set; }

		public int Id { get; set; }

		public string Top
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					stringBuilder.Append(this[0, i]);
				}

				return stringBuilder.ToString();
			}
		}

		public string Bottom
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					stringBuilder.Append(this[Data.Length - 1, i]);
				}

				return stringBuilder.ToString();
			}
		}

		public string Left
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					stringBuilder.Append(this[i, 0]);
				}

				return stringBuilder.ToString();
			}
		}

		public string Right
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					stringBuilder.Append(this[i, Data.Length - 1]);
				}

				return stringBuilder.ToString();
			}
		}

		public string Row(int row)
		{
			StringBuilder stringBuilder = new StringBuilder(Data.Length);
			for (int i = 0; i < Data.Length; i++)
			{
				stringBuilder.Append(this[row, i]);
			}

			return stringBuilder.ToString();
		}

		public string[] Data { get; set; }

		public char this[int x, int y]
		{
			get
			{
				// rotate
				for (int i = 0; i < Rotation; i++)
				{
					int oldX = x;
					x = y;
					y = Data.Length - 1 - oldX;
				}

				// flip
				if (FlippedHorizontal)
				{
					y = Data.Length - y - 1;
				}

				return Data[y][x];
			}
		}

		public State State
		{
			get => new State(Id, Rotation, FlippedHorizontal);

			set
			{
				Reset();

				for (int i = 0; i < value.Rotation; i++)
				{
					RotateRight();
				}

				if (value.FlippedHorizontally)
				{
					FlipHorizontalAxis();
				}
			}
		}
		public void Reset()
		{
			Rotation = 0;
			FlippedHorizontal = false;
		}

		public void RotateRight()
		{
			Rotation = ++Rotation % 4;
		}

		public void FlipHorizontalAxis()
		{
			FlippedHorizontal = !FlippedHorizontal;
		}

		protected bool Equals(Tile other)
		{
			return Id == other.Id;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Tile)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Id;
		}
	}
}