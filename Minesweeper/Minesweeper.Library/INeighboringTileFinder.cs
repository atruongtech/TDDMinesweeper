using System.Collections.Generic;

namespace Minesweeper.Library
{
   public interface INeighboringTileFinder
   {
      List<Tile> GetAllNeighbors(int index, List<Tile> tiles, int columns);
   }
}
