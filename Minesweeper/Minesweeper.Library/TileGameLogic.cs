using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Library
{
   public class TileGameLogic : ITileGameLogic
   {
      private readonly INeighboringTileFinder _neighboringTileFinder;

      public TileGameLogic(INeighboringTileFinder neighboringTileFinder)
      {
         _neighboringTileFinder = neighboringTileFinder;
      }

      public List<Tile> SetNeighborMineCounts(List<Tile> tiles, int columns)
      {
         foreach (Tile tile in tiles)
         {
            if (tile.IsMine)
            {
               var neighbors = _neighboringTileFinder.GetAllNeighbors(tile.TileIndex, tiles, columns);
               foreach (Tile neighbor in neighbors)
                  neighbor.NumNeighborMines = neighbor.NumNeighborMines + 1;
            }
         }

         return tiles;
      }

      public List<Tile> RevealTiles(Tile tileClicked, List<Tile> allTiles, int columns, IGame control)
      {
         var returnTiles = allTiles;

         if (tileClicked.IsMarked || tileClicked.IsRevealed)
            return returnTiles;

         tileClicked.Reveal();
         control.DecrementTileCounter();

         if (tileClicked.IsMine)
         {
            control.EndGame(false);
            return returnTiles;
         }

         if (tileClicked.NumNeighborMines == 0 && !tileClicked.IsMine)
         {
            returnTiles = VisitNeighborTilesByQueue(tileClicked, allTiles, columns, control);
         }

         if (control.TilesLeft == control.Settings.Mines)
            control.EndGame(true);

         return returnTiles;
      }

      private List<Tile> VisitNeighborTilesByQueue(Tile tile, List<Tile> tiles, int columns, IGame control)
      {
         Queue<Tile> checkQueue = new Queue<Tile>();
         checkQueue.Enqueue(tile);
         while (checkQueue.Count > 0)
         {
            Tile neighbor = checkQueue.Dequeue();

            if (!neighbor.IsRevealed)
            {
               neighbor.Reveal();
               control.DecrementTileCounter();
            }

            if (neighbor.NumNeighborMines == 0)
            {
               foreach (Tile newNeighbor in _neighboringTileFinder.GetAllNeighbors(neighbor.TileIndex, tiles,
                  columns))
               {
                  if (!newNeighbor.IsRevealed && !checkQueue.Contains(newNeighbor))
                     checkQueue.Enqueue(newNeighbor);
               }
            }
         }

         return tiles;
      }

      public List<Tile> QuickRevealNeighbors(Tile tileClicked, List<Tile> allTiles, int columns, IGame control)
      {
         var returnTiles = allTiles;
         var allNeighbors = _neighboringTileFinder.GetAllNeighbors(tileClicked.TileIndex, allTiles, columns);
         if (tileClicked.NumNeighborMines == allNeighbors.Count(t => t.IsMarked))
         {
            var tilesToReveal = allNeighbors.Where(t => !t.IsMarked && !t.IsRevealed);
            foreach (var toReveal in tilesToReveal)
            {
               returnTiles = RevealTiles(toReveal, allTiles, columns, control);
            }
         }

         return returnTiles;
      }
   }
}
