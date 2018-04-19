using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Minesweeper.Library.Test
{
   [TestFixture]
   public class TileGameLogicTest
   {
      [Test]
      public void SetNeighborMineCounts_Sets_numNeighborMines()
      {
         /* 1 1 1 - -
          * 1 m 2 1 -
          * 1 2 m 1 -
          * - 1 1 1 -
          * - - - - -
          */

         var tiles = new List<Tile>();
         for (int i = 0; i < 25; i++)
         {
            tiles.Add(new Tile(i));
         }
         tiles[6].IsMine = true;
         tiles[12].IsMine = true;
         
         var nColumns = 5;
         var nTiles = 25;

         var tileLogic = new TileGameLogic(new NeighboringTileFinder());
         tileLogic.SetNeighborMineCounts(tiles, nColumns);

         Assert.AreEqual(1, tiles[0].NumNeighborMines);
         Assert.AreEqual(1, tiles[1].NumNeighborMines);
         Assert.AreEqual(1, tiles[2].NumNeighborMines);
         Assert.AreEqual(1, tiles[5].NumNeighborMines);
         Assert.AreEqual(1, tiles[8].NumNeighborMines);
         Assert.AreEqual(1, tiles[10].NumNeighborMines);
         Assert.AreEqual(1, tiles[13].NumNeighborMines);
         Assert.AreEqual(1, tiles[16].NumNeighborMines);
         Assert.AreEqual(1, tiles[17].NumNeighborMines);
         Assert.AreEqual(1, tiles[18].NumNeighborMines);
         Assert.AreEqual(0, tiles[24].NumNeighborMines);

         Assert.AreEqual(2, tiles[7].NumNeighborMines);
         Assert.AreEqual(2, tiles[11].NumNeighborMines);
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

         var tiles = new List<Tile>();
         for (int i = 0; i < 25; i++)
         {
            tiles.Add(new Tile(i));
         }
         tiles[6].IsMine = true;
         tiles[12].IsMine = true;
         var nColumns = 5;
         var nTiles = 25;

         var settings = new DifficultySetting(DifficultyLevel.Easy) {Mines = 2};
         var mockGame = new Mock<IGame>();
         mockGame.Setup(g => g.TilesLeft).Returns(nTiles);
         mockGame.Setup(g => g.Settings).Returns(settings);

         var tileLogic = new TileGameLogic(new NeighboringTileFinder());
         tileLogic.SetNeighborMineCounts(tiles, nColumns);
         tileLogic.RevealTiles(tiles[24], tiles, nColumns, mockGame.Object);

         Assert.AreEqual(20, tiles.Count(t => t.IsRevealed));
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

         var tiles = new List<Tile>();
         for (int i = 0; i < 25; i++)
         {
            tiles.Add(new Tile(i));
         }
         var nColumns = 5;
         var nTiles = 25;

         tiles[6].IsMine = true;
         tiles[12].IsMine = true;
         tiles[24].IsMine = true;

         var settings = new DifficultySetting(DifficultyLevel.Easy) { Mines = 2 };
         var mockGame = new Mock<IGame>();
         mockGame.Setup(g => g.TilesLeft).Returns(nTiles);
         mockGame.Setup(g => g.Settings).Returns(settings);

         var tileLogic = new TileGameLogic(new NeighboringTileFinder());
         tileLogic.SetNeighborMineCounts(tiles, nColumns);

         tileLogic.RevealTiles(tiles[18], tiles, nColumns, mockGame.Object);
         Assert.AreEqual(1, tiles.Count(t => t.IsRevealed));
         tiles[18].IsRevealed = false;

         tileLogic.RevealTiles(tiles[24], tiles, nColumns, mockGame.Object);
         Assert.AreEqual(1, tiles.Count(t => t.IsRevealed));
      }

      [Test]
      public void RevealTile_DoesNot_Set_IsRevealed_If_IsMarked()
      {
         var tileLogic = new TileGameLogic(Mock.Of<INeighboringTileFinder>());
         var tiles = new List<Tile>() {new Tile()};
         tiles[0].IsMarked = true;

         tileLogic.RevealTiles(tiles[0], tiles, 89 ,Mock.Of<IGame>());
         Assert.IsFalse(tiles[0].IsRevealed);
      }

      [Test]
      public void RevealAllNonMineTiles_Sets_Win_True()
      {
         var tiles = new List<Tile>
         {
            new Tile { IsMine = true, NumNeighborMines = 0},
            new Tile { IsMine = false, NumNeighborMines = 1}
         };

         var settings = new DifficultySetting(DifficultyLevel.Easy) {Mines=1};
         var mockGame = new Mock<IGame>();
         mockGame.Setup(g => g.Settings).Returns(settings);
         mockGame.Setup(g => g.TilesLeft).Returns(1);
         mockGame.Setup(g => g.EndGame(true));

         var tileGameLogic = new TileGameLogic(Mock.Of<INeighboringTileFinder>());
         tileGameLogic.RevealTiles(tiles[1], tiles, 2, mockGame.Object);

         mockGame.Verify(g => g.EndGame(true), Times.Once);
      }

      [Test]
      public void RevealMine_Sets_GameOver_True()
      {
         var tiles = new List<Tile>
         {
            new Tile { IsMine = true, NumNeighborMines = 0},
            new Tile { IsMine = false, NumNeighborMines = 1}
         };

         var settings = new DifficultySetting(DifficultyLevel.Easy) { Mines = 1 };
         var mockGame = new Mock<IGame>();
         mockGame.Setup(g => g.Settings).Returns(settings);
         mockGame.Setup(g => g.TilesLeft).Returns(2);
         mockGame.Setup(g => g.EndGame(false));

         var tileGameLogic = new TileGameLogic(Mock.Of<INeighboringTileFinder>());
         tileGameLogic.RevealTiles(tiles[0], tiles, 2, mockGame.Object);

         mockGame.Verify(g => g.EndGame(false), Times.Once);
      }

      [Test]
      public void ToggleTileMarked_Sets_NumMines_Appropriately()
      {
         var nMines = 5;
         var tiles = new List<Tile>();
         for (var i = 0; i < nMines; i++)
            tiles.Add(new Tile() { IsMine = true });

         var mockGame = new Mock<IGame>();
         mockGame.Setup(g => g.NumMines).Returns(nMines);
         mockGame.Setup(g => g.DecrementMineCounter()).Callback(() => nMines--);
         mockGame.Setup(g => g.IncrementMineCounter()).Callback(() => nMines++);

         var tileGameLogic = new TileGameLogic(Mock.Of<INeighboringTileFinder>());

         tileGameLogic.ToggleTileMarked(tiles[1], mockGame.Object);
         Assert.AreEqual(4, nMines);
         tileGameLogic.ToggleTileMarked(tiles[1], mockGame.Object);
         Assert.AreEqual(5, nMines);
      }
   }
}
