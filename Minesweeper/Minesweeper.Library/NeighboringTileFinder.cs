using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Library
{
   public class NeighboringTileFinder : INeighboringTileFinder
   {
      public List<Tile> GetAllNeighbors(int index, List<Tile> tiles, int columns)
      {
         var indicesToCheck = new List<int>(CalculateMiddleColumnIndices(index, columns));

         if (IndexIsNotInFirstColumn(index, columns))
            indicesToCheck.AddRange(CalculateLeftColumnIndices(index, columns));

         if (IndexIsNotInLastColumn(index, columns))
            indicesToCheck.AddRange(CalculateRightColumnIndices(index, columns));

         Func<int, bool> indexIsValid = IndexIsValidForNTiles(tiles.Count);
         return indicesToCheck.Where(indexIsValid).Select(i => tiles[i]).ToList();

      }

      private IEnumerable<int> CalculateRightColumnIndices(int index, int nColumns)
      {
         return new List<int> {index - nColumns + 1, index + 1, index + nColumns + 1};
      }

      private IEnumerable<int> CalculateLeftColumnIndices(int index, int nColumns)
      {
         return new List<int> { index - nColumns - 1, index - 1, index + nColumns - 1 };
      }

      private IEnumerable<int> CalculateMiddleColumnIndices(int index, int nColumns)
      {
         return new List<int> {index - nColumns, index + nColumns};
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