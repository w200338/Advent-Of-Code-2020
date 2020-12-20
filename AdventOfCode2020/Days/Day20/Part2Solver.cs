using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day20
{
	public class Part2Solver
	{
		private readonly List<Tile> tiles;
		private readonly List<string> uniqueEdges;
		private readonly List<Tile> cornerTiles;
		private readonly int width;

		public Part2Solver(List<Tile> tiles)
		{
			this.tiles = tiles;
			width = (int) Math.Sqrt(tiles.Count);

			// find corners
			List<string> edges = tiles.SelectMany(t => new[] { t.Top, t.Bottom, t.Left, t.Right })
				.Select(edge =>
				{
					string reversedEdge = new string(edge.Reverse().ToArray());
					if (edge.GetHashCode() < reversedEdge.GetHashCode())
					{
						return edge;
					}

					return reversedEdge;
				})
				.ToList();

			uniqueEdges = edges.Where(edge => edges.Count(e => e == edge) == 1).ToList();

			cornerTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.Top) || uniqueEdges.Contains(new string(tile.Top.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Bottom) || uniqueEdges.Contains(new string(tile.Bottom.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Left) || uniqueEdges.Contains(new string(tile.Left.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Right) || uniqueEdges.Contains(new string(tile.Right.Reverse().ToArray())) ? 1 : 0;

				return count == 2;
			}).ToList();
		}

		public void CalculateRoughness()
		{
			Console.WriteLine("\n\nLooking for sea monster:");
			Console.WriteLine("                  # \n#    ##    ##    ###\n #  #  #  #  #  #   \n");
			string[] monsterLines = "                  # \n#    ##    ##    ###\n #  #  #  #  #  #   ".Split('\n');

			//Regex seasMonsterRegex = new Regex(@"[#\.]{18}#[#\.][^\n]*\n[^\n]*#([#\.]{4}##){3}#[^\n]*\n[^\n]*[#\.](#[#\.]{2}){5}#[#\.]{3}", RegexOptions.Multiline);
			foreach (Tile cornerTile in cornerTiles)
			{
				foreach (object _ in Permute(cornerTile))
				{
					// create accurate grid of tiles
					List<List<Tile>> currentTiles = new List<List<Tile>>(width);
					for (int i = 0; i < width; i++)
					{
						currentTiles.Add(new List<Tile>());
						for (int j = 0; j < width; j++)
						{
							currentTiles[i].Add(null);
						}
					}

					currentTiles[0][0] = cornerTile;

					if (!Solve(currentTiles, Vector2Int.Zero))
					{
						continue;
					}

					// combine all tiles into 1 big string
					StringBuilder stringBuilder = new StringBuilder(width * 8 * width * 8 + width);
					int dataLength = currentTiles[0][0].Data.Length;
					for (int y = 0; y < currentTiles.Count; y++)
					{
						for (int row = 1; row < dataLength - 1; row++)
						{
							for (int x = 0; x < currentTiles.Count; x++)
							{
								stringBuilder.Append(currentTiles[y][x].Row(row).Substring(1, dataLength - 2));
							}

							stringBuilder.Append('\n');
						}
					}

					string sea = stringBuilder.ToString();

					//int monsters = seasMonsterRegex.Matches(sea).Count;

					string[] seaLines = sea.Split('\n');
					int monsters = 0;
					for (int seaY = 0; seaY < seaLines.Length - monsterLines.Length; seaY++)
					{
						for (int seaX = 0; seaX < seaLines.Length - monsterLines[0].Length; seaX++)
						{
							if (IsMonsterPresent(monsterLines, seaLines, seaY, seaX))
							{
								monsters++;
							}
						}
					}

					if (monsters > 0)
					{
						Console.WriteLine($"\nFound {monsters} sea monsters");

						int roughness = sea.Count(c => c == '#') - monsters * 15;
						Console.WriteLine($"Water roughness: {roughness}\n");

						return;
					}
				}
			}
		}

		private static bool IsMonsterPresent(string[] monsterLines, string[] seaLines, int seaY, int seaX)
		{
			for (int monsterY = 0; monsterY < monsterLines.Length; monsterY++)
			{
				for (int monsterX = 0; monsterX < monsterLines[0].Length; monsterX++)
				{
					if (monsterLines[monsterY][monsterX] == '#' && seaLines[seaY + monsterY][seaX + monsterX] != '#')
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool Solve(List<List<Tile>> currentTiles, Vector2Int pos)
		{
			// solved, first tile outside of range
			if (pos.X == currentTiles.Count && pos.Y == currentTiles.Count - 1)
			{
				return true;
			}

			// next column
			if (pos.X == currentTiles.Count)
			{
				pos.Y++;
				pos.X = 0;
			}

			// skip existing values
			if (currentTiles[pos.Y][pos.X] != null)
			{
				return Solve(currentTiles, pos + new Vector2Int(1, 0));
			}

			foreach (State state in PossibleTiles(currentTiles, pos, tiles.Except(currentTiles.SelectMany(t => t)).ToList()))
			{
				currentTiles[pos.Y][pos.X] = tiles.First(t => t.Id == state.TileId);
				currentTiles[pos.Y][pos.X].State = state;

				if (Solve(currentTiles, pos + new Vector2Int(1, 0)))
				{
					return true;
				}
			}

			currentTiles[pos.Y][pos.X] = null;
			return false;
		}

		private HashSet<State> PossibleTiles(List<List<Tile>> currentTiles, Vector2Int pos, List<Tile> availableTiles)
		{
			HashSet<State> possibleStates = new HashSet<State>();

			bool leftEdge = pos.X == 0;
			bool rightEdge = pos.X == currentTiles.Count - 1;
			bool topEdge = pos.Y == 0;
			bool bottomEdge = pos.Y == currentTiles.Count - 1;

			foreach (Tile availableTile in availableTiles)
			{
				foreach (object _ in Permute(availableTile))
				{
					// horizontal check
					if (leftEdge)
					{
						if (!IsUniqueEdge(availableTile.Left))
						{
							continue;
						}
					}
					else
					{
						if (rightEdge && !IsUniqueEdge(availableTile.Right))
						{
							continue;
						}

						if (currentTiles[pos.Y][pos.X - 1].Right != availableTile.Left)
						{
							continue;
						}
					}

					// vertical check
					if (topEdge)
					{
						if (!IsUniqueEdge(availableTile.Top))
						{
							continue;
						}
					}
					else
					{
						if (bottomEdge && !IsUniqueEdge(availableTile.Bottom))
						{
							continue;
						}

						if (currentTiles[pos.Y - 1][pos.X].Bottom != availableTile.Top)
						{
							continue;
						}
					}

					possibleStates.Add(availableTile.State);
				}
			}

			return possibleStates;
		}

		private bool IsUniqueEdge(string edge)
		{
			return uniqueEdges.Contains(edge) || uniqueEdges.Contains(new string(edge.Reverse().ToArray()));
		}

		public static IEnumerable Permute(Tile tile)
		{
			tile.Reset();
			
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					tile.RotateRight();
					yield return null;
				}

				tile.FlipHorizontalAxis();
				yield return null;
			}
		}
	}
}