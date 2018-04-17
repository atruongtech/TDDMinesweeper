using NUnit.Framework;
using System.Collections.Generic;
using Prism.Mvvm;
using System.ComponentModel;
using Moq;

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
            Gameboard board = new Gameboard(level, Mock.Of<INeighboringTileFinder>());
            
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
            Gameboard board = new Gameboard(level, Mock.Of<INeighboringTileFinder>());

            Assert.IsTrue(board.Tiles.Count == settings.Rows * settings.Columns);
        }

        [Test]
        public void SeedMines_Sets_ProperNumberOfMines(
            [Values(DifficultyLevel.Easy,
            DifficultyLevel.Medium,
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);

            Gameboard board = new Gameboard(level, Mock.Of<INeighboringTileFinder>());

            int realNMines = 0;
            foreach(Tile tile in board.Tiles)
            {
                if (tile.IsMine)
                    realNMines++;
            }

            Assert.AreEqual(settings.Mines, realNMines);
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

            var gameboard = new Gameboard(DifficultyLevel.Easy, new NeighboringTileFinder())
            {
               Tiles = new List<Tile>()
            };

            for (int i = 0; i < 25; i++)
            {
               gameboard.Tiles.Add(new Tile(i));
            }
            gameboard.Tiles[6].IsMine = true;
            gameboard.Tiles[12].IsMine = true;

            gameboard.SetNeighborMineCounts(gameboard.Tiles, 5);

            Assert.AreEqual(1, gameboard.Tiles[0].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[1].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[2].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[5].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[8].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[10].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[13].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[16].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[17].NumNeighborMines);
            Assert.AreEqual(1, gameboard.Tiles[18].NumNeighborMines);
            Assert.AreEqual(0, gameboard.Tiles[24].NumNeighborMines);

            Assert.AreEqual(2, gameboard.Tiles[7].NumNeighborMines);
            Assert.AreEqual(2, gameboard.Tiles[11].NumNeighborMines);
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

            Gameboard board = new Gameboard(DifficultyLevel.Easy, new NeighboringTileFinder());
            board.Columns = 5;
            board.Rows = 5;
            board.Tiles = new List<Tile>();
            for (int i = 0; i < 25; i++)
            {
                board.Tiles.Add(new Tile(i));
            }

            board.Tiles[6].IsMine = true;
            board.Tiles[12].IsMine = true;
            board.SetNeighborMineCounts(board.Tiles, board.Columns);

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

            Gameboard board = new Gameboard(DifficultyLevel.Easy, new NeighboringTileFinder());
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
            board.SetNeighborMineCounts(board.Tiles, board.Columns);

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
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
            Assert.IsNotNull(board.RevealCommand);
        }
        
        [Test]
        public void Constructor_Sets_NumMines_Appropriately(
            [Values(DifficultyLevel.Easy, 
            DifficultyLevel.Medium, 
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting setting = new DifficultySetting(level);
            Gameboard board = new Gameboard(level, Mock.Of<INeighboringTileFinder>());

            Assert.AreEqual(board.NumMines, setting.Mines);
        }

        [Test]
        public void ToggleTileMarked_Sets_NumMines_Appropriately()
        {
            DifficultySetting setting = new DifficultySetting(DifficultyLevel.Easy);
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
            board.InitializeGameBoard();
            board.ToggleTileMarked(board.Tiles[1]);

            Assert.AreEqual(setting.Mines - 1, board.NumMines);

            board.ToggleTileMarked(board.Tiles[1]);
            Assert.AreEqual(setting.Mines, board.NumMines);
        }

        [Test]
        public void ToggleTileMarked_DoesNot_Set_Marked_If_NoMinesLeft()
        {
            DifficultySetting setting = new DifficultySetting(DifficultyLevel.Easy);
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());

            board.NumMines = 0;
            Assert.IsFalse(board.Tiles[1].IsMarked);

            board.ToggleTileMarked(board.Tiles[1]);
            Assert.IsFalse(board.Tiles[1].IsMarked);
            Assert.Zero(board.NumMines);
        }

        [Test]
        public void RevealTile_DoesNot_Set_IsRevealed_If_IsMarked()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
            board.ToggleTileMarked(board.Tiles[1]);

            board.RevealTiles(board.Tiles[1]);
            Assert.IsFalse(board.Tiles[1].IsRevealed);
        }

        [Test]
        public void RevealMine_Sets_GameOver_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
            board.InitializeGameBoard();
            board.StartGame();
            board.Tiles[1].SetIsMine();

            board.RevealTiles(board.Tiles[1]);
            Assert.IsTrue(board.Tiles[1].IsRevealed);
            Assert.IsTrue(board.GameOver);
        }

        [Test]
        public void PropertyChanged_EventFires_When_GameOver_Set_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
            bool eventFired = false;
            ((BindableBase)board).PropertyChanged += 
                new PropertyChangedEventHandler((s, e) => { if (e.PropertyName == "GameOver") { eventFired = true; } });

            board.GameOver = true;
            Assert.AreEqual(true, eventFired);
        }

        [Test]
        public void RevealAllNonMineTiles_Sets_Win_True()
        {
            Gameboard board = new Gameboard(DifficultyLevel.Easy, new NeighboringTileFinder());
            board.InitializeGameBoard();
            board.StartGame();
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
            Gameboard board = new Gameboard(DifficultyLevel.Easy, Mock.Of<INeighboringTileFinder>());
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
            Gameboard board = new Gameboard(DifficultyLevel.Easy, new NeighboringTileFinder());
            board.InitializeGameBoard();
            board.StartGame();
            System.Threading.Thread.Sleep(4000);
            Assert.NotZero(board.PlayTime);
        }
    }
}
