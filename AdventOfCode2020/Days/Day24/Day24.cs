using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day24
{
	public class Day24 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			List<Vector2Int> foundTiles = new List<Vector2Int>();

			foreach (string line in lines)
			{
				Vector2Int position = Vector2Int.Zero;

				for (int i = 0; i < line.Length; i++)
				{
					// https://math.stackexchange.com/questions/2254655/hexagon-grid-coordinate-system
					// axial coordinates
					switch (line[i])
					{
						case 'e':
							//position += new Vector2Int(1, 0);
							position += new Vector2Int(1, 0);
							break;

						case 'w':
							//position += new Vector2Int(-1, 0);
							position += new Vector2Int(-1, 0);
							break;

						case 'n':
							i++;

							if (line[i] == 'e')
							{
								//position += new Vector2Int(1, -1);
								position += new Vector2Int(1, -1);
							}
							else
							{
								//position += new Vector2Int(-1, -1);
								position += new Vector2Int(0, -1);
							}
							break;

						case 's':
							i++;

							if (line[i] == 'e')
							{
								//position += new Vector2Int(1, 1);
								position += new Vector2Int(0, 1);
							}
							else
							{
								//position += new Vector2Int(-1, 1);
								position += new Vector2Int(-1, 1);
							}
							break;
					}
				}

				foundTiles.Add(position);
			}

			return foundTiles.GroupBy(p => p)
				.Count(p => p.Count() % 2 != 0)
				.ToString();

			return "not implemented";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			List<Vector2Int> blackTiles = new List<Vector2Int>();

			foreach (string line in lines)
			{
				Vector2Int position = Vector2Int.Zero;

				for (int i = 0; i < line.Length; i++)
				{
					switch (line[i])
					{
						case 'e':
							//position += new Vector2Int(1, 0);
							position += new Vector2Int(1, 0);
							break;

						case 'w':
							//position += new Vector2Int(-1, 0);
							position += new Vector2Int(-1, 0);
							break;

						case 'n':
							i++;

							if (line[i] == 'e')
							{
								//position += new Vector2Int(1, -1);
								position += new Vector2Int(1, -1);
							}
							else
							{
								//position += new Vector2Int(-1, -1);
								position += new Vector2Int(0, -1);
							}
							break;

						case 's':
							i++;

							if (line[i] == 'e')
							{
								//position += new Vector2Int(1, 1);
								position += new Vector2Int(0, 1);
							}
							else
							{
								//position += new Vector2Int(-1, 1);
								position += new Vector2Int(-1, 1);
							}
							break;
					}
				}

				blackTiles.Add(position);
			}

			// initial black tiles
			blackTiles = blackTiles.GroupBy(p => p)
				.Where(p => p.Count() % 2 != 0)
				.Select(p => p.Key)
				.ToList();

			Console.WriteLine($"Day {0}: {blackTiles.Count}");

			for (int i = 0; i < 100; i++)
			{
				List<Vector2Int> newBlackTiles = new List<Vector2Int>();

				// black -> ?
				foreach (Vector2Int pos in blackTiles)
				{
					List<Vector2Int> hexagonalNeighbours = GetHexagonalNeighbours(pos);
					int activeNeighbours = hexagonalNeighbours.Count(tile => blackTiles.Contains(tile));

					if (activeNeighbours != 0 && activeNeighbours <= 2)
					{
						newBlackTiles.Add(pos);
					}
				}

				// get all neighbours of all black tiles which are white
				List<Vector2Int> neighboursToCheck = blackTiles
					.Select(GetHexagonalNeighbours)
					.SelectMany(l => l)
					.Distinct()
					.Except(blackTiles)
					.ToList();

				// white -> black
				foreach (Vector2Int pos in neighboursToCheck)
				{
					List<Vector2Int> hexagonalNeighbours = GetHexagonalNeighbours(pos);
					int activeNeighbours = hexagonalNeighbours.Count(tile => blackTiles.Contains(tile));

					if (activeNeighbours == 2)
					{
						newBlackTiles.Add(pos);
					}
				}


				blackTiles = newBlackTiles;
				Console.WriteLine($"Day {i + 1}: {blackTiles.Count}");
			}

			return blackTiles.Count.ToString();
		}

		public List<Vector2Int> GetHexagonalNeighbours(Vector2Int pos)
		{
			return offsets.Select(o => pos + o)
				.ToList();
		}

		private static List<Vector2Int> offsets = new List<Vector2Int>()
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(1, -1),
			new Vector2Int(0, -1),
			new Vector2Int(0, 1),
			new Vector2Int(-1, 1)
		};
	}
}