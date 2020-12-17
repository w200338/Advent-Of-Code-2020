using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2020.Tools.Mathematics.Vectors;

namespace AdventOfCode2020.Days.Day17
{
	public class Day17 : Day
	{
		private int startOffset;

		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");
			startOffset = (int)Math.Floor(lines.Length / 2f);

			List<Cell> cells = new List<Cell>();

			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[0].Length; j++)
				{
					if (lines[i][j] == '#')
					{
						cells.Add(new Cell()
						{
							Position = new Vector3Int()
							{
								X = j - 1,
								Y = i - 1,
								Z = 0,
							},
							Active = lines[i][j] == '#'
						});
					}
				}
			}

			/*
			for (int i = 0; i < 6; i++)
			{
				Vector3Int offset = new Vector3Int(-i, -i, -i);

				Cell[,,] updatedCellGrid = new Cell[3 + i * 2, 3 + i * 2, i * 2 + 3];
				//updatedCellGrid.Initialize();

				for (int j = 0; j < updatedCellGrid.GetLength(0); j++)
				{
					for (int k = 0; k < updatedCellGrid.GetLength(1); k++)
					{
						for (int l = 0; l < updatedCellGrid.GetLength(2); l++)
						{
							Vector3Int position = new Vector3Int(j, k, l) + offset;

							int activeNeighbours = ActiveNeighbours(cells, position);

							bool active = cells.FirstOrDefault(c => c.Position == position)?.Active ?? false;
							updatedCellGrid[j, k, l] = new Cell()
							{
								Position = position,
								Active = active && (activeNeighbours == 2 || activeNeighbours == 3) ||
								         !active && activeNeighbours == 3
							};
						}
					}
				}

				/*
				// already known cells
				foreach (Cell cell in cells)
				{
					int activeNeighbours = ActiveNeighbours(cells, cell.Position);

					if (cell.Active && (activeNeighbours == 2 || activeNeighbours == 3) ||
					    !cell.Active && activeNeighbours == 3)
					{
						cells.Add(new Cell()
						{
							Active = true,
							Position = cell.Position
						});
					}
					else
					{
						cells.Add(new Cell()
						{
							Active = false,
							Position = cell.Position
						});
					}
				}
				*

				// shift cells
				cells = new List<Cell>(updatedCellGrid.GetLength(0) * updatedCellGrid.GetLength(1) * updatedCellGrid.GetLength(2));
//				Vector3Int offset = new Vector3Int(-i + 1, -i + 1, -i + 1);

				for (int j = 0; j < updatedCellGrid.GetLength(0); j++)
				{
					for (int k = 0; k < updatedCellGrid.GetLength(1); k++)
					{
						for (int l = 0; l < updatedCellGrid.GetLength(2); l++)
						{
							cells.Add(new Cell()
							{
								Position = updatedCellGrid[j, k, l].Position + offset,
								Active = updatedCellGrid[j,k,l].Active
							});
						}
					}
				}
			}
			*/

			for (int i = 0; i < 6; i++)
			{
				cells = Tick(cells, i);
			}

			return cells.Count(c => c.Active).ToString();
			// higher than 196
			// higher than 227
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");
			startOffset = (int)Math.Floor(lines.Length / 2f);

			List<TimeCell> cells = new List<TimeCell>();

			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[0].Length; j++)
				{
					if (lines[i][j] == '#')
					{
						cells.Add(new TimeCell()
						{
							Position = new VectorNInt(4),
							Active = lines[i][j] == '#'
						});

						cells.Last().Position[0] = j - 1;
						cells.Last().Position[1] = i - 1;
						cells.Last().Position[2] = 0;
						cells.Last().Position[3] = 0;
					}
				}
			}

			for (int i = 0; i < 6; i++)
			{
				cells = TickPart2(cells, i);
			}

			return cells.Count(c => c.Active).ToString();

			// less than 6246
		}

		public int ActiveNeighbours(List<Cell> cells, Vector3Int position)
		{
			//return cells.Count(c => c.Active && Vector3Int.Distance(c.Position, position) < 2 && Vector3Int.Distance(c.Position, position) > 0);
			return cells.Count(c => c.Active && c.Position != position && Math.Abs(c.Position.X - position.X) <= 1 && Math.Abs(c.Position.Y - position.Y) <= 1 && Math.Abs(c.Position.Z - position.Z) <= 1);

			//return cells.Count(c => c.Active && Vector3Int.DistanceManhattan(c.Position, position) == 1);
		}

		public int ActiveNeighboursPart2(List<TimeCell> cells, VectorNInt position)
		{
			//return cells.Count(c => c.Active && Vector3Int.Distance(c.Position, position) < 2 && Vector3Int.Distance(c.Position, position) > 0);
			return cells.Count(c => c.Active && c.Position != position && 
			                        Math.Abs(c.Position.Values[0] - position.Values[0]) <= 1 && 
			                        Math.Abs(c.Position.Values[1] - position.Values[1]) <= 1 && 
			                        Math.Abs(c.Position.Values[2] - position.Values[2]) <= 1 && 
			                        Math.Abs(c.Position.Values[3] - position.Values[3]) <= 1);
			

			//return cells.Count(c => c.Active && Vector3Int.DistanceManhattan(c.Position, position) == 1);
		}

		public List<Cell> Tick(List<Cell> activeCells, int tickCount)
		{
			List<Cell> updatedActiveCells = new List<Cell>();

			for (int k = -startOffset - tickCount - startOffset; k <= startOffset + tickCount + startOffset; k++)
			{
				for (int j = -startOffset - tickCount - startOffset; j <= startOffset + tickCount + startOffset; j++)
				{
					for (int i = -startOffset - tickCount - startOffset; i <= startOffset + tickCount + startOffset; i++)
					{
						Vector3Int pos = new Vector3Int(i, j, k);

						int activeNeighbours = ActiveNeighbours(activeCells, pos);

						if (activeCells.FirstOrDefault(c => c.Position == pos) != null)
						{
							if (activeNeighbours == 2 || activeNeighbours == 3)
							{
								updatedActiveCells.Add(new Cell()
								{
									Position = pos,
									Active = true
								});

								//Console.Write("#");
							}
							else
							{
								//Console.Write(".");
							}
						}
						else if (activeNeighbours == 3)
						{

							updatedActiveCells.Add(new Cell()
							{
								Position = pos,
								Active = true
							});

							//Console.Write("#");
						}
						else
						{
							//Console.Write(".");
						}
					}

					//Console.WriteLine();
				}

				//Console.WriteLine();
			}

			return updatedActiveCells;
		}

		public List<TimeCell> TickPart2(List<TimeCell> activeCells, int tickCount)
		{
			List<TimeCell> updatedActiveCells = new List<TimeCell>();

			for (int l = -startOffset - tickCount - startOffset; l < startOffset + tickCount + startOffset; l++)
			{
				for (int k = -startOffset - tickCount - startOffset; k <= startOffset + tickCount + startOffset; k++)
				{
					for (int j = -startOffset - tickCount - startOffset; j <= startOffset + tickCount + startOffset; j++)
					{
						for (int i = -startOffset - tickCount - startOffset; i <= startOffset + tickCount + startOffset; i++)
						{
							VectorNInt pos = new VectorNInt(4);
							pos.Values[0] = i;
							pos.Values[1] = j;
							pos.Values[2] = k;
							pos.Values[3] = l;

							//Vector3Int pos = new Vector3Int(i, j, k);

							int activeNeighbours = ActiveNeighboursPart2(activeCells, pos);

							if (activeCells.FirstOrDefault(c => c.Position == pos) != null)
							{
								if (activeNeighbours == 2 || activeNeighbours == 3)
								{
									updatedActiveCells.Add(new TimeCell()
									{
										Position = pos,
										Active = true
									});

									//Console.Write("#");
								}
								else
								{
									//Console.Write(".");
								}
							}
							else if (activeNeighbours == 3)
							{

								updatedActiveCells.Add(new TimeCell()
								{
									Position = pos,
									Active = true
								});

								//Console.Write("#");
							}
							else
							{
								//Console.Write(".");
							}
						}

						//Console.WriteLine();
					}

					//Console.WriteLine();
				}

			}

			return updatedActiveCells;
		}
	}

	public class Cell
	{
		public Vector3Int Position { get; set; }
		public bool Active { get; set; }
	}

	public class TimeCell
	{
		public VectorNInt Position { get; set; }
		public bool Active { get; set; }
	}
}