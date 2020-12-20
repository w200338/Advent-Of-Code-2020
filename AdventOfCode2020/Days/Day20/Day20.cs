using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020.Days.Day20
{
	public class Day20 : Day
	{
		private List<Tile> tiles;

		/// <inheritdoc />
		public override string Part1()
		{
			List<string> tileStrings = Input.Split("\r\n\r\n").ToList();

			// create tiles
			tiles = ParseTiles(tileStrings);

			List<string> edges = tiles.SelectMany(t => new[] {t.Top, t.Bottom, t.Left, t.Right})
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
				count += uniqueEdges.Contains(tile.Top) || uniqueEdges.Contains(new string(tile.Top.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Bottom) || uniqueEdges.Contains(new string(tile.Bottom.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Left) || uniqueEdges.Contains(new string(tile.Left.Reverse().ToArray())) ? 1 : 0;
				count += uniqueEdges.Contains(tile.Right) || uniqueEdges.Contains(new string(tile.Right.Reverse().ToArray())) ? 1 : 0;

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
			Part2Solver part2Solver = new Part2Solver(tiles);
			part2Solver.CalculateRoughness();

			return "";
		}

		private static List<Tile> ParseTiles(List<string> tileStrings)
		{
			List<Tile> tiles = new List<Tile>(tileStrings.Count);
			foreach (string tileString in tileStrings)
			{
				string[] lines = tileString.Split("\r\n");

				int tileId = int.Parse(lines[0].Substring(5, 4));

				tiles.Add(new Tile
				{
					Id = tileId,
					Data = lines.Skip(1).ToArray()
				});
			}

			return tiles;
		}
	}
}