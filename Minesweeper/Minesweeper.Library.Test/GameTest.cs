using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Minesweeper.Library.Test
{
   [TestFixture]
   public class GameTest
   {
      [Test]
      public void Constructor_Sets_RevealCommand()
      {
         var gameboard = new Gameboard(new DifficultySetting(DifficultyLevel.Easy));
         Game board = new Game(Mock.Of<ITileGameLogic>(), gameboard);
         Assert.IsNotNull(board.RevealCommand);
      }

      [Test]
      public void PlayTimer_Sets_PlayTime()
      {
         var gameboard = new Gameboard(new DifficultySetting(DifficultyLevel.Easy));
         Game board = new Game(Mock.Of<ITileGameLogic>(), gameboard);
         board.StartGame();
         System.Threading.Thread.Sleep(4000);
         Assert.NotZero(board.PlayTime);
      }

      [Test]
      public void PropertyChanged_EventFires_When_Win_Set_True()
      {
         var gameboard = new Gameboard(new DifficultySetting(DifficultyLevel.Easy));
         Game board = new Game(Mock.Of<ITileGameLogic>(), gameboard);
         bool eventFired = false;
         board.PropertyChanged +=
            (s, e) =>
            {
               if (e.PropertyName == "Win")
               {
                  eventFired = true;
               }
            };

         board.EndGame(true);
         Assert.AreEqual(true, eventFired);
      }

      [Test]
      public void ToggleTileMarked_DoesNot_Set_Marked_If_NoMinesLeft()
      {
         DifficultySetting setting = new DifficultySetting(DifficultyLevel.Easy) { Mines = 0 };
         var tiles = new List<Tile> { new Tile { IsMine = true }, new Tile { IsMine = false } };
         var mockGameboard = new Mock<IGameboard>();
         mockGameboard.Setup(gb => gb.Tiles).Returns(tiles);
         mockGameboard.Setup(gb => gb.Settings).Returns(setting);
         Game game = new Game(Mock.Of<ITileGameLogic>(), mockGameboard.Object);

         var i = 0;
         while (game.NumMines > 0)
         {
            game.ToggleTileMarked(game.Tiles[i]);
            i++;
         }

         game.ToggleTileMarked(game.Tiles[i]);
         Assert.IsFalse(game.Tiles[i].IsMarked);
         Assert.Zero(game.NumMines);
      }
   }
}
