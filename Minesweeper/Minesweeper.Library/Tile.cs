﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Minesweeper.Library
{
    public class Tile : BindableBase
    {
        private bool _isMine;
        private int _numNeighborMines;
        private bool _isRevealed;
        private int _tileIndex;


        #region properties
        public bool IsMine
        {
            get { return _isMine; }
            set {
                SetProperty<bool>(ref this._isMine, value);
            }
        }

        public int TileIndex
        {
            get { return _tileIndex; }
            set { SetProperty(ref this._tileIndex, value); }
        }

        public int NumNeighborMines
        {
            get { return _numNeighborMines; }
            set { SetProperty<int>(ref this._numNeighborMines, value); }
        }

        public bool IsRevealed
        {
            get { return _isRevealed; }
            set { SetProperty<bool>(ref this._isRevealed, value); }
        }

        #endregion

        public Tile()
        {

        }

        public Tile(int index)
        {
            this.TileIndex = index;
        }

        public void SetIsMine()
        {
            this.IsMine = true;
        }

        public void SetNeighborMines(int input)
        {
            this.NumNeighborMines = input;
        }

        public void Reveal()
        {
            this.IsRevealed = true;
        }
    }
}
