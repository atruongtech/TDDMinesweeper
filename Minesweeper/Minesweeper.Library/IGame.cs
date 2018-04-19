namespace Minesweeper.Library
{
   public interface IGame
   {
      DifficultySetting Settings { get; }
      int TilesLeft { get; }

      void DecrementTileCounter();
      void IncrementTileCounter();
      void EndGame(bool win);
   }
}
