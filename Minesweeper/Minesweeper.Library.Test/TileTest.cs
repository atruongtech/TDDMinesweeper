using NUnit.Framework;

namespace Minesweeper.Library.Test
{
    [TestFixture]
    public class TileTest
    {
        [Test]
        public void DefaultTile_IsNot_Mine()
        {
            Tile tile = new Tile();
            Assert.IsFalse(tile.IsMine);
        }

        [Test]
        public void Constructor_Sets_TileIndex()
        {
            Tile tile = new Tile(0);
            Assert.AreEqual(0, tile.TileIndex);
        }

        [Test]
        public void SetIsMine_Sets_IsMine_True()
        {
            Tile tile = new Tile();
            tile.SetIsMine();
            Assert.IsTrue(tile.IsMine);
        }

        [Test]
        public void SetNeighborMines_Sets_NumNeighborMines(
            [Values(0, 1, 2, 3, 4, 5, 6, 7, 8)] int input)
        {
            Tile tile = new Tile();

            tile.SetNeighborMines(input);
            Assert.AreEqual(input, tile.NumNeighborMines);
        }

        [Test]
        public void Reveal_Sets_IsRevealed_True()
        {
            Tile tile = new Tile();
            tile.Reveal();

            Assert.IsTrue(tile.IsRevealed);
        }

        [Test]
        public void ToggleMark_Toggles_IsMarked()
        {
            Tile tile = new Tile();
            Assert.IsFalse(tile.IsMarked);

            tile.ToggleMark();
            Assert.IsTrue(tile.IsMarked);

            tile.ToggleMark();
            Assert.IsFalse(tile.IsMarked);
        }
        
    }
}
