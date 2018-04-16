using NUnit.Framework;
using System.Collections.Generic;
using Prism.Mvvm;
using System.ComponentModel;

namespace Minesweeper.Library.Test
{
   [TestFixture]
    public class GameboardTest
    {
        [Test]
        public void SetDimensions_Sets_RowsAndCols(
            [Values(DifficultyLevel.Easy, 
            DifficultyLevel.Medium, 
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);
            Gameboard board = new Gameboard(level);
            
            Assert.AreEqual(settings.Rows, board.Rows);
            Assert.AreEqual(settings.Columns, board.Columns);
        }

        [Test]
        public void CreateTiles_Populates_Tiles(
            [Values(DifficultyLevel.Easy,
            DifficultyLevel.Medium,
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);
            Gameboard board = new Gameboard(level);

            Assert.IsTrue(board.Tiles.Count == settings.Rows * settings.Columns);
        }

        [Test]
        public void SeedMines_Sets_ProperNumberOfMines(
            [Values(DifficultyLevel.Easy,
            DifficultyLevel.Medium,
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);

            Gameboard board = new Gameboard(level);

            int realNMines = 0;
            foreach(Tile tile in board.Tiles)
            {
                if (tile.IsMine)
                    realNMines++;
            }

            Assert.AreEqual(settings.Mines, realNMines);
        }

        [Test]
        public void Easy_GetAllNeighbors_Edges_Returns_AppropriateNeighbors(
            [Values(23, 24, 60, 4, 8, 55, 62, 1)] int index)
        {
            /* - 8 - - 4 - - -
             * 5 - - - - - - -
             * - - - - - - - 1
             * 2 - - - - - - -
             * - - - - - - - -
             * - - - - - - - -
             * - - - - - - - 6
             * - - - - 3 - 7 -
             */

            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            List<Tile> neighbors;

            neighbors = Gameboard.GetAllNeighbors(index, board.Tiles, board.Columns);
            Assert.AreEqual(5, neighbors.Count);
        }

        [Test]
        public void Easy_GetAllNeighbors_Corners_Returns_AppropriateNeighbors(
            [Values(0, 63, 7, 56)] int index)
        {
            /* 1 - - - - - - 3
             * - - - - - - - -
             * - - - - - - - -
             * - - - - - - - -
             * - - - - - - - -
             * - - - - - - - -
             * - - - - - - - -
             * 4 - - - - - - 2
             */

            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            List<Tile> neighbors;

            neighbors = Gameboard.GetAllNeighbors(index, board.Tiles, board.Columns);
            Assert.AreEqual(3, neighbors.Count);
        }

        [Test]
        public void Easy_GetAllNeighbors_General_Returns_AppropriateNeighbors(
            [Values(27, 33, 36, 9, 54, 14, 49 )] int index)
        {
            /* - - - - - - - -
             * - 4 - - - - 6 -
             * - - - - - - - -
             * - - - - 1 - - -
             * - - 2 - - 3 - -
             * - - - - - - - -
             * - 7 - - - - 5 -
             * - - - - - - - -
             */

            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            List<Tile> neighbors;
            
            neighbors = Gameboard.GetAllNeighbors(index, board.Tiles, board.Columns);
            var neighbor = board.Tiles[index + 1];

            Assert.IsTrue(neighbors.Contains(neighbor));
            Assert.AreEqual(8, neighbors.Count);
        }

        [Test]
        public void SetNeighborMineCounts_Sets_numNeighborMines()
        {
            /* 1 1 1 - -
             * 1 m 2 1 -
             * 1 2 m 1 -
             * - 1 1 1 -
             * - - - - -
             */

            List<Tile> boardTiles = new List<Tile>();
            for (int i = 0; i < 25; i++)
            {
                boardTiles.Add(new Tile(i));
            }
            boardTiles[6].IsMine = true;
            boardTiles[12].IsMine = true;

            Gameboard.SetNeighborMineCounts(boardTiles, 5);

            Assert.AreEqual(1, boardTiles[0].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[1].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[2].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[5].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[8].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[10].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[13].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[16].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[17].NumNeighborMines);
            Assert.AreEqual(1, boardTiles[18].NumNeighborMines);
            Assert.AreEqual(0, boardTiles[24].NumNeighborMines);

            Assert.AreEqual(2, boardTiles[7].NumNeighborMines);
            Assert.AreEqual(2, boardTiles[11].NumNeighborMines);
        }

        [Test]
        public void RevealTiles_Sets_IsRevealed_On0_AndNeighbors()
        {
            /* 1 1 1 - -
             * 1 m 2 1 -
             * 1 2 m 1 -
             * - 1 1 1 -
             * - - - - -
             */

            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            board.Columns = 5;
            board.Rows = 5;
            board.Tiles = new List<Tile>();
            for (int i = 0; i < 25; i++)
            {
                board.Tiles.Add(new Tile(i));
            }

            board.Tiles[6].IsMine = true;
            board.Tiles[12].IsMine = true;
            Gameboard.SetNeighborMineCounts(board.Tiles, board.Columns);

            board.RevealTiles(board.Tiles[24]);

            int revealedCount = 0;
            foreach(Tile tile in board.Tiles)
            {
                if (tile.IsRevealed)
                    revealedCount++;
            }

            Assert.AreEqual(20, revealedCount);
        }

        [Test]
        public void RevealTiles_Sets_IsRevealed_OnlyOnTile()
        {
            /* 1 1 1 - -
             * 1 m 2 1 -
             * 1 2 m 1 -
             * - 1 1 2 1
             * - - - 1 m
             */

            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            board.Columns = 5;
            board.Rows = 5;
            board.Tiles = new List<Tile>();
            for (int i = 0; i < 25; i++)
            {
                board.Tiles.Add(new Tile(i));
            }

            board.Tiles[6].IsMine = true;
            board.Tiles[12].IsMine = true;
            board.Tiles[24].IsMine = true;
            Gameboard.SetNeighborMineCounts(board.Tiles, board.Columns);

            board.RevealTiles(board.Tiles[18]);

            int revealedCount = 0;
            foreach (Tile tile in board.Tiles)
            {
                if (tile.IsRevealed)
                    revealedCount++;
            }

            Assert.AreEqual(1, revealedCount);


            board.Tiles[18].IsRevealed = false;

            board.RevealTiles(board.Tiles[24]);
            revealedCount = 0;
            foreach (Tile tile in board.Tiles)
            {
                if (tile.IsRevealed)
                    revealedCount++;
            }

            Assert.AreEqual(1, revealedCount);
        }

        [Test]
        public void Constructor_Sets_RevealCommand()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            Assert.IsNotNull(board.RevealCommand);
        }
        
        [Test]
        public void Constructor_Sets_NumMines_Appropriately(
            [Values(DifficultyLevel.Easy, 
            DifficultyLevel.Medium, 
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting setting = new DifficultySetting(level);
            Gameboard board = new Gameboard(level);

            Assert.AreEqual(board.NumMines, setting.Mines);
        }

        [Test]
        public void ToggleTileMarked_Sets_NumMines_Appropriately()
        {
            DifficultySetting setting = new DifficultySetting(DifficultyLevel.Easy);
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            board.ToggleTileMarked(board.Tiles[1]);

            Assert.AreEqual(setting.Mines - 1, board.NumMines);

            board.ToggleTileMarked(board.Tiles[1]);
            Assert.AreEqual(setting.Mines, board.NumMines);
        }

        [Test]
        public void ToggleTileMarked_DoesNot_Set_Marked_If_NoMinesLeft()
        {
            DifficultySetting setting = new DifficultySetting(DifficultyLevel.Easy);
            Gameboard board = new Gameboard(DifficultyLevel.Easy);

            board.NumMines = 0;
            Assert.IsFalse(board.Tiles[1].IsMarked);

            board.ToggleTileMarked(board.Tiles[1]);
            Assert.IsFalse(board.Tiles[1].IsMarked);
            Assert.Zero(board.NumMines);
        }

        [Test]
        public void RevealTile_DoesNot_Set_IsRevealed_If_IsMarked()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            board.ToggleTileMarked(board.Tiles[1]);

            board.RevealTiles(board.Tiles[1]);
            Assert.IsFalse(board.Tiles[1].IsRevealed);
        }

        [Test]
        public void RevealMine_Sets_GameOver_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            board.Tiles[1].SetIsMine();

            board.RevealTiles(board.Tiles[1]);
            Assert.IsTrue(board.Tiles[1].IsRevealed);
            Assert.IsTrue(board.GameOver);
        }

        [Test]
        public void PropertyChanged_EventFires_When_GameOver_Set_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            bool eventFired = false;
            ((BindableBase)board).PropertyChanged += 
                new PropertyChangedEventHandler((s, e) => { if (e.PropertyName == "GameOver") { eventFired = true; } });

            board.GameOver = true;
            Assert.AreEqual(true, eventFired);
        }

        [Test]
        public void RevealAllNonMineTiles_Sets_Win_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            foreach(Tile tile in board.Tiles)
            {
                if (!tile.IsMine)
                    board.RevealTiles(tile);
            }
            Assert.IsTrue(board.Win);
        }

        [Test]
        public void PropertyChanged_EventFires_When_Win_Set_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            bool eventFired = false;
            ((BindableBase)board).PropertyChanged +=
                new PropertyChangedEventHandler(
                    (s, e) =>
                    {
                        if (e.PropertyName == "Win")
                        {
                            eventFired = true;
                        }
                    });

            board.Win = true;
            Assert.AreEqual(true, eventFired);
        }

        [Test]
        public void PlayTimer_Sets_PlayTime()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy);
            System.Threading.Thread.Sleep(4000);
            Assert.NotZero(board.PlayTime);
        }
    }
}
