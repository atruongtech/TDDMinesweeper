using System.Collections.Generic;

namespace Minesweeper.Library
{
   public interface ITileGameLogic
   {
      List<Tile> SetNeighborMineCounts(List<Tile> tiles, int columns);
      List<Tile> RevealTiles(Tile tileClicked, List<Tile> allTiles, int columns, IGame control);
      List<Tile> QuickRevealNeighbors(Tile tileClicked, List<Tile> allTiles, int columns, IGame control);
   }
}
