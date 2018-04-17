using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Commands;
using System.Timers;

namespace Minesweeper.Library
{
   public class Gameboard : BindableBase
   {
      private int _rows;
      private int _columns;
      private int _numMines;
      private List<Tile> _tiles;
      private DifficultySetting _settings;
      private bool _gameOver;
      private bool _win;
      private int _playTime;
      private Timer _timer;

      private int _tilesLeft;
      private readonly INeighboringTileFinder _neighboringTileFinder;

      #region properties
      public int Rows
      {
         get { return _rows; }
         set { SetProperty(ref _rows, value); }
      }

      public int Columns
      {
         get { return _columns; }
         set { SetProperty(ref _columns, value); }
      }

      public List<Tile> Tiles
      {
         get { return _tiles; }
         set { SetProperty(ref this._tiles, value); }
      }

      public int NumMines
      {
         get { return _numMines; }
         set { SetProperty(ref this._numMines, value); }
      }

      public DifficultySetting Settings
      {
         get { return _settings; }
         set { SetProperty(ref this._settings, value); }
      }

      public bool GameOver
      {
         get { return _gameOver; }
         set { SetProperty(ref this._gameOver, value); }
      }

      public bool Win
      {
         get { return _win; }
         set { SetProperty(ref this._win, value); }
      }

      public int PlayTime
      {
         get { return _playTime; }
         set { SetProperty(ref this._playTime, value); }
      }

      public DelegateCommand<Tile> RevealCommand { get; private set; }

      public DelegateCommand<Tile> ToggleTileMarkedCommand { get; private set; }

      public DelegateCommand<Tile> QuickRevealNeighborsCommand { get; private set; }

      #endregion

      public Gameboard(INeighboringTileFinder neighboringTileFinder)
      {
         _neighboringTileFinder = neighboringTileFinder;
         this.RevealCommand = new DelegateCommand<Tile>(RevealTiles);
         this.ToggleTileMarkedCommand = new DelegateCommand<Tile>(ToggleTileMarked);
         this.QuickRevealNeighborsCommand = new DelegateCommand<Tile>(QuickRevealNeighbors);

         this.GameOver = false;
      }

      public Gameboard(DifficultyLevel level, INeighboringTileFinder neighboringTileFinder) : this(neighboringTileFinder)
      {
         this.Settings = new DifficultySetting(level);

         Tiles = new List<Tile>();
         SetDimensions(this.Settings.Rows, this.Settings.Columns);
      }

      public void InitializeGameBoard()
      {
         PopulateTiles();
         SeedMines();
      }

      public void StartGame()
      {
         SetNeighborMineCounts(this.Tiles, this.Columns);
         StartPlayTimer();
      }

      public void SetNeighborMineCounts(List<Tile> tiles, int columns)
      {
         List<Tile> neighbors;

         foreach (Tile tile in tiles)
         {
            if (tile.IsMine)
            {
               neighbors = _neighboringTileFinder.GetAllNeighbors(tile.TileIndex, tiles, columns);
               foreach (Tile neighbor in neighbors)
                  neighbor.NumNeighborMines = neighbor.NumNeighborMines + 1;
            }
         }
      }

      public void RevealTiles(Tile tile)
      {
         if (tile.IsMarked || tile.IsRevealed)
            return;

         tile.Reveal();
         this._tilesLeft--;

         if (tile.IsMine)
         {
            EndGame(false);
            return;
         }

         if (tile.NumNeighborMines == 0 && !tile.IsMine)
         {
            Queue<Tile> checkQueue = new Queue<Tile>();
            checkQueue.Enqueue(tile);
            while (checkQueue.Count > 0)
            {
               Tile neighbor = checkQueue.Dequeue();

               if (!neighbor.IsRevealed)
               {
                  neighbor.Reveal();
                  this._tilesLeft--;
               }

               if (neighbor.NumNeighborMines == 0)
               {
                  foreach (Tile newNeighbor in _neighboringTileFinder.GetAllNeighbors(neighbor.TileIndex, this.Tiles, this.Columns))
                  {
                     if (!newNeighbor.IsRevealed && !checkQueue.Contains(newNeighbor))
                        checkQueue.Enqueue(newNeighbor);
                  }
               }
            }
         }

         if (this._tilesLeft == this.Settings.Mines)
            EndGame(true);
      }

      public void ToggleTileMarked(Tile tile)
      {
         if (tile.IsMarked)
            this.NumMines++;
         else if (this.NumMines < 1)
            return;
         else
            this.NumMines--;

         tile.ToggleMark();
      }

      public void QuickRevealNeighbors(Tile tile)
      {
         var allNeighbors = _neighboringTileFinder.GetAllNeighbors(tile.TileIndex, this.Tiles, this.Columns);
         if (tile.NumNeighborMines == allNeighbors.Count(t => t.IsMarked))
         {
            var tilesToReveal = allNeighbors.Where(t => !t.IsMarked && !t.IsRevealed);
            foreach (var toReveal in tilesToReveal)
            {
               RevealTiles(toReveal);
            }
         }
      }

      private void PopulateTiles()
      {
         Tiles = new List<Tile>();
         for (int i = 0; i < this.Rows * this.Columns; i++)
         {
            this.Tiles.Add(new Tile(i));
         }
         this._tilesLeft = this.Rows * this.Columns;
      }

      private void SeedMines()
      {
         Random rng = new Random();
         this.NumMines = 0;

         while (this.NumMines < this.Settings.Mines)
         {
            int position = rng.Next(0, (this.Rows * this.Columns));
            if (!this.Tiles[position].IsMine)
            {
               this.Tiles[position].IsMine = true;
               this.NumMines++;
            }
         }
      }

      private void SetDimensions(int rows, int columns)
      {
         this.Rows = rows;
         this.Columns = columns;
      }

      private void StartPlayTimer()
      {
         this._timer = new Timer(1000);
         this._timer.Elapsed += ((s, e) => { this.PlayTime++; });
         this._timer.Enabled = true;
         this._timer.Start();
      }

      private void StopPlayTimer()
      {
         this._timer.Stop();
         this._timer.Dispose();
      }

      private void EndGame(bool win)
      {
         StopPlayTimer();
         if (win)
            this.Win = true;
         else
         {
            RevealAllMines();
            this.GameOver = true;
         }
      }

      private void RevealAllMines()
      {
         foreach (var tile in _tiles.Where(t => t.IsMine))
         {
            tile.Reveal();
         }
      }
   }
}
