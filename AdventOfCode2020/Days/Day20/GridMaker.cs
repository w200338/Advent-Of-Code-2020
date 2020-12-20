﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day20
{
	public class GridMaker
	{
		private readonly List<Tile> tiles;
		private readonly Day20.Tile topLeftTile;
		private readonly Day20.Tile topRightTile;
		private readonly List<string> uniqueEdges;
		private List<Tile> cornerTiles;
		private readonly int width;

		public GridMaker(List<Day20.Tile> tiles, Day20.Tile topLeftTile, Day20.Tile topRightTile)
		{
			this.tiles = tiles.Select(t => new Tile(t)).ToList();
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
			})
				.Select(t => new Tile(t))
				.ToList();
		}

		public List<List<Day20.Tile>> MakeGrid()
		{
			List<List<Day20.Tile>> output = new List<List<Day20.Tile>>();

			// make top row
			output.Add(new List<Day20.Tile>());
			/*
			output[0].Add(topLeftTile);
			for (int i = 1; i < width - 1; i++)
			{
				output[0].Add(null);
			}

			output[0].Add(topRightTile);
			*/

			/*
			// try to figure out middle tiles of top row
			List<Tile> possibleTopRowTiles = tiles.Where(tile =>
			{
				int count = 0;
				count += uniqueEdges.Contains(tile.Top) || uniqueEdges.Contains(new string(tile.Top.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Bottom) || uniqueEdges.Contains(new string(tile.Bottom.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Left) || uniqueEdges.Contains(new string(tile.Left.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Right) || uniqueEdges.Contains(new string(tile.Right.Reverse().ToArray())) ? 1 : 0;

				return count == 1;
			}).ToList();
			*/
			//Tile topLeftTile = new Tile(this.topLeftTile);
			//Tile topRightTile = new Tile(this.topRightTile);

			/*
			HashSet<LineMatch> matches = new HashSet<LineMatch>();
			// every orientation of top two tiles
			foreach (object _ in Permutate(topLeftTile))
			{
				// make sure unique edges are on the outside
				if (!IsUniqueEdge(topLeftTile.Top) || !IsUniqueEdge(topLeftTile.Left))
				{
					continue;
				}

				foreach (object __ in Permutate(topRightTile))
				{
					if (!IsUniqueEdge(topRightTile.Top) || !IsUniqueEdge(topRightTile.Right))
					{
						continue;
					}

					// create bridge in between two corners
					List<Tile> currentRow = new List<Tile>();
					for (int i = 1; i < width - 1; i++)
					{
						foreach (Tile tile in possibleTopRowTiles)
						{
							foreach (object ___ in Permutate(tile))
							{
								// check if unique edge is top edge
								if (IsUniqueEdge(tile.Top))
								{
									if (tile.Left == topLeftTile.Right && tile.Right == topRightTile.Left)
									{
										Console.WriteLine("Found one!");

										matches.Add(new LineMatch()
										{
											Left = topLeftTile.State,
											Middle = tile.State,
											Right = topRightTile.State
										});
									}
								}
							}
						}
					}
				}
			}

			// init tiles
			foreach (LineMatch lineMatch in matches)
			{
				List<List<Tile>> currentTiles = new List<List<Tile>>(width);
				for (int i = 0; i < width; i++)
				{
					currentTiles.Add(new List<Tile>());
					for (int j = 0; j < width; j++)
					{
						currentTiles[i].Add(null);
					}
				}

				currentTiles[0][0] = topLeftTile;
				currentTiles[0][0].State = lineMatch.Left;

				//currentTiles[0][1] = tiles.First(t => t.Id == lineMatch.Middle.TileId);
				//currentTiles[0][1].State = lineMatch.Middle;

				currentTiles[0][2] = topRightTile;
				currentTiles[0][2].State = lineMatch.Right;

				if (Solve(currentTiles, Vector2Int.Zero))
				{
					Console.WriteLine("Solved");
				}
			}
			*/

			List<List<Tile>> currentTiles = new List<List<Tile>>(width);
			for (int i = 0; i < width; i++)
			{
				currentTiles.Add(new List<Tile>());
				for (int j = 0; j < width; j++)
				{
					currentTiles[i].Add(null);
				}
			}

			if (Solve(currentTiles, Vector2Int.Zero))
			{
				Console.WriteLine("Solved");
			}

			return output;
		}

		private bool Solve(List<List<Tile>> currentTiles, Vector2Int pos)
		{
			// solved, first tile outside of range
			if (pos.Y == currentTiles.Count && pos.X == currentTiles.Count - 1)
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

		public IEnumerable Permute(Day20.Tile tile)
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

		public static IEnumerable Permute(Tile tile)
		{
			tile.Reset();
			yield return null;

			for (int i = 0; i < 3; i++)
			{
				tile.RotateRight();
				yield return null;
			}
			tile.RotateRight();


			tile.FlipHorizontalAxis();
			yield return null;

			for (int i = 0; i < 3; i++)
			{
				tile.RotateRight();
				yield return null;
			}
			tile.RotateRight();
			tile.FlipHorizontalAxis();

			tile.FlipVerticalAxis();
			yield return null;

			for (int i = 0; i < 3; i++)
			{
				tile.RotateRight();
				yield return null;
			}
			tile.RotateRight();

			tile.FlipHorizontalAxis();
			yield return null;

			for (int i = 0; i < 3; i++)
			{
				tile.RotateRight();
				yield return null;
			}
			tile.RotateRight();
			tile.FlipHorizontalAxis();
			tile.FlipVerticalAxis();

			/*
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
			*/
		}

		public class Tile
		{
			public int Rotation { get; set; }
			public bool FlippedHorizontal { get; set; }
			public bool FlippedVertical { get; set; }

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

			public State State
			{
				get => new State()
				{
					TileId = Id,
					FlippedHorizontally = FlippedHorizontal,
					FlippedVertically = FlippedVertical,
					Rotation = Rotation
				};

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

					if (value.FlippedVertically)
					{
						FlipVerticalAxis();
					}
				}
			}
			public void Reset()
			{
				Top = OriginalTop;
				Bottom = OriginalBottom;
				Left = OriginalLeft;
				Right = OriginalRight;

				Rotation = 0;
				FlippedHorizontal = false;
				FlippedVertical = false;
			}

			public void RotateRight()
			{
				string temp = Top;

				Top = Left;
				Left = Bottom;
				Bottom = Right;
				Right = temp;

				Rotation = ++Rotation % 4;
			}

			public void FlipHorizontalAxis()
			{
				string temp = Top;
				Top = Bottom;
				Bottom = temp;

				Right = new string(Right.Reverse().ToArray());
				Left = new string(Left.Reverse().ToArray());

				FlippedHorizontal = !FlippedHorizontal;
			}

			public void FlipVerticalAxis()
			{
				string temp = Right;
				Right = Left;
				Left = temp;

				Top = new string(Top.Reverse().ToArray());
				Bottom = new string(Bottom.Reverse().ToArray());

				FlippedVertical = !FlippedVertical;
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

		public class State
		{
			public int TileId { get; set; }
			public int Rotation { get; set; }
			public bool FlippedHorizontally { get; set; }
			public bool FlippedVertically { get; set; }

			public State()
			{
				
			}

			public State(Tile currentState)
			{
				TileId = currentState.Id;
				Rotation = currentState.Rotation;
				FlippedHorizontally = currentState.FlippedHorizontal;
				FlippedVertically = currentState.FlippedVertical;
			}

			public State(int rotation, bool flippedHorizontally, bool flippedVertically)
			{
				Rotation = rotation;
				FlippedHorizontally = flippedHorizontally;
				FlippedVertically = flippedVertically;
			}

			protected bool Equals(State other)
			{
				return TileId == other.TileId && Rotation == other.Rotation && FlippedHorizontally == other.FlippedHorizontally && FlippedVertically == other.FlippedVertically;
			}

			/// <inheritdoc />
			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((State) obj);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return HashCode.Combine(TileId, Rotation, FlippedHorizontally, FlippedVertically);
			}
		}

		private class LineMatch
		{
			public State Left { get; set; }
			public State Middle { get; set; }
			public State Right { get; set; }

			protected bool Equals(LineMatch other)
			{
				return Equals(Left, other.Left) && Equals(Middle, other.Middle) && Equals(Right, other.Right);
			}

			/// <inheritdoc />
			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((LineMatch) obj);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return HashCode.Combine(Left, Middle, Right);
			}
		}
	}
}