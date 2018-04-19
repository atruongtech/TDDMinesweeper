namespace Minesweeper.Library
{
   public interface IGame
   {
      DifficultySetting Settings { get; }
      int TilesLeft { get; }
      int NumMines { get; }

      void DecrementTileCounter();
      void IncrementTileCounter();
      void DecrementMineCounter();
      void IncrementMineCounter();
      void EndGame(bool win);
   }
}
