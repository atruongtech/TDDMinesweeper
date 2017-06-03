using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Minesweeper.Library
{
    public class Gameboard : BindableBase
    {
        private int _rows;
        private int _columns;
        private List<Tile> _tiles;
        private DifficultySetting _settings;

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

        public DifficultySetting Settings
        {
            get { return _settings; }
            set { SetProperty(ref this._settings, value); }
        }
        #endregion

        public Gameboard(DifficultyLevel level)
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

        private void PopulateTiles()
        {
            for (int i = 0; i < this.Rows * this.Columns; i ++)
            {
                this.Tiles.Add(new Tile(i));
            }
        }

        private void SeedMines()
        {
            Random rng = new Random();
            int nMines = 0;

            while (nMines < this.Settings.Mines)
            {
                int position = rng.Next(0, this.Rows * this.Columns);
                if (!this.Tiles[position].IsMine)
                {
                    this.Tiles[position].IsMine = true;
                    nMines++;
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
