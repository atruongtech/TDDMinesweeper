using System.Collections.Generic;
using NUnit.Framework;

namespace Minesweeper.Library.Test
{
   [TestFixture]
   public class NeighboringMineFinderTest
   {
      [Test]
      public void Easy_GetAllNeighbors_Edges_Returns_AppropriateNeighbors(
          [Values(23, 24, 60, 4, 8, 55, 62, 1)] int index)
      {
         /* - 8 - - 4 - - -
          * 5 - - - - - - -
          * - - - - - - - 1
          * 2 - - - - - - -
          * - - - - - - - -
          * - - - - - - - -
          * - - - - - - - 6
          * - - - - 3 - 7 -
          */
         var tiles = new List<Tile>();
         for (int i = 0; i < 64; i ++)
            tiles.Add(new Tile());

         const int nColumns = 8;
         var neighboringTileFinder = new NeighboringTileFinder();
         var neighbors = neighboringTileFinder.GetAllNeighbors(index, tiles, nColumns);
         Assert.AreEqual(5, neighbors.Count);
      }

      [Test]
      public void Easy_GetAllNeighbors_Corners_Returns_AppropriateNeighbors(
          [Values(0, 63, 7, 56)] int index)
      {
         /* 1 - - - - - - 3
          * - - - - - - - -
          * - - - - - - - -
          * - - - - - - - -
          * - - - - - - - -
          * - - - - - - - -
          * - - - - - - - -
          * 4 - - - - - - 2
          */

         var tiles = new List<Tile>();
         for (int i = 0; i < 64; i++)
            tiles.Add(new Tile());

         const int nColumns = 8;
         var neighboringTileFinder = new NeighboringTileFinder();
         var neighbors = neighboringTileFinder.GetAllNeighbors(index, tiles, nColumns);
         Assert.AreEqual(3, neighbors.Count);
      }

      [Test]
      public void Easy_GetAllNeighbors_General_Returns_AppropriateNeighbors(
          [Values(27, 33, 36, 9, 54, 14, 49)] int index)
      {
         /* - - - - - - - -
          * - 4 - - - - 6 -
          * - - - - - - - -
          * - - - - 1 - - -
          * - - 2 - - 3 - -
          * - - - - - - - -
          * - 7 - - - - 5 -
          * - - - - - - - -
          */

         var tiles = new List<Tile>();
         for (int i = 0; i < 64; i++)
            tiles.Add(new Tile());

         const int nColumns = 8;
         var neighboringTileFinder = new NeighboringTileFinder();
         var neighbors = neighboringTileFinder.GetAllNeighbors(index, tiles, nColumns);
         var neighbor = tiles[index + 1];

         Assert.IsTrue(neighbors.Contains(neighbor));
         Assert.AreEqual(8, neighbors.Count);
      }
   }
}
