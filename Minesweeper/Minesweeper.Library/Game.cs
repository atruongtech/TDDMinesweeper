using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Commands;
using System.Timers;

[assembly: InternalsVisibleTo("Minesweeper.Library.Test")]
namespace Minesweeper.Library
{
   public class Game : BindableBase, IGame
   {
      private int _numMines;
      private DifficultySetting _settings;
      private bool _gameOver;
      private bool _win;
      private int _playTime;
      private Timer _timer;
      private int _tilesLeft;

      private readonly ITileGameLogic _tileGameLogic;
      private readonly IGameboard _gameboard;

      #region properties
      public int Rows => _gameboard.Rows;

      public int Columns => _gameboard.Columns;

      public List<Tile> Tiles => _gameboard.Tiles;

      public int NumMines
      {
         get => _numMines;
         private set => SetProperty(ref _numMines, value);
      }

      public DifficultySetting Settings => _gameboard.Settings;

      public bool GameOver
      {
         get => _gameOver;
         private set => SetProperty(ref _gameOver, value);
      }

      public bool Win
      {
         get => _win;
         private set => SetProperty(ref _win, value);
      }

      public int PlayTime
      {
         get => _playTime;
         private set => SetProperty(ref _playTime, value);
      }

      public int TilesLeft => _tilesLeft;

      public DelegateCommand<Tile> RevealCommand { get; private set; }

      public DelegateCommand<Tile> ToggleTileMarkedCommand { get; private set; }

      public DelegateCommand<Tile> QuickRevealNeighborsCommand { get; private set; }

      #endregion

      public Game(ITileGameLogic tileGameLogic, IGameboard gameboard)
      {
         _tileGameLogic = tileGameLogic;
         _gameboard = gameboard;
         _numMines = gameboard.Tiles.Count(t => t.IsMine);
         _tilesLeft = gameboard.Tiles.Count;

         this.RevealCommand = new DelegateCommand<Tile>(RevealTiles);
         this.ToggleTileMarkedCommand = new DelegateCommand<Tile>(ToggleTileMarked);
         this.QuickRevealNeighborsCommand = new DelegateCommand<Tile>(QuickRevealNeighbors);

         this.GameOver = false;
         this._timer = new Timer(1000);
      }

      public void StartGame()
      {
         SetNeighborMineCounts();
         StartPlayTimer();
      }

      private void SetNeighborMineCounts()
      {
         _gameboard.With(_tileGameLogic.SetNeighborMineCounts(_gameboard.Tiles, _gameboard.Columns));
      }

      public void RevealTiles(Tile tile)
      {
         _gameboard.With(_tileGameLogic.RevealTiles(tile, _gameboard.Tiles, _gameboard.Columns, this));
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
         _gameboard.With(_tileGameLogic.QuickRevealNeighbors(tile, _gameboard.Tiles, _gameboard.Columns, this));
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

      public void EndGame(bool win)
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
         foreach (var tile in _gameboard.Tiles.Where(t => t.IsMine))
         {
            tile.Reveal();
         }
      }

      public void DecrementTileCounter()
      {
         _tilesLeft--;
      }

      public void IncrementTileCounter()
      {
         _tilesLeft++;
      }
   }
}
