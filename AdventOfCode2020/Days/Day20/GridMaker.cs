using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day20
{
	public class GridMaker
	{
		private readonly List<Day20.Tile> tiles;
		private readonly Day20.Tile topLeftTile;
		private readonly Day20.Tile topRightTile;
		private readonly List<string> uniqueEdges;
		private List<Day20.Tile> cornerTiles;
		private readonly int width;

		public GridMaker(List<Day20.Tile> tiles, Day20.Tile topLeftTile, Day20.Tile topRightTile)
		{
			this.tiles = tiles;
			this.topLeftTile = topLeftTile;
			this.topRightTile = topRightTile;
			width = (int) Math.Sqrt(tiles.Count);

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

			uniqueEdges = edges.Where(edge => edges.Count(e => e == edge) == 1).ToList();

			cornerTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.TopString) || uniqueEdges.Contains(new string(tile.TopString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.BottomString) || uniqueEdges.Contains(new string(tile.BottomString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.LeftString) || uniqueEdges.Contains(new string(tile.LeftString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.RightString) || uniqueEdges.Contains(new string(tile.RightString.Reverse().ToArray())) ? 1 : 0;

				return count == 2;
			}).ToList();
		}

		public List<List<Day20.Tile>> MakeGrid()
		{
			List<List<Day20.Tile>> output = new List<List<Day20.Tile>>();

			// make top row
			output.Add(new List<Day20.Tile>());
			output[0].Add(topLeftTile);
			for (int i = 1; i < width - 1; i++)
			{
				output[0].Add(null);
			}

			output[0].Add(topRightTile);

			// try to figure out middle tiles
			List<Tile> possibleTopRowTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.TopString) || uniqueEdges.Contains(new string(tile.TopString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.BottomString) || uniqueEdges.Contains(new string(tile.BottomString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.LeftString) || uniqueEdges.Contains(new string(tile.LeftString.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.RightString) || uniqueEdges.Contains(new string(tile.RightString.Reverse().ToArray())) ? 1 : 0;

				return count == 1;
			})
				.Select(t => new Tile(t))
				.ToList();

			// every orientation of top two tiles
			foreach (object _ in Permutate(topLeftTile))
			{
				// make sure unique edges are on the outside
				if (!IsUniqueEdge(topLeftTile.TopString) || !IsUniqueEdge(topLeftTile.LeftString))
				{
					continue;
				}

				foreach (object __ in Permutate(topRightTile))
				{
					if (!IsUniqueEdge(topRightTile.TopString) || !IsUniqueEdge(topRightTile.RightString))
					{
						continue;
					}

					// create bridge in between two corners
					for (int i = 1; i < width - 1; i++)
					{
						foreach (Tile tile in possibleTopRowTiles)
						{
							foreach (object ___ in Permutate(tile))
							{
								// check if unique edge is top edge
								if (IsUniqueEdge(tile.Top))
								{
									if (tile.Left == topLeftTile.RightString && tile.Right == topRightTile.LeftString)
									{
										Console.WriteLine("Found one!");
									}
								}
							}
						}
					}
				}
			}

			return output;
		}

		private bool IsUniqueEdge(string edge)
		{
			return uniqueEdges.Contains(edge) || uniqueEdges.Contains(new string(edge.Reverse().ToArray()));
		}

		public IEnumerable Permutate(Day20.Tile tile)
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

		private IEnumerable Permutate(Tile tile)
		{
			tile.Reset();
			yield return null;

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						tile.RotateRight();
						yield return null;
					}

					tile.FlipHorizontalAxis();
					yield return null;
				}

				tile.FlipVerticalAxis();
				yield return null;
			}
		}

		private class Tile
		{
			public int Id { get; }

			public string OriginalTop { get; }
			public string OriginalBottom { get; }
			public string OriginalLeft { get; }
			public string OriginalRight { get; }


			public string Top { get; set; }
			public string Bottom { get; set; }
			public string Left { get; set; }
			public string Right { get; set; }

			public Tile()
			{
				
			}

			public Tile(Day20.Tile tile)
			{
				Id = tile.Id;

				OriginalTop = tile.TopString;
				Top = tile.TopString;
				
				OriginalBottom = tile.BottomString;
				Bottom = tile.BottomString;

				OriginalLeft = tile.LeftString;
				Left = tile.LeftString;

				OriginalRight = tile.RightString;
				Right = tile.RightString;
			}

			public void Reset()
			{
				Top = OriginalTop;
				Bottom = OriginalBottom;
				Left = OriginalLeft;
				Right = OriginalRight;
			}

			public void RotateRight()
			{
				string temp = Top;

				Top = Left;
				Left = Bottom;
				Bottom = Right;
				Right = temp;
			}

			public void FlipHorizontalAxis()
			{
				string temp = Top;
				Top = Bottom;
				Bottom = temp;

				Right = new string(Right.Reverse().ToArray());
				Left = new string(Left.Reverse().ToArray());
			}

			public void FlipVerticalAxis()
			{
				string temp = Right;
				Right = Left;
				Left = temp;

				Top = new string(Top.Reverse().ToArray());
				Bottom = new string(Bottom.Reverse().ToArray());
			}
		}
	}
}