using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Tools.Mathematics._2DShapes;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day11
{
	public class Day11 : Day
	{
		//private List<Tile> tiles;
		private List<List<Tile>> tiles;
		private int width;
		private int height;

		private bool printOut1 = false;
		private bool printOut2 = false;

		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");
			height = lines.Length;
			width = lines[0].Length;

			tiles = new List<List<Tile>>();
			for (int i = 0; i < lines.Length; i++)
			{
				tiles.Add(new List<Tile>());
				for (int j = 0; j < lines[0].Length; j++)
				{
					tiles[i].Add(new Tile()
					{
						Occupied = false,
						Position = new Vector2Int(j, i),
						IsSeat = lines[i][j] == 'L',
					});
				}
			}

			List<List<Tile>> updatedTiles = tiles;
			bool first = true;
			int interations = 0;

			while (first || !Stabilized(tiles, updatedTiles))
			{
				first = false;
				tiles = updatedTiles;
				updatedTiles = UpdateTiles();

				if (printOut1)
				{
					for (int i = 0; i < updatedTiles.Count; i++)
					{
						for (int j = 0; j < updatedTiles[0].Count; j++)
						{
							if (!updatedTiles[i][j].IsSeat)
							{
								Console.Write('.');
							}
							else if (updatedTiles[i][j].Occupied)
							{
								Console.Write('#');
							}
							else
							{
								Console.Write('L');
							}
						}

						Console.WriteLine();
					}

					Console.WriteLine();
					Console.WriteLine();
				}
			}

			return tiles.SelectMany(tl => tl)
				.Count(tiles => tiles.Occupied)
				.ToString();

			/*
			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					if (lines[i][j] == 'L')
					{
						tiles.Add(new Tile()
						{
							Position = new Vector2Int(j, i),
							Occupied = true
						});
					}
				}
			}

			List <Tile> updatedTiles = tiles;
			bool first = true;

			while (first || !Stabilized(tiles, updatedTiles))
			{
				first = false;
				tiles = updatedTiles;
				updatedTiles = UpdateTiles();
			}

			return tiles.Count(tiles => tiles.Occupied).ToString();
			*/
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");
			height = lines.Length;
			width = lines[0].Length;

			tiles = new List<List<Tile>>();
			for (int i = 0; i < lines.Length; i++)
			{
				tiles.Add(new List<Tile>());
				for (int j = 0; j < lines[0].Length; j++)
				{
					tiles[i].Add(new Tile()
					{
						Occupied = false,
						Position = new Vector2Int(j, i),
						IsSeat = lines[i][j] == 'L',
					});
				}
			}

			List<List<Tile>> updatedTiles = tiles;
			bool first = true;
			int interations = 0;

			while (first || !Stabilized(tiles, updatedTiles))
			{
				first = false;
				tiles = updatedTiles;
				updatedTiles = UpdateTilesPart2();

				if (printOut2)
				{
					for (int i = 0; i < updatedTiles.Count; i++)
					{
						for (int j = 0; j < updatedTiles[0].Count; j++)
						{
							if (!updatedTiles[i][j].IsSeat)
							{
								Console.Write('.');
							}
							else if (updatedTiles[i][j].Occupied)
							{
								Console.Write('#');
							}
							else
							{
								Console.Write('L');
							}
						}

						Console.WriteLine();
					}

					Console.WriteLine();
					Console.WriteLine();
				}
			}

			return tiles.SelectMany(tl => tl)
				.Count(tiles => tiles.Occupied)
				.ToString();
		}

		private List<List<Tile>> UpdateTilesPart2()
		{
			List<List<Tile>> updatedTiles = new List<List<Tile>>(height);
			for (int i = 0; i < tiles.Count; i++)
			{
				updatedTiles.Add(new List<Tile>(width));
				for (int j = 0; j < tiles[0].Count; j++)
				{
					if (tiles[i][j].IsSeat)
					{
						int amountOccupied = GetNeighboursPart2(tiles[i][j].Position);

						if (!tiles[i][j].Occupied && amountOccupied == 0)
						{
							updatedTiles[i].Add(new Tile()
							{
								Position = tiles[i][j].Position,
								Occupied = true,
								IsSeat = true
							});
						}
						else if (tiles[i][j].Occupied && amountOccupied >= 5)
						{
							updatedTiles[i].Add(new Tile()
							{
								Position = tiles[i][j].Position,
								Occupied = false,
								IsSeat = true
							});
						}
						else
						{
							updatedTiles[i].Add(tiles[i][j]);
						}

					}
					else
					{
						updatedTiles[i].Add(tiles[i][j]);
					}
				}
			}

			return updatedTiles;
		}

		private int GetNeighboursPart2(Vector2Int pos)
		{
			RectangleInt bounds = new RectangleInt(Vector2Int.Zero, new Vector2Int(width - 1, height - 1));
			int amount = 0;

			foreach (Vector2Int offset in offsets)
			{
				Vector2Int accumulator = new Vector2Int(pos.X, pos.Y) + offset;

				while (bounds.IsInRectangle(accumulator))
				{
					if (tiles[accumulator.Y][accumulator.X].IsSeat)
					{
						if (tiles[accumulator.Y][accumulator.X].Occupied)
						{
							amount++;
						}

						break;
					}

					accumulator += offset;
				}
			}

			return amount;
		}

		private List<List<Tile>> UpdateTiles()
		{
			List<List<Tile>> updatedTiles = new List<List<Tile>>(height);
			for (int i = 0; i < tiles.Count; i++)
			{
				updatedTiles.Add(new List<Tile>(width));
				for (int j = 0; j < tiles[0].Count; j++)
				{
					if (tiles[i][j].IsSeat)
					{
						int amountOccupied = GetNeighbours(tiles[i][j].Position).Count(t => t.IsSeat && t.Occupied);

						if (!tiles[i][j].Occupied && amountOccupied == 0)
						{
							updatedTiles[i].Add(new Tile()
							{
								Position = tiles[i][j].Position,
								Occupied = true,
								IsSeat = true
							});
						}
						else if (tiles[i][j].Occupied && amountOccupied >= 4)
						{
							updatedTiles[i].Add(new Tile()
							{
								Position = tiles[i][j].Position,
								Occupied = false,
								IsSeat = true
							});
						}
						else
						{
							updatedTiles[i].Add(tiles[i][j]);
						}

					}
					else
					{
						updatedTiles[i].Add(tiles[i][j]);
					}
				}
			}

			/*
			foreach (Tile tile in tiles.SelectMany(tl => tl))
			{
				int amountOccupied = GetNeighbours(tile.Position).Count(t => t.Occupied);

				if (!tile.Occupied && amountOccupied == 0)
				{
					updatedTiles.Add(new Tile()
					{
						Position = tile.Position,
						Occupied = true
					});
				}
				else if (tile.Occupied && amountOccupied >= 4)
				{
					updatedTiles.Add(new Tile()
					{
						Position = tile.Position,
						Occupied = false
					});
				}
				else
				{
					updatedTiles.Add(tile);
				}
			}
			*/

			return updatedTiles;
		}

		/*
		private List<Tile> UpdateTiles()
		{
			List<Tile> updatedTiles = new List<Tile>();
			foreach (Tile tile in tiles.SelectMany(tl => tl))
			{
				int amountOccupied = GetNeighbours(tile.Position).Count(t => t.Occupied);

				if (!tile.Occupied && amountOccupied == 0)
				{
					updatedTiles.Add(new Tile()
					{
						Position = tile.Position,
						Occupied = true
					});
				}
				else if (tile.Occupied && amountOccupied >= 4)
				{
					updatedTiles.Add(new Tile()
					{
						Position = tile.Position,
						Occupied = false
					});
				}
				else
				{
					updatedTiles.Add(tile);
				}
			}

			return updatedTiles;
		}
		*/

		

		public class Tile
		{
			public bool IsSeat { get; set; }
			public bool Occupied { get; set; }
			public Vector2Int Position { get; set; }
		}

		private List<Vector2Int> offsets = new List<Vector2Int>()
		{
			new Vector2Int(-1, -1),
			new Vector2Int(-1, 0),
			new Vector2Int(-1, 1),
			new Vector2Int(0, -1),
			new Vector2Int(0, 1),
			new Vector2Int(1, -1),
			new Vector2Int(1, 0),
			new Vector2Int(1, 1),
		};

		public List<Tile> GetNeighbours(Vector2Int selectedTile)
		{
			List<Vector2Int> surroundingTiles = new List<Vector2Int>();

			foreach (Vector2Int offset in offsets)
			{
				surroundingTiles.Add(selectedTile + offset);
			}

			return surroundingTiles.Where(pos => pos.X >= 0 && pos.Y >= 0 && pos.X < width && pos.Y < height)
				.Select(pos => tiles[pos.Y][pos.X])
				.ToList();
			//return tiles.SelectMany.Where(t => surroundingTiles.Contains(t.Position)).ToList();
		}

		public bool Stabilized(List<Tile> original, List<Tile> updated)
		{
			foreach (Tile tile in original)
			{
				if (updated.FirstOrDefault(updated => updated.Position == tile.Position)?.Occupied != tile.Occupied)
				{
					return false;
				}
			}

			return true;
		}

		public bool Stabilized(List<List<Tile>> original, List<List<Tile>> updated)
		{
			for (int i = 0; i < original.Count; i++)
			{
				for (int j = 0; j < original[0].Count; j++)
				{
					if (original[i][j] != updated[i][j])
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}