using System.Collections.Generic;
using AdventOfCode2020.Days.Day20;
using NUnit.Framework;

namespace AdventOfCode2020.Tests
{
	[TestFixture]
	public class Day20PermuteTests
	{
		/*
		[Test]
		public void Should_Assign_All_Possible_States([Values(0,1,2,3)]int rotation, [Values(true, false)]bool flipHor, [Values(true, false)] bool flipVer)
		{
			Tile testTile = new Tile(new Day20.Tile()
			{
				TopString = ".",
				BottomString = ".",
				LeftString = ".",
				RightString = ".",
			});

			List<State> actualStates = new List<State>();

			foreach (object _ in GridMaker.Permute(testTile))
			{
				actualStates.Add(testTile.State);
			}

			Assert.That(actualStates, Does.Contain(new State(0, rotation, flipHor, flipVer)));
		}

		[Test]
		public void Order_Of_Operations_Does_Not_Matter()
		{
			Tile testTile1 = new Tile(new Day20.Tile()
			{
				TopString = "AB",
				BottomString = "CD",
				LeftString = "EF",
				RightString = "GH",
			});

			testTile1.FlipHorizontalAxis();
			testTile1.RotateRight();
			testTile1.RotateRight();

			Tile testTile2 = new Tile(new Day20.Tile()
			{
				TopString = "AB",
				BottomString = "CD",
				LeftString = "EF",
				RightString = "GH",
			});

			testTile2.RotateRight();
			testTile2.RotateRight();
			testTile2.FlipHorizontalAxis();

			Assert.That(testTile1.Top, Is.EqualTo(testTile2.Top));
			Assert.That(testTile1.Bottom, Is.EqualTo(testTile2.Bottom));
			Assert.That(testTile1.Left, Is.EqualTo(testTile2.Left));
			Assert.That(testTile1.Right, Is.EqualTo(testTile2.Right));
		}
		*/
	}
}