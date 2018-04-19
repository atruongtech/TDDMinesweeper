using NUnit.Framework;
using System.Collections.Generic;
using Prism.Mvvm;
using System.ComponentModel;
using Moq;
using System.Linq;

namespace Minesweeper.Library.Test
{
   [TestFixture]
    public class GameboardTest
    {
        [Test]
        public void Constructor_Sets_RowsAndCols(
            [Values(DifficultyLevel.Easy, 
            DifficultyLevel.Medium, 
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);
            Gameboard gameboard = new Gameboard(settings);
            
            Assert.AreEqual(settings.Rows, gameboard.Rows);
            Assert.AreEqual(settings.Columns, gameboard.Columns);
        }

        [Test]
        public void CreateTiles_Populates_Tiles(
            [Values(DifficultyLevel.Easy,
            DifficultyLevel.Medium,
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);
            var gameboard = new Gameboard(settings);

            Assert.IsTrue(gameboard.Tiles.Count == settings.Rows * settings.Columns);
        }

        [Test]
        public void SeedMines_Sets_ProperNumberOfMines(
            [Values(DifficultyLevel.Easy,
            DifficultyLevel.Medium,
            DifficultyLevel.Hard)] DifficultyLevel level)
        {
            DifficultySetting settings = new DifficultySetting(level);
            Gameboard board = new Gameboard(settings);

            Assert.AreEqual(settings.Mines, board.Tiles.Count(t => t.IsMine));
        }

        [Test]
        public void PropertyChanged_EventFires_When_GameOver_Set_True()
        {
            var gameboard = new Gameboard(new DifficultySetting(DifficultyLevel.Easy));
            Game board = new Game(Mock.Of<ITileGameLogic>(), gameboard);
            bool eventFired = false;
            board.PropertyChanged += 
                (s, e) => { if (e.PropertyName == "GameOver") { eventFired = true; } };

            board.EndGame(false);
            Assert.AreEqual(true, eventFired);
        }

    }
}
