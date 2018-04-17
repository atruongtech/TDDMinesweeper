using System;
using System.Collections.Generic;

namespace Minesweeper.Library
{
   public class NeighboringTileFinder : INeighboringTileFinder
   {
      public List<Tile> GetAllNeighbors(int index, List<Tile> tiles, int columns)
      {
         List<Tile> neighbors = new List<Tile>();
         Func<int, bool> indexIsValid = IndexIsValidForNTiles(tiles.Count);

         // top, middle, bottom
         // left, middle, right
         int topLeftIndex = index - columns - 1;
         int topMiddleIndex = index - columns;
         int topRightIndex = index - columns + 1;

         int middleLeftIndex = index - 1;
         int middleRightIndex = index + 1;

         int bottomLeftIndex = index + columns - 1;
         int bottomMiddleIndex = index + columns;
         int bottomRightIndex = index + columns + 1;

         if (IndexIsNotInFirstColumn(index, columns))
         {
            neighbors.Add(tiles[middleLeftIndex]);

            if (indexIsValid(topLeftIndex))
               neighbors.Add(tiles[topLeftIndex]);
            if (indexIsValid(bottomLeftIndex))
               neighbors.Add(tiles[bottomLeftIndex]);
         }

         if (indexIsValid(topMiddleIndex))
            neighbors.Add(tiles[topMiddleIndex]);

         if (IndexIsNotInLastColumn(index, columns))
         {
            neighbors.Add(tiles[middleRightIndex]);

            if (indexIsValid(topRightIndex))
               neighbors.Add(tiles[topRightIndex]);
            if (indexIsValid(bottomRightIndex))
               neighbors.Add(tiles[bottomRightIndex]);
         }

         if (indexIsValid(bottomMiddleIndex))
            neighbors.Add(tiles[bottomMiddleIndex]);

         return neighbors;

      }

      private static bool IndexIsNotInLastColumn(int index, int columns)
      {
         return index % columns < (columns - 1);
      }

      private static bool IndexIsNotInFirstColumn(int index, int columns)
      {
         return index % columns > 0;
      }

      private Func<int, bool> IndexIsValidForNTiles(int n)
      {
         return (index) => index >= 0 && index < n;
      }
   }
}