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

			// check if all corners exist
			/*
			HashSet<long> answers = new HashSet<long>();
			foreach (Tile tile in tiles)
			{
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 4; k++)
						{
							// attempt to match sums and sides
							foreach (Tile insideTile in tiles)
							{
								insideTile.Top = tiles.FirstOrDefault(t => t.BottomSum == insideTile.TopSum);
								insideTile.Bottom = tiles.FirstOrDefault(t => t.TopSum == insideTile.BottomSum);

								insideTile.Left = tiles.FirstOrDefault(t => t.RightSum == insideTile.LeftSum);
								insideTile.Right = tiles.FirstOrDefault(t => t.LeftSum == insideTile.RightSum);
							}

							if (CornersExist(tiles) && MiddlePresent(tiles))
							{
								long total = 1;
								total *= tiles.First(t => t.Top == null && t.Left == null && t.Right != null && t.Bottom != null).Id;
								total *= tiles.First(t => t.Bottom == null && t.Left == null && t.Top != null && t.Right != null).Id;
								total *= tiles.First(t => t.Bottom == null && t.Right == null && t.Left != null && t.Top != null).Id;
								total *= tiles.First(t => t.Top == null && t.Right == null && t.Left != null && t.Bottom != null).Id;
								answers.Add(total);
							}

							//tiles.ForEach(t => t.TurnRight());
							tile.TurnRight();
						}

						tile.FlipHorizontal();
					}

					tile.FlipVertical();
				}
			}
			*/


			// find corners
			/*
			long total = 1;
			total *= tiles.First(t => t.Top == null && t.Left == null && t.Right != null && t.Bottom != null).Id;
			total *= tiles.First(t => t.Bottom == null && t.Left == null && t.Top != null && t.Right != null).Id;
			total *= tiles.First(t => t.Bottom == null && t.Right == null && t.Left != null && t.Top != null).Id;
			total *= tiles.First(t => t.Top == null && t.Right == null && t.Left != null && t.Bottom != null).Id;
			*/
			/*
			HashSet<long> answers = new HashSet<long>();
			foreach (object _ in Next(tiles))
			{
				if (CornersExist(tiles) && MiddlePresent(tiles))
				{
					long total = 1;
					total *= tiles.First(t => t.Top == null && t.Left == null && t.Right != null && t.Bottom != null).Id;
					total *= tiles.First(t => t.Bottom == null && t.Left == null && t.Top != null && t.Right != null).Id;
					total *= tiles.First(t => t.Bottom == null && t.Right == null && t.Left != null && t.Top != null).Id;
					total *= tiles.First(t => t.Top == null && t.Right == null && t.Left != null && t.Bottom != null).Id;
					answers.Add(total);
					Console.WriteLine(total);
				}
			}
			*/

			/*
			List<int> possibleCorners = new List<int>();
			//bool found = false;
			possibleCorners.AddRange(CalcTopLeftCorner(tiles));
			possibleCorners.AddRange(CalcTopRightCorner(tiles));
			possibleCorners.AddRange(CalcBottomLeftCorner(tiles));
			possibleCorners.AddRange(CalcBottomRightCorner(tiles));
			HashSet<int> uniqueCorners = possibleCorners.ToHashSet();

			long total = 1;
			foreach (long answer in uniqueCorners)
			{
				total *= answer;
				Console.WriteLine(answer);
			}
			*/

			// find edges with 1 occurrence
			/*
			List<int> edges = tiles.SelectMany(t => new[] {t.TopSum, t.BottomSum, t.LeftSum, t.RightSum}) // split tile into edges
				//.SelectMany(edge => new [] {edge, ~edge & 0b1111_1111_11 }) // add reversed version of edge
				.Select(edge => Math.Min(edge, ~edge & 0b1111_1111_11 )) // normalize
				.ToList();

			edges.Sort();

			List<int> uniqueEdges = edges.Where(edge => edges.Count(e => e == edge) == 1).ToList();

			List<Tile> cornerTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(Math.Min(tile.TopSum, ~tile.TopSum & 0b1111_1111_11)) ? 1 : 0;
				count += uniqueEdges.Contains(Math.Min(tile.BottomSum, ~tile.BottomSum & 0b1111_1111_11)) ? 1 : 0;
				count += uniqueEdges.Contains(Math.Min(tile.LeftSum, ~tile.LeftSum & 0b1111_1111_11)) ? 1 : 0;
				count += uniqueEdges.Contains(Math.Min(tile.RightSum, ~tile.RightSum & 0b1111_1111_11)) ? 1 : 0;

				return count == 2;
			}).Distinct()
				.ToList();
			*/

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
			int width = (int)Math.Sqrt(tileStrings.Count);


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
			HashSet<int> topLeftCornerIds = CalcTopLeftCorner(tiles);
			HashSet<int> topRightCornerIds = CalcTopRightCorner(tiles);
			Tile topLeftCorner = cornerTiles.First(tile => topLeftCornerIds.Contains(tile.Id));
			Tile topRightCorner = cornerTiles.First(tile => topRightCornerIds.Contains(3079)); //TODO remove hardcoded
			

			/*
			List<List<Tile>> tileGrid = new List<List<Tile>>(width);
			tileGrid.Add(new List<Tile>(width));
			tileGrid.Add(new List<Tile>(width));
			tileGrid[0].Add(topLeftCorner);
			*/
			/*
			// rotate topleft corner to its unique edges are in top and left
			bool found = false;
			int count = 0;
			foreach (object _ in Permutate(topLeftCorner))
			{
				if ((uniqueEdges.Contains(topLeftCorner.TopString) || uniqueEdges.Contains(new string(topLeftCorner.TopString.Reverse().ToArray()))) && 
					(uniqueEdges.Contains(topLeftCorner.LeftString) || uniqueEdges.Contains(new string(topLeftCorner.LeftString.Reverse().ToArray()))))
				{
					foreach (Tile rightTile in tiles.Except(new []{topLeftCorner}))
					{
						foreach (object __ in Permutate(rightTile))
						{
							if (topLeftCorner.RightString == rightTile.LeftString &&
							    (uniqueEdges.Contains(rightTile.TopString) || uniqueEdges.Contains(new string(rightTile.TopString.Reverse().ToArray()))))
							{
								foreach (Tile bottomTile in tiles.Except(new[] { topLeftCorner, rightTile }))
								{
									foreach (object ___ in Permutate(bottomTile))
									{
										if (uniqueEdges.Contains(bottomTile.LeftString) &&
										    (uniqueEdges.Contains(bottomTile.LeftString) || uniqueEdges.Contains(new string(bottomTile.LeftString.Reverse().ToArray()))))
										{
											if (topLeftCorner.BottomString == bottomTile.TopString)
											{
												if (count != 2)
												{
													count++;
												}
												else
												{
													tileGrid[0].Add(rightTile);
													tileGrid[1].Add(bottomTile);

													found = true;
												}

												break;
											}

											if (found) break;
										}
									}

									if (found) break;
								}
							}

							if (found) break;
						}

						if (found) break;
					}
				}

				if (found) break;
			}
			*/

			/*
			// next to topleft
			foreach (object _ in Permutate(topLeftCorner))
			{
				List<Tile> possibleTiles = tiles.Except(new[] {topLeftCorner}).ToList();
				
				try
				{
					tileGrid[0].Add(MatchRight(tileGrid[0][0], possibleTiles));
					break;
				}
				catch (ArgumentException) { }
			}
			*/

			/*
			// top row
			for (int i = 1; i < width; i++)
			{
				tileGrid[0].Add(MatchRight(tileGrid[0][i - 1], tiles.Except(tileGrid.SelectMany(tg => tg)).ToList()));
			}

			// second row
			for (int i = 1; i < width; i++)
			{
				tileGrid[1].Add(MatchRight(tileGrid[0][i], tiles.Except(tileGrid.SelectMany(tg => tg)).ToList()));
			}

			// rest of the grid
			for (int i = 1; i < width; i++)
			{
				tileGrid.Add(new List<Tile>(width));

				for (int j = 0; j < width; j++)
				{
					tileGrid[i].Add(MatchBottom(tileGrid[i - 1][j], tiles.Except(tileGrid.SelectMany(tg => tg)).ToList()));
				}
			}
			*/

			/*
			 Tile[,] tileGrid = new Tile[width, width];
			foreach (object _ in Permutate(topLeftCorner))
			{
				tileGrid = new Tile[width, width];
				List<Tile> availableTiles = tiles.Where(t => true).ToList();
				availableTiles.Remove(topLeftCorner);

				try
				{
					for (int i = 0; i < width; i++)
					{
						for (int j = 0; j < width; j++)
						{
							if (i == 0 && j == 0)
							{
								tileGrid[0, 0] = topLeftCorner;
							}
							else
							{
								Tile tile = null;
								if (i == 0)
								{
									tile = MatchRight(tileGrid[i, j - 1], availableTiles);
								}
								else if (j == 0)
								{
									tile = MatchBottom(tileGrid[i - 1, j], availableTiles);
								}
								else
								{
									tile = MatchTopLeft(tileGrid[i, j - 1].RightString, tileGrid[i - 1, j].BottomString, availableTiles);
								}

								availableTiles.Remove(tile);
								tileGrid[i, j] = tile;
							}
						}
					}

					break;
				}
				catch(ArgumentException) { }
			}
			*/

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

		public Tile MatchRight(Tile selectedTile, List<Tile> possibleTiles)
		{
			foreach (Tile possibleTile in possibleTiles)
			{
				foreach (object _ in Permutate(possibleTile))
				{
					if (selectedTile.RightString == possibleTile.LeftString)
					{
						return possibleTile;
					}
				}
			}

			throw new ArgumentException("No possibility found in tiles");
		}

		public Tile MatchBottom(Tile selectedTile, List<Tile> possibleTiles)
		{
			foreach (Tile possibleTile in possibleTiles)
			{
				foreach (object _ in Permutate(possibleTile))
				{
					if (selectedTile.BottomString == possibleTile.TopString)
					{
						return possibleTile;
					}
				}
			}

			throw new ArgumentException("No possibility found in tiles");
		}

		public Tile MatchTopLeft(string left, string top, List<Tile> possibleTiles)
		{
			foreach (Tile tile in possibleTiles)
			{
				foreach (object _ in Permutate(tile))
				{
					if (tile.LeftString == left && tile.TopString == top)
					{
						return tile;
					}
				}
			}

			throw new ArgumentException("No solution found");
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

		private HashSet<int> CalcBottomLeftCorner(List<Tile> tiles)
		{
			HashSet<int> answers = new HashSet<int>();
			bool found = false;
			foreach (Tile tile in tiles)
			{
				foreach (object _ in Permutate(tile))
				{
					foreach (Tile adjacentTile in tiles.Except(new[] { tile }))
					{
						if (tile.RightSum == adjacentTile.LeftSum)
						{
							foreach (object __ in Permutate(adjacentTile))
							{
								foreach (Tile otherAdjacentTile in tiles.Except(new[] { tile, adjacentTile }))
								{
									foreach (object ___ in Permutate(otherAdjacentTile))
									{
										if (tile.TopSum == otherAdjacentTile.BottomSum)
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

		private HashSet<int> CalcBottomRightCorner(List<Tile> tiles)
		{
			HashSet<int> answers = new HashSet<int>();
			bool found = false;
			foreach (Tile tile in tiles)
			{
				foreach (object _ in Permutate(tile))
				{
					foreach (Tile adjacentTile in tiles.Except(new[] { tile }))
					{
						if (tile.LeftSum == adjacentTile.RightSum)
						{
							foreach (object __ in Permutate(adjacentTile))
							{
								foreach (Tile otherAdjacentTile in tiles.Except(new[] { tile, adjacentTile }))
								{
									foreach (object ___ in Permutate(otherAdjacentTile))
									{
										if (tile.TopSum == otherAdjacentTile.BottomSum)
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


		public IEnumerable Next(List<Tile> tiles)
		{
			foreach (object _ in Permutate(tiles[0]))
			{
				if (tiles.Count > 1)
				{
					foreach (object __ in Next(tiles.Skip(1).ToList()))
					{
						yield return null;
					}
				}

				yield return null;
			}
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

		public bool CornersExist(List<Tile> tiles)
		{
			return tiles.FirstOrDefault(t => t.Top == null && t.Left == null && t.Right != null && t.Bottom != null) != null &&
			       tiles.FirstOrDefault(t => t.Bottom == null && t.Left == null && t.Top != null && t.Right != null) != null &&
			       tiles.FirstOrDefault(t => t.Bottom == null && t.Right == null && t.Left != null && t.Top != null) != null &&
			       tiles.FirstOrDefault(t => t.Top == null && t.Right == null && t.Left != null && t.Bottom != null) != null;
		}

		/// <summary>
		/// Correct amount of middle tiles
		/// </summary>
		/// <param name="tiles"></param>
		/// <returns></returns>
		public bool MiddlePresent(List<Tile> tiles)
		{
			int amount = (int)(Math.Sqrt(tiles.Count)) - 2;

			return tiles.Count(t => t.Top != null && t.Bottom != null && t.Left != null && t.Right != null) == amount;
		}

		public class Tile
		{
			public int Id { get; set; }
			public string TileData { get; set; }

			public int TopSum { get; set; }
			public string TopString { get; set; }
			public Tile Top { get; set; }

			public int BottomSum { get; set; }
			public string BottomString { get; set; }
			public Tile Bottom { get; set; }

			public int LeftSum { get; set; }
			public string LeftString { get; set; }
			public Tile Left { get; set; }

			public int RightSum { get; set; }
			public string RightString { get; set; }
			public Tile Right { get; set; }

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