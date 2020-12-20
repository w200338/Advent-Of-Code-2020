using System;

namespace AdventOfCode2020.Days.Day20
{
	public struct State
	{
		public int TileId { get; }
		public int Rotation { get; }
		public bool FlippedHorizontally { get; }

		public State(Tile currentState)
		{
			TileId = currentState.Id;
			Rotation = currentState.Rotation;
			FlippedHorizontally = currentState.FlippedHorizontal;
		}

		public State(int id, int rotation, bool flippedHorizontally)
		{
			TileId = id;
			Rotation = rotation;
			FlippedHorizontally = flippedHorizontally;
		}

		private bool Equals(State other)
		{
			return TileId == other.TileId &&
			       Rotation == other.Rotation &&
			       FlippedHorizontally == other.FlippedHorizontally;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != GetType()) return false;
			return Equals((State)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return HashCode.Combine(TileId, Rotation, FlippedHorizontally);
		}
	}
}