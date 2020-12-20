using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020.Days.Day20
{
	public class Day20 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> tileStrings = Input.Split("\r\n\r\n").ToList();

			// create tiles
			List<Tile> tiles = ParseTiles(tileStrings);

			List<string> edges = tiles.SelectMany(t => new[] {t.TopString, t.BottomString, t.LeftString, t.RightString})
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

			List<string> uniqueEdges = edges.Where(edge => edges.Count(e => e == edge) == 1).ToList();

			List<Tile> cornerTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.TopString) || uniqueEdges.Contains(new string(tile.TopString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.BottomString) || uniqueEdges.Contains(new string(tile.BottomString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.LeftString) || uniqueEdges.Contains(new string(tile.LeftString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.RightString) || uniqueEdges.Contains(new string(tile.RightString.Reverse().ToArray())) ? 1 : 0;

				return count == 2;
			}).ToList();

			long total = 1;
			foreach (Tile uniqueCornerTile in cornerTiles)
			{
				total *= uniqueCornerTile.Id;
			}
			
			Console.WriteLine(total);

			return "done";
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<string> tileStrings = Input.Split("\r\n\r\n").ToList();
			//int width = (int)Math.Sqrt(tileStrings.Count);

			List<Tile> tiles = ParseTiles(tileStrings);

			// find corners
			List<string> edges = tiles.SelectMany(t => new[] { t.TopString, t.BottomString, t.LeftString, t.RightString })
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

			List<string> uniqueEdges = edges.Where(edge => edges.Count(e => e == edge) == 1).ToList();

			List<Tile> cornerTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.TopString) || uniqueEdges.Contains(new string(tile.TopString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.BottomString) || uniqueEdges.Contains(new string(tile.BottomString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.LeftString) || uniqueEdges.Contains(new string(tile.LeftString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.RightString) || uniqueEdges.Contains(new string(tile.RightString.Reverse().ToArray())) ? 1 : 0;

				return count == 2;
			}).ToList();

			// 'click' one corner into place
			//HashSet<int> topLeftCornerIds = CalcTopLeftCorner(tiles);
			//HashSet<int> topRightCornerIds = CalcTopRightCorner(tiles);
			Tile topLeftCorner = cornerTiles.First(tile => tile.Id == 1951);
			Tile topRightCorner = cornerTiles.First(tile => tile.Id == 3079); //TODO remove hardcoded

			GridMaker gridMaker = new GridMaker(tiles, topLeftCorner, topRightCorner);
			gridMaker.MakeGrid();

			return "not implemented";
		}

		private static List<Tile> ParseTiles(List<string> tileStrings)
		{
			List<Tile> tiles = new List<Tile>(tileStrings.Count);
			foreach (string tileString in tileStrings)
			{
				string[] lines = tileString.Split("\r\n");

				int tileId = int.Parse(lines[0].Substring(5, 4));

				int topSum = 0;
				int bottomSum = 0;
				int leftSum = 0;
				int rightSum = 0;

				string leftString = "";
				string rightString = "";

				for (int i = 0; i < 10; i++)
				{
					topSum |= lines[1][i] == '#' ? 1 : 0;
					topSum <<= 1;

					bottomSum |= lines[10][i] == '#' ? 1 : 0;
					bottomSum <<= 1;

					leftSum |= lines[i + 1][0] == '#' ? 1 : 0;
					leftString += lines[i + 1][0];
					leftSum <<= 1;

					rightSum |= lines[i + 1][9] == '#' ? 1 : 0;
					rightString += lines[i + 1][9];
					rightSum <<= 1;
				}

				topSum >>= 1;
				bottomSum >>= 1;
				leftSum >>= 1;
				rightSum >>= 1;


				tiles.Add(new Tile()
				{
					Id = tileId,
					TileData = tileString.Substring(10).Trim(),
					TopSum = topSum,
					BottomSum = bottomSum,
					LeftSum = leftSum,
					RightSum = rightSum,

					TopString = lines[1],
					BottomString = lines[10],
					LeftString = leftString,
					RightString = rightString
				});
			}

			return tiles;
		}

		private HashSet<int> CalcTopLeftCorner(List<Tile> tiles)
		{
			HashSet<int> answers = new HashSet<int>();
			bool found = false;
			foreach (Tile tile in tiles)
			{
				foreach (object _ in Permutate(tile))
				{
					foreach (Tile adjacentTile in tiles.Except(new[] {tile}))
					{
						if (tile.RightString == adjacentTile.LeftString)
						{
							foreach (object __ in Permutate(adjacentTile))
							{
								foreach (Tile otherAdjacentTile in tiles.Except(new[] {tile, adjacentTile}))
								{
									foreach (object ___ in Permutate(otherAdjacentTile))
									{
										if (tile.BottomString == otherAdjacentTile.TopString)
										{
											answers.Add(tile.Id);
											//found = true;
											//break;
										}
									}

									if (found) break;
								}

								if (found) break;
							}

							if (found) break;
						}
					}

					if (found) break;
				}

				if (found) break;
			}

			return answers;
		}

		private HashSet<int> CalcTopRightCorner(List<Tile> tiles)
		{
			HashSet<int> answers = new HashSet<int>();
			bool found = false;
			foreach (Tile tile in tiles)
			{
				foreach (object _ in Permutate(tile))
				{
					foreach (Tile adjacentTile in tiles.Except(new[] { tile }))
					{
						if (tile.LeftString == adjacentTile.RightString)
						{
							foreach (object __ in Permutate(adjacentTile))
							{
								foreach (Tile otherAdjacentTile in tiles.Except(new[] { tile, adjacentTile }))
								{
									foreach (object ___ in Permutate(otherAdjacentTile))
									{
										if (tile.BottomString == otherAdjacentTile.TopString)
										{
											answers.Add(tile.Id);
											//found = true;
											//break;
										}
									}

									if (found) break;
								}

								if (found) break;
							}

							if (found) break;
						}
					}

					if (found) break;
				}

				if (found) break;
			}

			return answers;
		}

		public IEnumerable Permutate(Tile tile)
		{
			yield return null;

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						tile.TurnRight();
						yield return null;
					}

					tile.FlipHorizontal();
					yield return null;
				}

				tile.FlipVertical();
				yield return null;
			}
		}

		public class Tile
		{
			public int Id { get; set; }
			public string TileData { get; set; }

			public int TopSum { get; set; }
			public string TopString { get; set; }

			public int BottomSum { get; set; }
			public string BottomString { get; set; }

			public int LeftSum { get; set; }
			public string LeftString { get; set; }
			public Tile Left { get; set; }

			public int RightSum { get; set; }
			public string RightString { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{nameof(Id)}: {Id}, {nameof(TopSum)}: {TopSum}, {nameof(BottomSum)}: {BottomSum}, {nameof(LeftSum)}: {LeftSum}, {nameof(RightSum)}: {RightSum}";
			}

			public void TurnRight()
			{
				int temp = TopSum;
				string tempStr = TopString;

				TopSum = LeftSum;
				TopString = LeftString;

				LeftSum = BottomSum;
				LeftString = BottomString;

				BottomSum = RightSum;
				BottomString = RightString;

				RightSum = temp;
				RightString = tempStr;
			}

			public void FlipHorizontal()
			{
				int temp = LeftSum;
				string tempStr = LeftString;

				LeftSum = RightSum;
				LeftString = RightString;

				RightSum = temp;
				RightString = tempStr;

				TopSum = ~TopSum & 0b1111_1111_11;
				TopString = new string(TopString.Reverse().ToArray());

				BottomSum = ~BottomSum & 0b1111_1111_11;
				BottomString = new string(BottomString.Reverse().ToArray());
			}

			public void FlipVertical()
			{
				int temp = TopSum;
				string tempStr = TopString;

				TopSum = BottomSum;
				TopString = BottomString;

				BottomSum = temp;
				BottomString = tempStr;

				LeftSum = ~LeftSum & 0b1111_1111_11;
				LeftString = new string(LeftString.Reverse().ToArray());

				RightSum = ~RightSum & 0b1111_1111_11;
				RightString = new string(RightString.Reverse().ToArray());
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
				return Equals((Tile) obj);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return Id;
			}
		}
	}
}