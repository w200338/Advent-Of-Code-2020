using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days.Day21
{
	public class Day21 : Day
	{
		private HashSet<string> safeIngredients;
		private Dictionary<string, HashSet<string>> possibleTranslations;

		/// <inheritdoc />
		public override string Part1()
		{
			List<string> lines = Input.Split("\r\n").ToList();

			// parse input
			List<Food> foods = new List<Food>();
			foreach (string line in lines)
			{
				string[] lineParts = line.Split(" (contains ");

				foods.Add(new Food()
				{
					Ingredients = lineParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(),
					Allergens = lineParts[1].Substring(0, lineParts[1].Length - 1)
						.Split(", ")
						.ToList()
				});
			}

			safeIngredients = new HashSet<string>();
			possibleTranslations = new Dictionary<string, HashSet<string>>();

			foreach (Food food in foods)
			{
				safeIngredients.UnionWith(food.Ingredients);

				foreach (string allergen in food.Allergens)
				{
					if (possibleTranslations.ContainsKey(allergen))
					{
						possibleTranslations[allergen].RemoveWhere(possibleIngredient => !food.Ingredients.Contains(possibleIngredient));
					}
					else
					{
						possibleTranslations.Add(allergen, food.Ingredients.ToHashSet());
					}
				}
			}

			safeIngredients.RemoveWhere(s => possibleTranslations.SelectMany(kv => kv.Value).Contains(s));

			return foods.SelectMany(f => f.Ingredients)
				.GroupBy(i => i)
				.Where(group => safeIngredients.Contains(group.Key))
				.Select(g => g.Count())
				.Sum()
				.ToString();
		}

		/// <inheritdoc />
		public override string Part2()
		{
			while (possibleTranslations.Any(pt => pt.Value.Count > 1))
			{
				List<KeyValuePair<string, HashSet<string>>> certainTranslations = possibleTranslations
					.Where(kv => kv.Value.Count == 1)
					.ToList();

				foreach (KeyValuePair<string, HashSet<string>> certainTranslation in certainTranslations)
				{
					foreach (KeyValuePair<string, HashSet<string>> possibleTranslation in possibleTranslations)
					{
						if (possibleTranslation.Key != certainTranslation.Key)
						{
							possibleTranslation.Value.Remove(certainTranslation.Value.First());
						}
					}
				}
			}

			return string.Join(',', possibleTranslations.OrderBy(kv => kv.Key).Select(kv => kv.Value.First()));
		}
	}

	public class Food
	{
		public List<string> Ingredients { get; set; }
		public List<string> Allergens { get; set; }
	}
}