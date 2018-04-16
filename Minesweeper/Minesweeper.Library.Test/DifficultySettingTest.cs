using NUnit.Framework;

namespace Minesweeper.Library.Test
{
    [TestFixture]
    public class DifficultySettingTest
    {
        [Test]
        public void EasyConstructor_Sets_Properties()
        {
            DifficultySetting settings = new DifficultySetting(DifficultyLevel.Easy);

            Assert.IsTrue(settings.Mines == 10);
            Assert.IsTrue(settings.Rows == 8);
            Assert.IsTrue(settings.Columns == 8);
        }

        [Test]
        public void MediumConstructor_Sets_Properties()
        {
            DifficultySetting settings = new DifficultySetting(DifficultyLevel.Medium);

            Assert.IsTrue(settings.Mines == 40);
            Assert.IsTrue(settings.Rows == 16);
            Assert.IsTrue(settings.Columns == 16);
        }

        [Test]
        public void HardConstructor_Sets_Properties()
        {
            DifficultySetting settings = new DifficultySetting(DifficultyLevel.Hard);

            Assert.IsTrue(settings.Mines == 99);
            Assert.IsTrue(settings.Rows == 24);
            Assert.IsTrue(settings.Columns == 24);
        }
    }
}
