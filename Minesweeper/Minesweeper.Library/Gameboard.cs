using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;

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

        private int _tilesLeft;

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

        public DelegateCommand<Tile> RevealCommand
        {
            get;
            private set;
        }
        
        public DelegateCommand<Tile> ToggleTileMarkedCommand
        {
            get;
            private set;
        }

        

        #endregion

        public Gameboard()
        {
            this.RevealCommand = new DelegateCommand<Tile>(RevealTiles);
            this.ToggleTileMarkedCommand = new DelegateCommand<Tile>(ToggleTileMarked);

            this.GameOver = false;
        }

        public Gameboard(DifficultyLevel level) : this()
        {
            this.Settings = new DifficultySetting(level);

            Tiles = new List<Tile>();
            SetDimensions(this.Settings.Rows, this.Settings.Columns);
            PopulateTiles();
            SeedMines();
            SetNeighborMineCounts(this.Tiles, this.Columns);

        }

        public static void SetNeighborMineCounts(List<Tile> tiles, int columns)
        {
            List<Tile> neighbors;

            foreach(Tile tile in tiles)
            {
                if (tile.IsMine)
                {
                    neighbors = Gameboard.GetAllNeighbors(tile.TileIndex, tiles, columns);
                    foreach (Tile neighbor in neighbors)
                        neighbor.NumNeighborMines = neighbor.NumNeighborMines + 1;
                }
            }
        }

        public static List<Tile> GetAllNeighbors(int index, List<Tile> tiles, int columns)
        {
            List<Tile> neighbors = new List<Tile>();

            // top, middle, bottom
            // left, middle, right
            int tl = index - columns - 1;
            int tm = index - columns;
            int tr = index - columns + 1;

            int ml = index - 1;
            int mr = index + 1;

            int bl = index + columns - 1;
            int bm = index + columns;
            int br = index + columns + 1;

            // index is not the first column
            // and neighbor is a valid index
            if (index % columns > 0 && tl >= 0)
                neighbors.Add(tiles[tl]);

            if (tm >= 0)
                neighbors.Add(tiles[tm]);

            // index is not in last column
            // and neighbor is a valid index
            if (index % columns < (columns - 1) && tr >= 0)
                neighbors.Add(tiles[tr]);

            if (index % columns > 0)
                neighbors.Add(tiles[ml]);

            if (index % columns < (columns - 1))
                neighbors.Add(tiles[mr]);

            if (index % columns > 0 && bl < (tiles.Count))
                neighbors.Add(tiles[bl]);

            if (bm < tiles.Count)
                neighbors.Add(tiles[bm]);

            if (index % columns < (columns - 1) && br < (tiles.Count))
                neighbors.Add(tiles[br]);

            return neighbors;

        }

        public void RevealTiles(Tile tile)
        {
            if (tile.IsMarked)
                return;

            tile.Reveal();
            this._tilesLeft--;

            if (tile.IsMine)
            {
                this.GameOver = true;
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
                        foreach(Tile newNeighbor in Gameboard.GetAllNeighbors(neighbor.TileIndex, this.Tiles, this.Columns))
                        {
                            if (!newNeighbor.IsRevealed && !checkQueue.Contains(newNeighbor))
                                checkQueue.Enqueue(newNeighbor);
                        }
                    }
                }
            }

            if (this._tilesLeft == this.Settings.Mines)
                this.Win = true;
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

        private void PopulateTiles()
        {
            for (int i = 0; i < this.Rows * this.Columns; i ++)
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
    }
}
