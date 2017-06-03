using System;
using NUnit.Framework;
using Minesweeper.Library;
using System.Collections.Generic;

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
    }
}
