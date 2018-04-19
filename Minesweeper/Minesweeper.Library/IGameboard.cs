using System.Collections.Generic;

namespace Minesweeper.Library
{
   public interface IGameboard
   {
      IGameboard With(List<Tile> newTiles);

      int Columns { get; }
      int Rows { get; }
      DifficultySetting Settings { get; }
      List<Tile> Tiles { get; }
   }
}