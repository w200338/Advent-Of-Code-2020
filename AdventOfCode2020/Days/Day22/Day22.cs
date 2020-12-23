using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day22
{
	public class Day22 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			List<string> playerLines = Input.Split("\r\n\r\n").ToList();

			Player player1 = new Player()
			{
				Id = 1,
				Cards = new Queue<int>(playerLines[0].Split("\r\n").Skip(1).Select(int.Parse))
			};

			Player player2 = new Player()
			{
				Id = 2,
				Cards = new Queue<int>(playerLines[1].Split("\r\n").Skip(1).Select(int.Parse))
			};

			// keep playing till one person runs out of cards
			while (player1.Cards.Count > 0 && player2.Cards.Count > 0)
			{
				int player1Card = player1.Cards.Dequeue();
				int player2Card = player2.Cards.Dequeue();

				if (player1Card > player2Card)
				{
					player1.Cards.Enqueue(player1Card);
					player1.Cards.Enqueue(player2Card);
				}
				else
				{
					player2.Cards.Enqueue(player2Card);
					player2.Cards.Enqueue(player1Card);
				}
			}

			long total = 0;
			if (player1.Cards.Count > 0)
			{
				List<int> cardValues = player1.Cards.Reverse().ToList();
				for (int i = 0; i < player1.Cards.Count; i++)
				{
					total += cardValues[i] * (i + 1);
				}
			}
			else
			{
				List<int> cardValues = player2.Cards.Reverse().ToList();
				for (int i = 0; i < player2.Cards.Count; i++)
				{
					total += cardValues[i] * (i + 1);
				}
			}

			return total.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			List<string> playerLines = Input.Split("\r\n\r\n").ToList();

			Player player1 = new Player()
			{
				Id = 1,
				Cards = new Queue<int>(playerLines[0].Split("\r\n").Skip(1).Select(int.Parse))
			};

			Player player2 = new Player()
			{
				Id = 2,
				Cards = new Queue<int>(playerLines[1].Split("\r\n").Skip(1).Select(int.Parse))
			};

			// keep playing till one person runs out of cards
			int gameDepth = 1;
			long total = 0;
			if (PlaySubGame(player1, player2, gameDepth))
			{
				List<int> cardValues = player1.Cards.Reverse().ToList();
				for (int i = 0; i < player1.Cards.Count; i++)
				{
					total += cardValues[i] * (i + 1);
				}
			}
			else
			{
				List<int> cardValues = player2.Cards.Reverse().ToList();
				for (int i = 0; i < player2.Cards.Count; i++)
				{
					total += cardValues[i] * (i + 1);
				}
			}

			return total.ToString();
		}

		public bool PlaySubGame(Player player1, Player player2, int gameDepth)
		{
			List<GameState> seenStates = new List<GameState>();
			//int round = 0;

			// keep playing till one person runs out of cards
			while (player1.Cards.Count > 0 && player2.Cards.Count > 0)
			{
				//round++;
				//Console.WriteLine($"game {gameDepth}, round {round}");

				// check if gamestate occured before, then count it as a win for player1
				GameState startingState = new GameState()
				{
					Player1Cards = player1.Cards.ToList(),
					Player2Cards = player2.Cards.ToList(),
				};
				if (seenStates.Contains(startingState))
				{
					return true;
				}

				seenStates.Add(startingState);

				int player1Card = player1.Cards.Dequeue();
				int player2Card = player2.Cards.Dequeue();

				//Console.WriteLine($"Player 1 plays: {player1Card}");
				//Console.WriteLine($"Player 2 plays: {player2Card}");

				// special round
				if (player1.Cards.Count >= player1Card && player2.Cards.Count >= player2Card)
				{
					// save current state
					GameState state = new GameState()
					{
						Player1Cards = player1.Cards.ToList(),
						Player2Cards = player2.Cards.ToList(),
					};

					// play subgame to determine winner
					//Console.WriteLine($"Playing subgame to determine winner, current depth {gameDepth + 1}");
					player1.Cards = new Queue<int>(player1.Cards.Take(player1Card));
					player2.Cards = new Queue<int>(player2.Cards.Take(player2Card));

					if (PlaySubGame(player1, player2, gameDepth + 1))
					{
						//Console.WriteLine("player 1 won subgame");

						// restore state
						player1.Cards = new Queue<int>(state.Player1Cards);
						player2.Cards = new Queue<int>(state.Player2Cards);

						// give spoils to winner
						player1.Cards.Enqueue(player1Card);
						player1.Cards.Enqueue(player2Card);
					}
					else
					{
						//Console.WriteLine("player 2 won subgame");

						player1.Cards = new Queue<int>(state.Player1Cards);
						player2.Cards = new Queue<int>(state.Player2Cards);

						player2.Cards.Enqueue(player2Card);
						player2.Cards.Enqueue(player1Card);
					}
				}
				else // normal round
				{
					if (player1Card > player2Card)
					{
						//Console.WriteLine("Player 1 won regular round");
						player1.Cards.Enqueue(player1Card);
						player1.Cards.Enqueue(player2Card);
					}
					else
					{
						//Console.WriteLine("Player 2 won regular round");

						player2.Cards.Enqueue(player2Card);
						player2.Cards.Enqueue(player1Card);
					}
				}
			}

			//Console.WriteLine($"Subgame {gameDepth} won by {(player1.Cards.Count > 0 ? "player 1" : "player2")}");
			return player1.Cards.Count > 0;
		}

		public class Player
		{
			public int Id { get; set; }
			public Queue<int> Cards { get; set; }
		}

		public class GameState
		{
			public List<int> Player1Cards { get; set; }
			public List<int> Player2Cards { get; set; }

			protected bool Equals(GameState other)
			{
				if (other.Player1Cards.Count != Player1Cards.Count ||
					other.Player2Cards.Count != Player2Cards.Count)
				{
					return false;
				}

				for (int i = 0; i < Player1Cards.Count; i++)
				{
					if (other.Player1Cards[i] != Player1Cards[i])
					{
						return false;
					}
				}

				for (int i = 0; i < Player2Cards.Count; i++)
				{
					if (other.Player2Cards[i] != Player2Cards[i])
					{
						return false;
					}
				}

				return true;
			}

			/// <inheritdoc />
			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((GameState) obj);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return HashCode.Combine(Player1Cards, Player2Cards);
			}
		}
	}
}