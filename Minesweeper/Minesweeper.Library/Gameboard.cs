using System;
using System.Collections.Generic;
using System.Timers;

namespace Minesweeper.Library
{
   public class Gameboard : IGameboard
   {
      private int _rows;
      private int _columns;
      private List<Tile> _tiles;

      private readonly DifficultySetting _settings;

      public List<Tile> Tiles => _tiles;
      public DifficultySetting Settings => _settings;
      public int Rows => _rows;
      public int Columns => _columns;

      public Gameboard(DifficultySetting settings)
      {
         _settings = settings;
         SetDimensions(settings.Rows, settings.Columns);
         PopulateTiles();
         SeedMines();
      }

      public IGameboard With(List<Tile> newTiles)
      {
         _tiles = newTiles;
         return this;
      }

      private void SetDimensions(int rows, int columns)
      {
         this._rows = rows;
         this._columns = columns;
      }

      private void PopulateTiles()
      {
         _tiles = new List<Tile>();
         for (int i = 0; i < _rows * _columns; i++)
         {
            _tiles.Add(new Tile(i));
         }
      }

      private void SeedMines()
      {
         Random rng = new Random();
         var numMines = 0;

         while (numMines < _settings.Mines)
         {
            int position = rng.Next(0, (_rows * _columns));
            if (!_tiles[position].IsMine)
            {
               _tiles[position].IsMine = true;
               numMines++;
            }
         }
      }
   }
}
