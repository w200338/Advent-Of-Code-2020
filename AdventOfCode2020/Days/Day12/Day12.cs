using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Tools.Enums;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day12
{
	public class Day12 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			List<Vector> vectors = new List<Vector>();
			vectors.Add(new Vector()
			{
				Position = Vector2Int.Zero,
				Length = 0,
				Direction = Compass.East
			});

			Compass currentDir = Compass.East;

			foreach (string line in lines)
			{
				char instruction = line[0];
				int amount = int.Parse(line.Substring(1));

				int turns;
				switch (instruction)
				{
					case 'N':
						vectors.Add(new Vector()
						{
							Direction = Compass.North,
							Length = amount,
							Position = Move(vectors.Last())
						});
						break;

					case 'E':
						vectors.Add(new Vector()
						{
							Direction = Compass.East,
							Length = amount,
							Position = Move(vectors.Last())
						});
						break;

					case 'S':
						vectors.Add(new Vector()
						{
							Direction = Compass.South,
							Length = amount,
							Position = Move(vectors.Last())
						});
						break;

					case 'W':
						vectors.Add(new Vector()
						{
							Direction = Compass.West,
							Length = amount,
							Position = Move(vectors.Last())
						});
						break;

					case 'F':
						vectors.Add(new Vector()
						{
							Direction = currentDir,
							Length = amount,
							Position = Move(vectors.Last())
						});
						break;

					case 'R':
						turns = amount / 90;

						for (int i = 0; i < turns; i++)
						{
							currentDir = EnumUtils.TurnRight(currentDir);
						}
						break;

					case 'L':
						turns = amount / 90;

						for (int i = 0; i < turns; i++)
						{
							currentDir = EnumUtils.TurnLeft(currentDir);
						}
						break;
				}
			}

			return Vector2Int.DistanceManhattan(Move(vectors.Last()), Vector2Int.Zero).ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			List<Vector> vectors = new List<Vector>();
			vectors.Add(new Vector()
			{
				Position = Vector2Int.Zero,
				Length = 0,
				Direction = Compass.East
			});

			Compass currentDir = Compass.East;
			Vector2Int wayPoint = new Vector2Int(10, -1);

			foreach (string line in lines)
			{
				char instruction = line[0];
				int amount = int.Parse(line.Substring(1));

				int turns;
				switch (instruction)
				{
					case 'N':
						wayPoint += DirectionConverter(Compass.North) * amount;
						break;

					case 'E':
						wayPoint += DirectionConverter(Compass.East) * amount;
						break;

					case 'S':
						wayPoint += DirectionConverter(Compass.South) * amount;
						break;

					case 'W':
						wayPoint += DirectionConverter(Compass.West) * amount;
						break;

					case 'F':
						vectors.Add(new Vector()
						{
							Direction = currentDir,
							Length = amount,
							Position = vectors.Last().Position + wayPoint * amount
						});
						break;

					case 'L':
						turns = amount / 90;

						for (int i = 0; i < turns; i++)
						{
							wayPoint = new Vector2Int(wayPoint.Y, -wayPoint.X);
							currentDir = EnumUtils.TurnRight(currentDir);
						}
						break;

					case 'R':
						turns = amount / 90;

						for (int i = 0; i < turns; i++)
						{
							wayPoint = new Vector2Int(-wayPoint.Y, wayPoint.X);
							currentDir = EnumUtils.TurnLeft(currentDir);
						}
						break;
				}

				Console.WriteLine($"Waypoint: {wayPoint}, current pos: {vectors.Last().Position}");
			}

			return Vector2Int.DistanceManhattan(vectors.Last().Position, Vector2Int.Zero).ToString();
			// not 44409
			// not 22387
		}

		public class Vector
		{
			public Vector2Int Position { get; set; }
			public Compass Direction { get; set; }
			public int Length { get; set; }
		}

		public Vector2Int Move(Vector vector)
		{
			return vector.Position + DirectionConverter(vector.Direction) * vector.Length;
		}

		public Vector2Int DirectionConverter(Compass compass)
		{
			switch (compass)
			{
				case Compass.North:
					return new Vector2Int(0, -1);
				case Compass.East:
					return new Vector2Int(1, 0);
				case Compass.South:
					return new Vector2Int(0, 1);
				case Compass.West:
					return new Vector2Int(-1, 0);
				default:
					throw new ArgumentOutOfRangeException(nameof(compass), compass, null);
			}
		}
	}
}