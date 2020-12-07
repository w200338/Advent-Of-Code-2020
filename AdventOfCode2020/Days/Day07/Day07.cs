using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day07
{
	public class Day07 : Day
	{
		/// <inheritdoc />
		public override string Part1()
		{
			string[] lines = Input.Split("\r\n");

			List<Bag> bags = new List<Bag>();

			foreach (string line in lines)
			{
				string[] lineParts = line.Split(" bags contain ");
				Bag bag = new Bag();
				bag.Name = ExtractName(lineParts[0]);

				if (!lineParts[1].Contains("no other"))
				{
					bag.Contains = lineParts[1].Split(',').Select(s => s.Substring(2)).Select(ExtractName).ToList();
				}

				bags.Add(bag);
			}

			HashSet<string> found = new HashSet<string>();
			Queue<string> bagQueue = new Queue<string>();
			bagQueue.Enqueue("shiny gold");
			
			while (bagQueue.Count > 0)
			{
				string current = bagQueue.Dequeue();

				foreach (Bag bag in bags.Where(b => b.Contains.Contains(current)))
				{
					bagQueue.Enqueue(bag.Name);
					found.Add(bag.Name);
				}
			}

			return found.Count.ToString();
			/*
			List<Bag> bags = new List<Bag>();
			List<Bag> explored = new List<Bag>();

			foreach (string rule in rules)
			{
				string[] ruleParts = rule.Split("contain");

				/*
				Bag newBag = new Bag();
				newBag.Name = ruleParts[0];

				foreach (string subBag in ruleParts[1].Split(','))
				{
					string trim = subBag.Replace(".", "").Trim();

					if (trim == "no other parts")
					{
						break;
					}

					if (trim.EndsWith('s'))

					newBag.Contains.Add(subBag);
				}

				bags.Add(newBag);
				

				
				bags.Add(new Bag()
				{
					Name = ruleParts[0].Trim(),
					Contains = ruleParts[1].Split(',').Select(s => s.Substring(2, s.Length - 2).Trim().Replace(".", "")).Select(s =>
					{
						if (!s.EndsWith('s'))
						{
							return s + 's';
						}

						return s;
					}).ToList()
				});
			}

			int totalGold = 0;
			foreach (Bag bag in bags)
			{
				totalGold += SubBags(bags, explored, bag) > 0 ? 1 : 0;
			}
			*/

			//return totalGold.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			string[] lines = Input.Split("\r\n");

			List<DetailedBag> bags = new List<DetailedBag>();

			foreach (string line in lines)
			{
				string[] lineParts = line.Split(" bags contain ");
				DetailedBag bag = new DetailedBag();
				bag.Name = ExtractName(lineParts[0]);

				if (!lineParts[1].Contains("no other"))
				{
					string[] subBags = lineParts[1].Split(',');
					List<string> names = subBags.Select(s => s.Substring(2)).Select(ExtractName).ToList();
					List<int> amounts = subBags.Select(s => s[0].ToString()).Select(int.Parse).ToList();

					for (int i = 0; i < names.Count; i++)
					{
						bag.SubBags.Add(new SubBag()
						{
							Amount = amounts[i],
							Name = names[i]
						});
					}
				}

				bags.Add(bag);
			}

			int totalGold = 0;
			Queue<SubBag> bagQueue = new Queue<SubBag>();
			bagQueue.Enqueue(new SubBag()
			{
				Amount = 1,
				Name = "shiny gold"
			});

			while (bagQueue.Count > 0)
			{
				SubBag current = bagQueue.Dequeue();

				foreach (SubBag subBag in bags.First(b => b.Name == current.Name).SubBags)
				{
					totalGold += current.Amount * subBag.Amount;
					bagQueue.Enqueue(new SubBag()
					{
						Name = subBag.Name,
						Amount = current.Amount * subBag.Amount
					});
				}

				/*
				foreach (DetailedBag bag in bags.Where(b => b.SubBags.Any(sb => sb.Name == current.Name)))
				{
					SubBag subBag = bag.SubBags.First(b => b.Name == current.Name);

					totalGold += current.Amount * subBag.Amount;
					bagQueue.Enqueue(new SubBag()
					{
						Name = bag.Name,
						Amount = current.Amount * subBag.Amount
					});
				}
				*/
			}

			return totalGold.ToString();

		}

		private string ExtractName(string input)
		{
			return input.Split("bag")[0].Trim();
		}

		public int SubBags(List<Bag> bags, Bag bag)
		{
			if (bag.Name == "no other")
			{
				return 0;
			}

			if (bag.Name == "shiny gold")
			{
				return 1;
			}

			int totalGoldBags = 0;
			foreach (string contains in bag.Contains)
			{
				Bag foundBag = bags.FirstOrDefault(b => b.Name.Contains(contains));

				if (foundBag != null)
				{
					totalGoldBags += SubBags(bags, foundBag);
				}
			}

			return totalGoldBags;
		}
	}

	public class Bag
	{
		public string Name { get; set; }
		public List<string> Contains { get; set; } = new List<string>();
	}

	public class DetailedBag
	{
		public string Name { get; set; }
		public List<SubBag> SubBags { get; set; } = new List<SubBag>();
	}

	public class SubBag
	{
		public string Name { get; set; }
		public int Amount { get; set; }
	}
}